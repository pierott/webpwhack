namespace WebpWhack.Logging
{
    public interface ILogger
    {
        public void Log( string msg, LogMessageType logMessageType = LogMessageType.Info );
    }
}
