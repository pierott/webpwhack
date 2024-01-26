using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using WebpWhack.Logging;

namespace WebpWhack
{
    public class ImageConverter : IImageConverter
    {
        private const int MAX_FILE_NAME_FOR_LOGGING = 30;
        private readonly ILogger logger;

        public ImageConverter( ILogger logger )
        {
            this.logger = logger;
        }

        public void Convert( string webpPath )
        {
            if( !File.Exists( webpPath ) ) return; // File watcher raises several events for the same file

            int tries = 5;
            bool wasAbleToMove = false;
            bool converted = false;
            string webpPathInTemp;

            int i;

            for( i = 0; i < tries; i++ )
            {
                try
                {
                    ConvertToJpeg( webpPath );
                    converted = true;
                    webpPathInTemp = Path.Combine( Path.GetTempPath(), Path.GetFileName( webpPath ) );
                    
                    try
                    {
                        File.Move( webpPath, webpPathInTemp, true );
                        wasAbleToMove = true;
                    }
                    catch( Exception ex )
                    {
                        logger.Log( $"Cannot move to temp folder: {ex}" );
                        wasAbleToMove = false;
                    }

                    break;
                }
                catch( Exception ex )
                {
                    logger.Log( $"Cannot convert: {ex}" );
                }

                Thread.Sleep( 300 );
            }

            string webpName = Path.GetFileNameWithoutExtension( webpPath );
            if( webpName.Length > MAX_FILE_NAME_FOR_LOGGING ) webpName = string.Format( "{0}..", webpName.Substring( 0, MAX_FILE_NAME_FOR_LOGGING ) );
            webpName += ".webp";

            if( converted )
            {
                if( wasAbleToMove )
                {
                    logger.Log( $"Successfully converted {webpName} on {i+1} try", LogMessageType.UiLog );
                }
                else
                {
                    logger.Log( $"Converted {webpName} on {i+1} try but didn't move to temp", LogMessageType.UiLog );
                }
            }
            else
            {
                logger.Log( $"Wasn't able to convert {webpName} in {tries} tries", LogMessageType.UiLog );
            }
        }

        private void ConvertToJpeg( string webpPath )
        {
            string dirPath = Path.GetDirectoryName( webpPath )!;
            string fileName = Path.GetFileNameWithoutExtension( webpPath );
            string jpegPath = Path.Combine( dirPath, $"{fileName}.jpeg" );

            using( Image image = Image.Load( webpPath ) )
            {
                image.Save( jpegPath, new JpegEncoder() );
            }
        }
    }
}
