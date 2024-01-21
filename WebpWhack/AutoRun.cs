using Microsoft.Win32;

namespace WebpWhack
{
    public class AutoRun : IAutoRun
    {
        private bool? set;

        public void Set()
        {
            if( set == true ) return;
            set = true;

            var key = Registry.CurrentUser.CreateSubKey( @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" );
            string currentExePath = Environment.ProcessPath!;
            key.SetValue( Constants.AppName, $"\"{currentExePath}\" {Constants.StartupAttr}" );
            key.Close();
        }

        public void Unset()
        {
            if( set == false ) return;
            set = false;

            var key = Registry.CurrentUser.OpenSubKey( @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true );
            if( key == null ) return;
            if( key.GetValue( Constants.AppName ) == null ) return;
            key.DeleteValue( Constants.AppName );
        }
    }
}
