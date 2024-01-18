
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace WebpWhack
{
    public class Program
    {
        const string APP_MUTEX_NAME = @"Global\WebpWhackApp3CBB97740C294EE1B5E2B0D851ADF638";
        const string SHOW_WINDOW_EVENT_NAME = @"Global\WebpWhackShowWindowB90CC603777B4B79B6788E63FFDB2ABE";

        private EventWaitHandle showWindowEvent;
        private EventWaitHandle shutdownEvent;
        private Logger logger;
        private MainWindowViewModel mainViewModel;
        private IConfigRepo configRepo;
        private IAutoRun autoRun;
        private ImageConverter imageConverter;
        private WebpWatcher webpWatcher;
        private MainWindowController controller;
        private Dispatcher? uiDispatcher;
        private Application? app;
        private MainWindow? mainWindow;

        public Program()
        {
            showWindowEvent = new EventWaitHandle( false, EventResetMode.AutoReset, SHOW_WINDOW_EVENT_NAME );
            shutdownEvent = new EventWaitHandle( false, EventResetMode.AutoReset );

            logger = new Logger();
            mainViewModel = new MainWindowViewModel();
            configRepo = new RegistryConfigRepo();
            autoRun = new AutoRun();
            imageConverter = new ImageConverter();
            webpWatcher = new WebpWatcher();

            controller = new MainWindowController( logger, mainViewModel, configRepo, webpWatcher, autoRun, imageConverter );
        }

        public void Run( bool isStartup )
        {
            var appMutex = new Mutex( false, APP_MUTEX_NAME );

            if( !appMutex.WaitOne( 0 ) )
            {
                if( isStartup ) return;
                showWindowEvent.Set();
                return;
            }

            uiDispatcher = Dispatcher.CurrentDispatcher;
            Task.Factory.StartNew( CheckShowWindow, TaskCreationOptions.LongRunning );

            if( !isStartup ) ShowWindow();

            app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            app.Run();

            appMutex.ReleaseMutex();
        }

        private void CheckShowWindow()
        {
            while( true )
            {
                int index = WaitHandle.WaitAny( [showWindowEvent, shutdownEvent] );
                if( index != 0 ) return;
                uiDispatcher!.Invoke( ShowWindow );
            }
        }

        private void ShowWindow()
        {
            if( mainWindow != null ) return;
            mainWindow = new MainWindow( controller );
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.Show();
        }

        private void MainWindow_Closed( object? sender, EventArgs e )
        {
            mainWindow!.Closed -= MainWindow_Closed;
            mainWindow = null;
            controller.OnMainWindowClosed();
        }
    }
}