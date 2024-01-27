using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpWhack
{
    public class MainWindowViewModel : PropertyChangedNotifier
    {
        private string? folderPath;
        private string? runBtnText;
        private bool isAutoStart;
        
        public MainWindowViewModel()
        {
            LogMessages = new ObservableCollection<string>();
        }

        public string? FolderPath
        {
            get => folderPath;
            set
            {
                if( value == folderPath ) return;
                folderPath = value;
                RaisePropertyChanged( nameof( FolderPath ) );
            }
        }

        public string? RunBtnText
        {
            get => runBtnText;
            set
            {
                if( value == runBtnText ) return;
                runBtnText = value;
                RaisePropertyChanged( nameof( RunBtnText ) );
            }
        }

        public bool IsAutoStart
        {
            get => isAutoStart;
            set
            {
                if( value == isAutoStart ) return;
                isAutoStart = value;
                RaisePropertyChanged( nameof( IsAutoStart ) );
            }
        }

        public ObservableCollection<string> LogMessages { get; set; }
    }
}
