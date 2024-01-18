using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpWhack
{
    public class MainWindowViewModel
    {
        public string? FolderPath { get; internal set; }
        public bool IsRunning { get; internal set; }
    }
}
