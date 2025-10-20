namespace ServerMonitoringSystem.Consumer.Signaling.Interfaces;

public interface ISignalConsumer
{
    public Task Start();
    public Task Stop();
    public void Register(string signal, Action<string> action);
}