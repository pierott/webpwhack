namespace WebpWhack
{
    public class EventSignaller : IEventSignaller
    {
        const string SHOW_WINDOW_EVENT_NAME = @"Global\WebpWhackShowWindowB90CC603777B4B79B6788E63FFDB2ABE";

        private EventWaitHandle showWindowEvent;
        private EventWaitHandle shutdownEvent;

        public event Action? OnShowWindow;

        public EventSignaller()
        {
            showWindowEvent = new EventWaitHandle( false, EventResetMode.AutoReset, SHOW_WINDOW_EVENT_NAME );
            shutdownEvent = new EventWaitHandle( false, EventResetMode.AutoReset );
        }

        public void Start()
        {
            Task.Factory.StartNew( CheckShowWindow, TaskCreationOptions.LongRunning );
        }

        public void Stop()
        {
            shutdownEvent.Set();
        }

        public void SignalShowWindow()
        {
            showWindowEvent.Set();
        }

        private void CheckShowWindow()
        {
            while( true )
            {
                int index = WaitHandle.WaitAny( [showWindowEvent, shutdownEvent] );
                if( index != 0 ) return;
                OnShowWindow?.Invoke();
            }
        }
    }
}