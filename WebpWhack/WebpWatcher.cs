using System.IO;
using WebpWhack.Logging;

namespace WebpWhack
{
    public class WebpWatcher : IWebpWatcher
    {
        private readonly ILogger logger;
        private readonly FileSystemWatcher fileWatcher;
        public event Action<string>? OnWebpAdded;

        public WebpWatcher( ILogger logger )
        {
            this.logger = logger;
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Changed += OnFileAdded;
            fileWatcher.Created += OnFileAdded;
        }

        public bool IsRunning { get; private set; }

        public void Start( string dirPath )
        {
            if( IsRunning )
            {
                logger.Log( "Restarting watcher" );
                Stop();
            }

            logger.Log( $"Starting watcher at '{dirPath}'" );

            IsRunning = true;
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
            if( !IsRunning ) return;

            logger.Log( "Stopping watcher" );

            IsRunning = false;
            fileWatcher.EnableRaisingEvents = false;
        }
    }
}
