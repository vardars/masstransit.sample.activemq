using System.Threading.Tasks;
using MassTransit;
using MassTransit.Samples.ConsumerSample;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<SimpleMessageConsumer, SimpleMessageConsumerDescription>();

                    x.UsingActiveMq((context, cfg) =>
                    {
                        cfg.Host("localhost", h =>
                        {
                            h.Username("admin");
                            h.Password("admin");
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });
            })
            .Build()
            .RunAsync();
    }
}