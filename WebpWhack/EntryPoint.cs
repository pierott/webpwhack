using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebpWhack
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main( string[] args )
        {
            try
            {
                bool isStartup = args.Length > 0 && args[0].ToLower() == "startup";

                new Program().Run( isStartup );
            }
            catch( Exception ex )
            {
                Trace.WriteLine( $"Abnormal program termination: {ex}" );
            }
        }
    }
}
