using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace WebpWhack
{
    public class MainWindowController : IMainWindowController
    {
        private readonly ILogger logger;
        private readonly MainWindowViewModel mainViewModel;
        private readonly IConfigRepo configRepo;
        private readonly IAutoRun autoRun;
        private readonly IImageConverter imageConverter;
        private Config? config;
        private WebpWatcher webpWatcher;

        public MainWindowController( ILogger logger, MainWindowViewModel mainViewModel, IConfigRepo configRepo, WebpWatcher webpWatcher, IAutoRun autoRun, IImageConverter imageConverter )
        {
            this.logger = logger;
            this.mainViewModel = mainViewModel;
            this.configRepo = configRepo;
            this.webpWatcher = webpWatcher;
            this.autoRun = autoRun;
            this.imageConverter = imageConverter;
            webpWatcher.OnWebpAdded += ConvertFile;
        }

        private void ConvertFile( string filePath )
        {
            imageConverter.Convert( filePath );
        }

        public void Start()
        {
            config = configRepo.LoadConfig();
            mainViewModel.FolderPath = config.FolderPath;
            mainViewModel.IsRunning = config.IsRunning;
            ToggleWatcher();
        }

        public void OnBrowseButton()
        {
            OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Whack folder";

            bool? result = dialog.ShowDialog();

            if( result != true ) return;

            config!.FolderPath = dialog.FolderName;
            configRepo.SaveConfig( config );
            mainViewModel.FolderPath = config.FolderPath;
            ToggleWatcher();
        }

        public void OnRunButton()
        {
            config!.IsRunning = !config.IsRunning;
            configRepo.SaveConfig( config );
            mainViewModel.IsRunning = config.IsRunning;
            ToggleWatcher();
        }

        private void ToggleWatcher()
        {
            if( !Directory.Exists( config!.FolderPath ) )
            {
                string msg = $"Directory '{config.FolderPath}' doesn't exist";
                MessageBox.Show( msg );
                logger.Log( msg );
                webpWatcher.Stop();
                return;
            }

            if( !config.IsRunning )
            {
                webpWatcher.Stop();
                return;
            }

            webpWatcher.Start( config.FolderPath );
        }

        public void OnAutoRun( bool start )
        {
            if( start )
            {
                autoRun.Set();
            }
            else
            {
                autoRun.Unset();
            }
        }

        public void OnMainWindowClosed()
        {
        }
    }
}
