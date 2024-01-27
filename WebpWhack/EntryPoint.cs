using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Velopack;

namespace WebpWhack
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main( string[] args )
        {
            try
            {
                VelopackApp.Build().Run();

                bool isStartup = args.Length > 0 && args[0].ToLower() == Constants.StartupAttr.ToLower();

                new CompositionRoot().Run( isStartup );
            }
            catch( Exception ex )
            {
                Trace.WriteLine( $"Abnormal program termination: {ex}" );
            }
        }

        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager( "https://the.place/you-host/updates" );

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if( newVersion == null )
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync( newVersion );

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart();
        }
    }
}
