using MassTransit.Samples.Data;
using Microsoft.Extensions.Logging;

namespace MassTransit.Samples.ConsumerSample;

public class SimpleMessageConsumer : IConsumer<SimpleMessage>
{
    private readonly ILogger<SimpleMessageConsumer> logger;

    public SimpleMessageConsumer(ILogger<SimpleMessageConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<SimpleMessage> context)
    {
        if (context.GetRetryCount() < 2){
            return Task.FromException(new Exception());
        }

        logger.LogInformation("{date} Message Received {content}", DateTime.Now, context.Message.Content);

        return Task.CompletedTask;
    }
}

public class SimpleMessageConsumerDescription : ConsumerDefinition<SimpleMessageConsumer>
{
    public SimpleMessageConsumerDescription()
    {
        // override the default endpoint name, for whatever reason
        // EndpointName = "simple-message-queue";

        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 100;
    }
    
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SimpleMessageConsumer> consumerConfigurator, IRegistrationContext context)
    {
        consumerConfigurator.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
    }
}