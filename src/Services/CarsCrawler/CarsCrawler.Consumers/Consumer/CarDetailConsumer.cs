using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.SharedBusiness.Commands;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace CarsCrawler.Consumers.Consumer
{
    public class CarDetailConsumer : IConsumer
    {
        private readonly IMongoRepository<VehicleDetail> _mongo;
        private readonly IBus _bus;

        public CarDetailConsumer(IMongoRepository<VehicleDetail> mongo,IBus bus)
        {
                
            _mongo = mongo;
            _bus = bus;
        }

        public Task Consume(ConsumeContext<GetCarDetailCommand.IVehicleDetailCommand> context)
        {

            
            return Task.CompletedTask;
        }
    }
    
    public class CarDetailConsumerDefinition : ConsumerDefinition<CarDetailConsumer>
    {
        public CarDetailConsumerDefinition()
        {
            EndpointName = "In.Carsdotcom.CarDetail";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CarDetailConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;
            endpointConfigurator.ClearMessageDeserializers();
            endpointConfigurator.UseRawJsonSerializer();
        }
    }
}