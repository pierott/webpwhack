using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpWhack
{
    public class Config
    {
        public bool IsRunning { get; set; }
        public bool IsAutoStart { get; set; }
        public string? FolderPath { get; internal set; }
    }
}
