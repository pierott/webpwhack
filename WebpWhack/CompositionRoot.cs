using System.Windows.Threading;
using WebpWhack.Logging;

namespace WebpWhack
{
    public class CompositionRoot
    {
        private Logger logger;
        private MainWindowViewModel mainViewModel;
        private Dispatcher dispatcher;
        private IConfigRepo configRepo;
        private IAutoRun autoRun;
        private ImageConverter imageConverter;
        private WebpWatcher webpWatcher;
        private MainWindowController controller;
        private IEventSignaller eventSignaller;
        private DesktopNotifier desktopNotifier;
        private LogFileWriter logFileWriter;
        private UiLogWriter uiLogWriter;

        public CompositionRoot()
        {
            var defaultConfig = new Config
            {
                FolderPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ),
                IsAutoStart = true,
                IsRunning = false
            };

            logFileWriter = new LogFileWriter( "webpwhack.log", 512 * 1024 );

            mainViewModel = new MainWindowViewModel();

            dispatcher = Dispatcher.CurrentDispatcher;

            uiLogWriter = new UiLogWriter( mainViewModel, dispatcher );

            logger = new Logger( logFileWriter, uiLogWriter );

            configRepo = new RegistryConfigRepo( defaultConfig );
            autoRun = new AutoRun();
            imageConverter = new ImageConverter( logger );
            webpWatcher = new WebpWatcher( logger );
            eventSignaller = new EventSignaller();
            desktopNotifier = new DesktopNotifier();

            controller = new MainWindowController( logger, mainViewModel, configRepo, webpWatcher, autoRun, imageConverter, eventSignaller, desktopNotifier, dispatcher );
        }

        public void Run( bool isStartup )
        {
            controller.Run( isStartup );
        }
    }
}