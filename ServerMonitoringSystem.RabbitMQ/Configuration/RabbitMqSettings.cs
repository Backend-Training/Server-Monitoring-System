namespace RabbitMQ.Configuration;

public class RabbitMqSettings
{
    public string HostName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }

    public RabbitMqSettings(string hostName = "localhost", string username = "guest",
        string password = "guest", int port = 5672)
    {
        HostName = hostName;
        Username = username;
        Password = password;
        Port = port;
    }
}