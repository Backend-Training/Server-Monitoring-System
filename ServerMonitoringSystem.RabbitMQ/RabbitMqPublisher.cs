using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Configuration;

public class RabbitMqPublisher : IAsyncDisposable
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly string _exchangeName;
    private readonly string _routingKey;
    private IConnection? _connection;
    private IChannel? _channel;

    private RabbitMqPublisher(RabbitMqSettings settings, string exchangeName, string routingKey)
    {
        _rabbitMqSettings = settings;
        _exchangeName = exchangeName;
        _routingKey = routingKey;
    }
    
    public static async Task<RabbitMqPublisher> CreateAsync(
        RabbitMqSettings settings,
        string exchangeName,
        string routingKey)
    {
        var publisher = new RabbitMqPublisher(settings, exchangeName, routingKey);
        await publisher.InitAsync();
        return publisher;
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
    }

    public async Task PublishAsync(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: _routingKey,
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }
}