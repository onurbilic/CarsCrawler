using System.Diagnostics;
using CarsCrawler.Domain.Model;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using CarsCrawler.SharedBusiness.Commands;
using CefSharp;
using CefSharp.OffScreen;
using CarsCrawler.Consumers.CefCrawler;
using CarsCrawler.Infrastructure.Utils;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using System.Collections;

namespace CarsCrawler.Consumers.Consumer
{
    public class SearchConsumer : IConsumer<ISearchCarsCommand>
    {
        private readonly IMongoRepository<Vehicle> _mongo;

        public SearchConsumer(IMongoRepository<Vehicle> mongo)
        {
            _mongo = mongo;
        }

        public Task Consume(ConsumeContext<ISearchCarsCommand> context)
        {
            Console.WriteLine(context);

            SearchResult(context.Message);

            return Task.CompletedTask;
        }

        private void SearchResult(ISearchCarsCommand search)
        {
            AsyncContext.Run(async delegate
            {
                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "CefSharp\\Cache1")
                };

                var success =
                    await Cef.InitializeAsync(settings, performDependencyCheck: true, browserProcessHandler: null);

                if (!success)
                {
                    throw new Exception("Unable to initialize CEF, check the log file.");
                }

                using (var browser = new ChromiumWebBrowser(Consts.testUrl))
                {
                    var initialLoadResponse = await browser.WaitForInitialLoadAsync();

                    if (!initialLoadResponse.Success)
                    {
                        throw new Exception(string.Format("Page load failed with ErrorCode:{0}, HttpStatusCode:{1}",
                            initialLoadResponse.ErrorCode, initialLoadResponse.HttpStatusCode));
                    }

                    _ = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue("stock_type", search.StockType, HtmlSelector.name));
                    await Task.Delay(100);
                    _ = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue("makes", search.Makes, HtmlSelector.id));
                    await Task.Delay(1000);
                    _ = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue("models", search.Models, HtmlSelector.select));
                    await Task.Delay(1000);
                    _ = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue("list_price_max", search.Price, HtmlSelector.name));
                    await Task.Delay(100);
                    _ = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue("maximum_distance", search.Distance, HtmlSelector.name));
                    await Task.Delay(100);
                    _ = await browser.EvaluateScriptAsync(
                       HtmlValueHelper.SetHtmlValue("zip", search.Zip, HtmlSelector.name));
                    await Task.Delay(100);
                    var response = await browser.EvaluateScriptAsync(
                    HtmlValueHelper.SetHtmlValue("search-form", "", HtmlSelector.submitFormClassName));
                    await Task.Delay(3000);

                    if (response.Success)
                    {
                        await Task.Delay(5000);

                        var vehicleCard = await browser.EvaluateScriptAsync(
                            HtmlValueHelper.SetHtmlValue("vehicle-card", string.Empty, HtmlSelector.getVehicleCard));

                        Console.WriteLine(vehicleCard);
                        List<Vehicle> vehicles = new List<Vehicle>();
                        foreach (dynamic item in (IEnumerable)vehicleCard.Result)
                        {
                            var vehicle = new Vehicle()
                            {
                                image = ((dynamic)item).image,
                                miles = ((dynamic)item).miles,
                                price = ((dynamic)item).price,
                                rating = ((dynamic)item).rating,
                                title = ((dynamic)item).title,
                                carId = ((dynamic)item).id,
                                dealerName = ((dynamic)item).dealerName,
                                reportLink = ((dynamic)item).reportLink,
                                stockType = ((dynamic)item).stockType

                            };
                            vehicles.Add(vehicle);
                        }

                        _mongo.InsertMany(vehicles);
                    }
                }

                Cef.Shutdown();
            });
        }
    }

    public class SearchConsumerDefinition : ConsumerDefinition<SearchConsumer>
    {
        public SearchConsumerDefinition()
        {
            EndpointName = "In.Carsdotcom.Search";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<SearchConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = true;
            //endpointConfigurator.ClearMessageDeserializers();
            //endpointConfigurator.UseRawJsonSerializer();
        }
    }
}