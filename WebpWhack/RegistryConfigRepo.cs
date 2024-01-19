using Microsoft.Win32;
using System.Windows.Input;

namespace WebpWhack
{
    public class RegistryConfigRepo : IConfigRepo
    {
        private Config defaultConfig;

        public RegistryConfigRepo( Config defaultConfig )
        {
            this.defaultConfig = defaultConfig;
        }

        public Config LoadConfig()
        {
            var key = Registry.CurrentUser.OpenSubKey( @$"Software\{Constants.AppName}" );

            if( key == null )
            {
                return defaultConfig;
            }

            var config = new Config();

            config.FolderPath = GetValue( key, nameof( config.FolderPath ) );
            config.IsRunning = GetBool( key, nameof( config.IsRunning ) );
            config.IsAutoStart = GetBool( key, nameof( config.IsAutoStart ) )!;

            return config;
        }

        public void SaveConfig( Config config )
        {
            var key = Registry.CurrentUser.CreateSubKey( @$"Software\{Constants.AppName}" );
            key.SetValue( nameof( config.FolderPath ), config.FolderPath! );
            key.SetValue( nameof( config.IsRunning ), config.IsRunning );
            key.SetValue( nameof( config.IsAutoStart ), config.IsAutoStart );
        }

        private bool GetBool( RegistryKey key, string name )
        {
            bool.TryParse( GetValue( key, name ), out bool result );
            return result;
        }

        private string? GetValue( RegistryKey key, string name )
        {
            return (string?)key.GetValue( name );
        }
    }
}