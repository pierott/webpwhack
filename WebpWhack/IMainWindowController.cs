using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpWhack
{
    public interface IMainWindowController
    {
        void OnAutoRun( bool start );
        void OnBrowseButton();
        void OnRunButton();
    }
}
