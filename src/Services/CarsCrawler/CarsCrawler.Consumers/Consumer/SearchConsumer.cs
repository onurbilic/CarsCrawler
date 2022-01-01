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
using CarsCrawler.SharedBusiness.CefSharp;

namespace CarsCrawler.Consumers.Consumer
{
    public class SearchConsumer : IConsumer<ISearchCarsCommand>
    {
        private readonly IMongoRepository<Vehicle> _mongo;
        private readonly IBus _bus;

        public SearchConsumer(IMongoRepository<Vehicle> mongo, IBus bus)
        {
            _mongo = mongo;
            _bus = bus;
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
                        throw new Exception(
                            $"Page load failed with ErrorCode:{initialLoadResponse.ErrorCode}, HttpStatusCode:{initialLoadResponse.HttpStatusCode}");
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
                    await Task.Delay(10000);

                    if (response.Success)
                    {
                        for (int i = 0; i < search.PageCount; i++)
                        {
                            //you can also navigate page to click the page link at first page. But I want to prefer go with url navigation.
                            //TODO: Click on page link.
                            var navigatedPage = search.PageStart + i;
                            var navigatedUrl = $@"{Consts.testUrl}shopping/results/?
                                                page={navigatedPage.ToString()}&
                                                page_size=20&
                                                list_price_max={search.Price}&
                                                makes[]={search.Makes}&
                                                maximum_distance={search.Distance}&
                                                models[]={search.Models}&
                                                stock_type={search.StockType}&
                                                zip={search.Zip}";
                            browser.Load(navigatedUrl);
                            var initialnewLoadResponse = await browser.WaitForInitialLoadAsync();

                            if (!initialnewLoadResponse.Success)
                            {
                                throw new Exception(
                                    $"Page load failed with ErrorCode:{initialLoadResponse.ErrorCode}, HttpStatusCode:{initialLoadResponse.HttpStatusCode}");
                            }

                            var vehicleCard = await browser.EvaluateScriptAsync(
                                HtmlValueHelper.SetHtmlValue("vehicle-card", string.Empty,
                                    HtmlSelector.getVehicleCard));

                            Console.WriteLine(vehicleCard);
                            List<Vehicle> vehicles = new List<Vehicle>();
                            foreach (dynamic item in (IEnumerable) vehicleCard.Result)
                            {
                                var vehicle = new Vehicle()
                                {
                                    image = ((dynamic) item).image,
                                    miles = ((dynamic) item).miles,
                                    price = ((dynamic) item).price,
                                    rating = ((dynamic) item).rating,
                                    title = ((dynamic) item).title,
                                    carId = ((dynamic) item).id,
                                    dealerName = ((dynamic) item).dealerName,
                                    reportLink = ((dynamic) item).reportLink,
                                    stockType = ((dynamic) item).stockType,
                                    Status = 1
                                };
                                vehicles.Add(vehicle);
                            }
                            //we created status for outbox pattern, may we can change status for reusable scrapping
                            //also we can send command through rabbit queue. We should not write the database. But here we use outbox pattern
                            await _mongo.InsertManyAsync(vehicles);
                        }
                    }
                }
            });
        }
    }

    public class SearchConsumerDefinition : ConsumerDefinition<SearchConsumer>
    {
        public SearchConsumerDefinition()
        {
            EndpointName = Consts.SearchCarsCommand;
        }
    }
}