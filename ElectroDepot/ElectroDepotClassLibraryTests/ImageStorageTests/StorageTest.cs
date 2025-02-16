using ElectroDepotClassLibrary.Services;

namespace ElectroDepotClassLibraryTests.ImageStorageTests
{
    public class StorageTest
    {
        private bool DeleteAfterDone = false;
        private string AssetsPath 
        { 
            get 
            { 
                string assetsPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("ElectroDepotClassLibraryTests") + "ElectroDepotClassLibraryTests".Length) + "\\Assets\\";
                if (Path.Exists(assetsPath)) return assetsPath;
                else throw new Exception("There is a problem with assets path!");
            } 
        }
        private ImageStorageService imageStorageService;
        public StorageTest()
        {
            imageStorageService = ImageStorageService.CreateService();
            imageStorageService.Initialize();
        }

        ~StorageTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "ImageStorage";
            if (DeleteAfterDone == true)
            {
                // Delete output
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }
        
        [Fact]
        public void InsertNew()
        {
            string assetsPath = AssetsPath;
            byte[] image1 = File.ReadAllBytes(assetsPath + "image1.jpg");
            byte[] image2 = File.ReadAllBytes(assetsPath + "image2.png");

            string image1Identifier = imageStorageService.InsertProjectImage(image1);
            string image2Identifier = imageStorageService.InsertUserImage(image2);

            Assert.NotNull(image1Identifier);
            Assert.NotEmpty(image1Identifier);

            Assert.NotNull(image2Identifier);
            Assert.NotEmpty(image2Identifier);

            //Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "ImageStorage");
        }

        [Fact]
        public void Update()
        {
            string assetsPath = AssetsPath;
            byte[] image = File.ReadAllBytes(assetsPath + "image1.jpg");
            byte[] replacement = File.ReadAllBytes(assetsPath + "image2.png");

            string imageIdentifier = imageStorageService.InsertProjectImage(image);

            Assert.NotNull(imageIdentifier);
            Assert.NotEmpty(imageIdentifier);

            imageStorageService.UpdateProjectImage(imageIdentifier, replacement);

            byte[] changedImage = imageStorageService.RetrieveProjectImage(imageIdentifier);

            Assert.True(changedImage != image);
        }

        [Fact]
        public void Read()
        {
            string assetsPath = AssetsPath;
            byte[] image1 = File.ReadAllBytes(assetsPath + "image1.jpg");
            byte[] image2 = File.ReadAllBytes(assetsPath + "image2.png");

            string image1Identifier = imageStorageService.InsertProjectImage(image1);
            string image2Identifier = imageStorageService.InsertUserImage(image2);

            Assert.NotNull(image1Identifier);
            Assert.NotEmpty(image1Identifier);

            Assert.NotNull(image2Identifier);
            Assert.NotEmpty(image2Identifier);

            byte[] image1FromServer = imageStorageService.RetrieveProjectImage(image1Identifier);
            byte[] image2FromServer = imageStorageService.RetrieveUserImage(image2Identifier);
        
            Assert.True(image1FromServer.Length != 0);
            Assert.True(image1FromServer.Length != 0);
        }

        [Fact]
        public void Delete()
        {
            string assetsPath = AssetsPath;
            byte[] image = File.ReadAllBytes(assetsPath + "image1.jpg");

            string imageIdentifier = imageStorageService.InsertProjectImage(image);

            Assert.NotNull(imageIdentifier);
            Assert.NotEmpty(imageIdentifier);

            imageStorageService.RemoveProjectImage(imageIdentifier);

            byte[] changedImage = imageStorageService.RetrieveProjectImage(imageIdentifier);

            Assert.True(changedImage.Length == 0);
        }
    }
}
