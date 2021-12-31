using CarsCrawler.Consumers.CefCrawler;
using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.SharedBusiness.CefSharp;
using CarsCrawler.SharedBusiness.Commands;
using CefSharp;
using CefSharp.OffScreen;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace CarsCrawler.Consumers.Consumer
{
    public class CarDetailConsumer : IConsumer
    {
        private readonly IMongoRepository<VehicleDetail> _mongo;
        private readonly IBus _bus;

        public CarDetailConsumer(IMongoRepository<VehicleDetail> mongo, IBus bus)
        {

            _mongo = mongo;
            _bus = bus;
        }

        public Task Consume(ConsumeContext<IVehicleDetailCommand> context)
        {

            Console.WriteLine(context);

            GetVehicleDetail(context.Message);

            return Task.CompletedTask;
        }

        private void GetVehicleDetail(IVehicleDetailCommand vehicle)
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

                var navigatedUrl = $@"{Consts.testUrl}vehicledetail/{vehicle.VehicleId}/";
                using (var browser = new ChromiumWebBrowser(navigatedUrl))
                {
                    var initialLoadResponse = await browser.WaitForInitialLoadAsync();

                    if (!initialLoadResponse.Success)
                    {
                        throw new Exception(
                            $"Page load failed with ErrorCode:{initialLoadResponse.ErrorCode}, HttpStatusCode:{initialLoadResponse.HttpStatusCode}");
                    }


                    var vehicleDetail = await browser.EvaluateScriptAsync(
                        HtmlValueHelper.SetHtmlValue(string.Empty, string.Empty, HtmlSelector.getVehicleDetail));

                    VehicleDetail detail = new VehicleDetail
                    {

                    };

                    await _mongo.InsertOneAsync(detail);

                }

            });
        }

    }

    public class CarDetailConsumerDefinition : ConsumerDefinition<CarDetailConsumer>
    {
        public CarDetailConsumerDefinition()
        {
            EndpointName = Consts.GetCaretailCommand;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CarDetailConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = true;
            //endpointConfigurator.ClearMessageDeserializers();
            //endpointConfigurator.UseRawJsonSerializer();
        }
    }
}