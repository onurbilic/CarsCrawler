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
using System.Collections;

namespace CarsCrawler.Consumers.Consumer
{
    public class CarDetailConsumer : IConsumer<IVehicleDetailCommand>
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
              
                var navigatedUrl = $@"{Consts.TestUrl}vehicledetail/{vehicle.VehicleId}/";
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
                    await Task.Delay(1000);

                    if (vehicleDetail.Success)
                    {
                        var result = vehicleDetail.Result;
                        List<BasicInfoItem> basics = new List<BasicInfoItem>();
                        foreach (dynamic item in (IEnumerable) ((dynamic)result).basicInfo)
                        {
                            var basicInfo = new BasicInfoItem()
                            {
                                key = item.key,
                                value = item.value
                            };
                            basics.Add(basicInfo);
                        }

                        List<FutureInfoItem> futures = new List<FutureInfoItem>();
                        foreach (dynamic item in (IEnumerable)((dynamic)result).featuresInfo)
                        {
                            var futureInfo = new FutureInfoItem()
                            {
                                key = item.key,
                                value = item.value
                            };
                            futures.Add(futureInfo);
                        }

                        var detail = new VehicleDetail
                        {
                            carId = vehicle.VehicleId,
                            stockType = ((dynamic)result).stockType,
                            title = ((dynamic)result).title,
                            mileage = ((dynamic)result).mileage,
                            price = ((dynamic)result).price,
                            basicInfo = basics,
                            futureInfo = futures,
                            sellerName = ((dynamic)result).sellerName,
                            dealerPhone = ((dynamic)result).dealerPhone,
                            rating = ((dynamic)result).rating,
                            dealerAddress = ((dynamic)result).dealerAddress,
                            extLink = ((dynamic)result).extLink,
                            sellerNotes = ((dynamic)result).sellerNotes,

                        };

                        await _mongo.InsertOneAsync(detail);
                    }
                }

            });
        }
    }

    public class CarDetailConsumerDefinition : ConsumerDefinition<CarDetailConsumer>
    {
        public CarDetailConsumerDefinition()
        {
            EndpointName = Consts.GetCarDetailCommand;
        }
    }
}