namespace WebpWhack
{
    public interface IWebpWatcher
    {
        event Action<string>? OnWebpAdded;
        void Start( string dirPath );
        void Stop();
    }
}
