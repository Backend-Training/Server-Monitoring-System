namespace RabbitMQ.Interfaces;

public interface IMessageProducer
{
    void Publish(string body);
}