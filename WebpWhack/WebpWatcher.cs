using System.IO;

namespace WebpWhack
{
    public class WebpWatcher
    {
        private readonly FileSystemWatcher fileWatcher;

        public event Action<string>? OnWebpAdded;

        public WebpWatcher()
        {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Changed += OnFileAdded;
        }

        public void Start( string dirPath )
        {
            fileWatcher.Path = dirPath;
            fileWatcher.Filter = "*.webp";
            fileWatcher.IncludeSubdirectories = false;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileAdded( object sender, FileSystemEventArgs e )
        {
            OnWebpAdded?.Invoke( e.FullPath );
        }

        public void Stop()
        {
            fileWatcher.EnableRaisingEvents = false;
        }
    }
}
