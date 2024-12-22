using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;

namespace DesktopClient.Containers
{
    public class ImageContainer
    {
        private readonly PredefinedImage _image;
        public string Name { get { return _image.Name; } }
        public Bitmap Image { get { return _image.Image; } }
        public ImageContainer(PredefinedImage image)
        {
            _image = image;
        }
    }
}
