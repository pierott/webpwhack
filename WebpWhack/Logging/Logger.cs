namespace WebpWhack.Logging
{
    public class Logger : ILogger
    {
        private readonly ILogWriter logFileWriter;
        private readonly ILogWriter uiLogWriter;

        public Logger( ILogWriter logFileWriter, ILogWriter uiLogWriter )
        {
            this.logFileWriter = logFileWriter;
            this.uiLogWriter = uiLogWriter;
        }

        public void Log( string msg, LogMessageType logMessageType = LogMessageType.Info )
        {
            var logMsg = new LogMsg
            {
                Time = DateTime.Now,
                AppName = Constants.AppName,
                Msg = msg,
                ProcessId = Environment.ProcessId,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                MsgType = logMessageType
            };

            logFileWriter.WriteMessage( logMsg );

            if( logMessageType != LogMessageType.UiLog ) return;
            
            uiLogWriter.WriteMessage( logMsg );
        }
    }
}
