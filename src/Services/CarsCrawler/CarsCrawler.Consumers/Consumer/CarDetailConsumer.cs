using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace CarsCrawler.Consumers.Consumer
{
    public class CarDetailConsumer : IConsumer
    {
        private readonly IMongoRepository<VehicleDetail> _mongo;

        public CarDetailConsumer(IMongoRepository<VehicleDetail> mongo)
        {
            _mongo = mongo;
        }

        public Task Consume(ConsumeContext<string> context)
        {
            Console.WriteLine(context);

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