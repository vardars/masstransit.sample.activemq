using System.Threading.Tasks;
using MassTransit;
using MassTransit.Samples.ProducerSample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddMassTransit(x =>
                {
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

                services.AddHostedService<SimpleMessageProducer>();
            })
            .ConfigureLogging(c => {
                c.AddConsole();
            })
            .Build()
            .RunAsync();
    }
}