namespace ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;

public interface ISignal
{
    public Task Invoke(string signal, string message);
}