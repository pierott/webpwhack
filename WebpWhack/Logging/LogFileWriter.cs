using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace WebpWhack.Logging
{
    public class LogFileWriter : ILogWriter
    {
        private readonly string logPath;
        private readonly int maxSize;
        private readonly ActionBlock<string?> writeActionBlock;
        private Queue<string> msgQueue;
        private readonly TimeSpan delayBeforeRetry;

        private readonly byte rByte;
        private readonly byte nByte;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="maxSize">The algorithm going to be the following: Once the file exceeds maxSize, we'll find a mid point and search for a new line toward beginning of the file to cut it.
        /// For example if it's 100Mb, we'll cut it down to approximately 50Mb. We don't want to cut on every single message after it reaches the max value, it'll be resource intensive, especially with large size.</param>
        public LogFileWriter( string logPath, int maxSize )
        {
            this.logPath = logPath;
            this.maxSize = maxSize;
            writeActionBlock = new ActionBlock<string?>( OnWriteAction );
            msgQueue = new Queue<string>();
            delayBeforeRetry = TimeSpan.FromMilliseconds( 50 );
            rByte = Encoding.ASCII.GetBytes( "/r" )[0];
            nByte = Encoding.ASCII.GetBytes( "/n" )[0];
        }

        public void WriteMessage( LogMsg msg )
        {
            writeActionBlock.Post( FormatMessage( msg ) );
        }

        private string FormatMessage( LogMsg msg )
        {
            return $"{msg.Time:yyyy/MM/dd HH:mm:ss} [{msg.ProcessId}:{msg.ThreadId}] {msg.MsgType}: {msg.Msg}";
        }

        private async Task OnWriteAction( string? msg )
        {
            if( msg != null )
            {
                msgQueue.Enqueue( msg );
            }

            try
            {
                CheckMaxSize();

                using var sw = new StreamWriter( logPath, true );

                while( msgQueue.Count > 0 )
                {
                    var msgToWrite = msgQueue.Peek(); // If write fails, the message will still be in the queue
                    sw.WriteLine( msgToWrite );
                    msgQueue.Dequeue();
                }
            }
            catch( Exception ex )
            {
                Trace.WriteLine( $"Cannot write to a file '{logPath}': {ex}" );
                await Task.Delay( delayBeforeRetry );
                writeActionBlock.Post( null ); // We need to try again immediately, not waiting for the new message
            }
        }

        private void CheckMaxSize()
        {
            if( !File.Exists( logPath ) ) return;

            var fileInfo = new FileInfo( logPath );
            if( fileInfo.Length < maxSize ) return;

            int bufferSize = 80;
            string truncatedPath = $"{logPath}.truncated";

            using( Stream f = File.Open( logPath, FileMode.Open, FileAccess.ReadWrite ))
            {
                f.Position = fileInfo.Length / 2 - bufferSize;

                long afterNewLinePosition;

                while( (afterNewLinePosition = FindLastNewLine( f, bufferSize ) ) == -1 )
                {
                    if( f.Position - bufferSize < 0 ) return; // We weren't able to find a new line which is kind of exceptional situation but we'll just return and try again later.
                    f.Position -= bufferSize;
                }

                f.Position = afterNewLinePosition;

                using( Stream fTruncated = File.Open( truncatedPath, FileMode.Open, FileAccess.Write ))
                {
                    f.CopyTo( fTruncated );
                }
            }

            File.Delete( logPath ); // TODO: Since this also can fail with exception we should make more checks on the state of the CheckMaxSize process. Currently we assume it's a single state.
            File.Move( truncatedPath, logPath );
        }

        private long FindLastNewLine( Stream f, int bufferSize )
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead = f.Read( buffer );

            for( int i = 0; i < bytesRead - 1; i++ )
            {
                if( buffer[i] == rByte && buffer[i+1] == nByte ) return f.Position + i + 2;
            }

            return -1;
        }
    }
}
