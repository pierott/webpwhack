using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;

namespace WebpWhack
{
    public class ImageConverter : IImageConverter
    {
        public void Convert( string path )
        {
            //string dirPath = Path.Get

            string fIn = @"c:\r\in.webp";
            string fOut = @"c:\r\out.jpeg";

            using( Image image = Image.Load( fIn ) )
            {
                // Save to JPEG format
                image.Save( fOut, new JpegEncoder() );
            }
        }
    }

    public interface IImageConverter
    {
        void Convert( string path );
    }
}
