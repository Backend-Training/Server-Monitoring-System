using RabbitMQ.Client;

namespace RabbitMQ.Configuration;

public class RabbitMqSettings
{
    public string HostName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }

    public RabbitMqSettings()
    {
        HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        Username = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
        Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
        Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672");
    }

    public ConnectionFactory Connection()
    {
        return new ConnectionFactory()
        {
            HostName = this.HostName,
            UserName = this.Username,
            Password = this.Password,
            Port = this.Port
        };
    }
}