namespace ServerMonitoringSystem.Consumer.Signaling.Interfaces;

public interface ISignalConsumer
{
    public Task Start();
    public Task Stop();
    public Task Register(string signal, Action<string> action);
}