using System.Drawing;
using System.IO;

namespace Phony.Extensions
{
    public static class ImageConverterExtensions
    {
        public static byte[] ImageToByteArray(this Image imageIn)
        {
            MemoryStream ms = new();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(this byte[] byteArrayIn)
        {
            MemoryStream ms = new(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}