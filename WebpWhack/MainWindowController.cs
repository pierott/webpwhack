using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace WebpWhack
{
    public class MainWindowController : IMainWindowController
    {
        const string APP_MUTEX_NAME = @"Global\WebpWhackApp3CBB97740C294EE1B5E2B0D851ADF638";

        private readonly ILogger logger;
        private readonly MainWindowViewModel mainViewModel;
        private readonly IConfigRepo configRepo;
        private readonly IAutoRun autoRun;
        private readonly IImageConverter imageConverter;
        private readonly IEventSignaller eventSignaller;
        private readonly IWebpWatcher webpWatcher;

        private Config? config;
        private Dispatcher? uiDispatcher;
        private Application? app;
        private MainWindow? mainWindow;

        public MainWindowController( ILogger logger, MainWindowViewModel mainViewModel, IConfigRepo configRepo, IWebpWatcher webpWatcher, IAutoRun autoRun, IImageConverter imageConverter, IEventSignaller eventSignaller )
        {
            this.logger = logger;
            this.mainViewModel = mainViewModel;
            this.configRepo = configRepo;
            this.webpWatcher = webpWatcher;
            this.autoRun = autoRun;
            this.imageConverter = imageConverter;
            this.eventSignaller = eventSignaller;
            webpWatcher.OnWebpAdded += ConvertFile;
            eventSignaller.OnShowWindow += DispatchShowWindow;
        }

        public void Run( bool isStartup )
        {
            var appMutex = new Mutex( false, APP_MUTEX_NAME );

            if( !appMutex.WaitOne( 0 ) ) // Another instance of this process is running
            {
                if( isStartup ) return; // If we launched on system start, this shouldn't happen normally. If this still happens we just quit.
                eventSignaller.SignalShowWindow(); // Show window in the already running process.
                return;
            }

            uiDispatcher = Dispatcher.CurrentDispatcher;

            config = configRepo.LoadConfig();
            ApplyConfig();

            eventSignaller.Start();

            if( !isStartup ) ShowWindow();

            app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            app.Run();

            eventSignaller.Stop();
            appMutex.ReleaseMutex();
        }

        public void OnAutoRun( bool start )
        {
            config!.IsAutoStart = start;
            configRepo.SaveConfig( config );
            SetAutoRun( start );
        }

        private void SetAutoRun( bool start )
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

        public void OnBrowseButton()
        {
            OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Whack folder";

            bool? result = dialog.ShowDialog();

            if( result != true ) return;

            config!.FolderPath = dialog.FolderName;
            configRepo.SaveConfig( config );
            ApplyConfig();
        }

        public void OnRunButton()
        {
            config!.IsRunning = !config.IsRunning;
            configRepo.SaveConfig( config );
            ApplyConfig();
        }

        private void ApplyConfig()
        {
            mainViewModel.FolderPath = config!.FolderPath;
            mainViewModel.RunBtnText = config.IsRunning ? "Stop Running" : "Start Running";
            mainViewModel.IsAutoStart = config.IsAutoStart;
            SetAutoRun( config.IsAutoStart );
            ToggleWatcher();
        }

        private void DispatchShowWindow() => uiDispatcher!.Invoke( ShowWindow );

        private void ShowWindow()
        {
            if( mainWindow != null ) return;
            mainWindow = new MainWindow( this );
            mainWindow.DataContext = mainViewModel;
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.Show();
        }

        private void MainWindow_Closed( object? sender, EventArgs e )
        {
            mainWindow!.Closed -= MainWindow_Closed;
            mainWindow = null;
            
            if( !config!.IsRunning )
            {
                app!.Shutdown();
            }
        }

        private void ConvertFile( string filePath )
        {
            imageConverter.Convert( filePath );
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
    }
}
