using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Configuration;

namespace RabbitMQ;

public class RabbitMqConsumer : IAsyncDisposable
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _routingKey;
    private IConnection? _connection;
    private IChannel? _channel;

    private RabbitMqConsumer(
        RabbitMqSettings settings,
        string exchangeName,
        string queueName,
        string routingKey)
    {
        _rabbitMqSettings = settings;
        _exchangeName = exchangeName;
        _queueName = queueName;
        _routingKey = routingKey;
    }

    public static async Task<RabbitMqConsumer> CreateAsync(
        RabbitMqSettings settings,
        string exchangeName,
        string queueName,
        string routingKey)
    {
        var consumer = new RabbitMqConsumer(settings, exchangeName, queueName, routingKey);
        await consumer.InitAsync();
        return consumer;
    }

    private async Task InitAsync()
    {
        _connection = await _rabbitMqSettings.Connection().CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Topic,
            durable: true
        );

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await _channel.QueueBindAsync(
            queue: _queueName,
            exchange: _exchangeName,
            routingKey: _routingKey
        );
    }

    public async Task StartAsync(Func<string, string, Task> handleMessageAsync)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            await handleMessageAsync(message, routingKey);

            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: consumer
        );

        await Task.Delay(Timeout.Infinite);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }
}