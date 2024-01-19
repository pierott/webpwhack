namespace WebpWhack
{
    public class CompositionRoot
    {
        private Logger logger;
        private MainWindowViewModel mainViewModel;
        private IConfigRepo configRepo;
        private IAutoRun autoRun;
        private ImageConverter imageConverter;
        private WebpWatcher webpWatcher;
        private MainWindowController controller;
        private IEventSignaller eventSignaller;

        public CompositionRoot()
        {
            var defaultConfig = new Config
            {
                FolderPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ),
                IsAutoStart = true,
                IsRunning = false
            };

            logger = new Logger();
            mainViewModel = new MainWindowViewModel();
            configRepo = new RegistryConfigRepo( defaultConfig );
            autoRun = new AutoRun();
            imageConverter = new ImageConverter( logger );
            webpWatcher = new WebpWatcher( logger );
            eventSignaller = new EventSignaller();

            controller = new MainWindowController( logger, mainViewModel, configRepo, webpWatcher, autoRun, imageConverter, eventSignaller );
        }

        public void Run( bool isStartup )
        {
            controller.Run( isStartup );
        }
    }
}