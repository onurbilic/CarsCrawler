using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace CarsCrawler.Consumers.Consumer
{
    public class SearchConsumer : IConsumer
    {
        
    }
    
    public class SearchConsumerDefinition : ConsumerDefinition<CarDetailConsumer>
    {
        public SearchConsumerDefinition()
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