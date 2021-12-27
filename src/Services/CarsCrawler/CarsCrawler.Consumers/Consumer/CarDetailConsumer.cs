using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace CarsCrawler.Consumers.Consumer
{
    public class CarDetailConsumer : IConsumer
    {
        
    }
    
    public class CarDetailConsumerDefinition : ConsumerDefinition<CarDetailConsumer>
    {
        public CarDetailConsumerDefinition()
        {
            EndpointName = "In.Data.CarDetail";
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