using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class PredefinedImagesStore : RootStore
    {
        private readonly PredefinedImageDataProvider _provider;
        private List<PredefinedImage> _images = new List<PredefinedImage>();
        public List<PredefinedImage> Images
        {
            get
            {
                return _images;
            }
        }

        public event Action ImagesLoaded;

        public PredefinedImagesStore(DatabaseStore dbStore, PredefinedImageDataProvider predefinedImageDataProvider) : base(dbStore) 
        { 
            _provider = predefinedImageDataProvider;
        }

        public async void Load()
        {
            Images.Clear();

            IEnumerable<PredefinedImage> imagesFromDB = await _provider.GetAll();
            Images.AddRange(imagesFromDB);

            ImagesLoaded?.Invoke();
        }

    }
}
