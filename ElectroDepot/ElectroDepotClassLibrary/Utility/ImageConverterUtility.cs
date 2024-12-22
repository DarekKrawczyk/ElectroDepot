using Avalonia.Media.Imaging;

namespace ElectroDepotClassLibrary.Utility
{
    public static class ImageConverterUtility
    {
        public static byte[] BitmapToBytes(Bitmap image)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                image.Save(memoryStream);
                return memoryStream.ToArray();
            }
            catch(Exception ex)
            {
                return new byte[] { };
            }
        }

        public static Bitmap BytesToBitmap(byte[] image)
        {
            try
            {
                using (Stream stream = new MemoryStream(image))
                {
                    return new Bitmap(stream);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
