using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WebpWhack.Logging
{
    public class UiLogWriter : ILogWriter
    {
        private MainWindowViewModel mainViewModel;
        private Dispatcher dispatcher;

        public UiLogWriter( MainWindowViewModel mainViewModel, Dispatcher dispatcher )
        {
            this.mainViewModel = mainViewModel;
            this.dispatcher = dispatcher;
        }

        public void WriteMessage( LogMsg msg )
        {
            dispatcher?.Invoke( () => AddToViewModel( msg ) );
        }

        private void AddToViewModel( LogMsg msg )
        {
            mainViewModel.LogMessages.Insert( 0, $"[{msg.Time:HH:mm:ss}] {msg.Msg}{Environment.NewLine}" );
            if( mainViewModel.LogMessages.Count > 5 ) mainViewModel.LogMessages.RemoveAt( mainViewModel.LogMessages.Count-1 );
        }
    }
}
