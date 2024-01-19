using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using System.Windows;
using System.Windows.Controls;
using Image = SixLabors.ImageSharp.Image;
//using System.Windows.Controls;

namespace WebpWhack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWindowController mainCommands;

        public MainWindow( IMainWindowController mainCommands ) // TODO: Harsh violation of MVVM! Call the police or get your lazy ass to do it properly.
        {
            this.mainCommands = mainCommands;

            InitializeComponent();
        }

        private void RunButton_Click( object sender, RoutedEventArgs e )
        {
            mainCommands.OnRunButton();
        }

        private void BrowseButton_Click( object sender, RoutedEventArgs e )
        {
            mainCommands.OnBrowseButton();
        }

        private void CheckBox_Checked( object sender, RoutedEventArgs e )
        {
            mainCommands.OnAutoRun( true );
        }

        private void CheckBox_Unchecked( object sender, RoutedEventArgs e )
        {
            mainCommands.OnAutoRun( false );
        }
    }
}