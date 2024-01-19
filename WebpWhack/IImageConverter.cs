using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;

namespace WebpWhack
{
    public class ImageConverter : IImageConverter
    {
        private readonly ILogger logger;

        public ImageConverter( ILogger logger )
        {
            this.logger = logger;
        }

        public void Convert( string webpPath )
        {
            int tries = 3;

            for( int i = 0; i < tries; i++ )
            {
                try
                {
                    ConvertToJpeg( webpPath );
                    string webpPathInTemp = Path.Combine( Path.GetTempPath(), Path.GetFileName( webpPath ) );
                    
                    try
                    {
                        File.Move( webpPath, webpPathInTemp );
                    }
                    catch {}

                    break;
                }
                catch( Exception ex )
                {
                    logger.Log( $"Cannot convert: {ex}" );
                }

                Thread.Sleep( 300 );
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

    public interface IImageConverter
    {
        void Convert( string path );
    }
}
