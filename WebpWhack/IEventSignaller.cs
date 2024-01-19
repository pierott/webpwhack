namespace WebpWhack
{
    public interface IEventSignaller
    {
        event Action? OnShowWindow;
        void Start();
        void Stop();
        void SignalShowWindow();
    }
}