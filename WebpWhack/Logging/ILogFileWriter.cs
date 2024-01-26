namespace WebpWhack.Logging
{
    public interface ILogWriter
    {
        void WriteMessage( LogMsg msg );
    }
}
