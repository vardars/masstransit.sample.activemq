using MassTransit.Samples.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MassTransit.Samples.ProducerSample;

public class SimpleMessageProducer : IHostedService
{
    private readonly ILogger<SimpleMessageProducer> _logger;
    private readonly IBus _bus;
    private readonly Timer _timer;

    public SimpleMessageProducer(ILogger<SimpleMessageProducer> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
        _timer = new Timer
        {
            Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
        };
        _timer.Elapsed += timerTick;
    }

    private void timerTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        var random = new Random();
        int v = random.Next(0, 9);

        var simpleMessage = new SimpleMessage{
            Content = "sample",
            Indexer = v
        };

        SendMessage(simpleMessage).Wait();
    }

    public async Task SendMessage(SimpleMessage simpleMessage)
    {
        ISendEndpoint sendEndpoint = await _bus.GetPublishSendEndpoint<SimpleMessage>();

        await sendEndpoint.Send(simpleMessage);
        
        _logger.LogInformation("Message sent at {date}", DateTime.Now);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
