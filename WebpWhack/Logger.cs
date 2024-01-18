using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpWhack
{
    public interface ILogger
    {
        public void Log( string msg );
    }

    public class Logger : ILogger
    {
        public void Log( string msg )
        {
        }
    }
}
