namespace WebpWhack.Logging
{
    public interface ILogFormatter
    {
        string FormatMessage( LogMsg logMsg );
    }
}