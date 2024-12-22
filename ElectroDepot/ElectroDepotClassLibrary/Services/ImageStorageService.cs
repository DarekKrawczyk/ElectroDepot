using System.Drawing;
using System.Drawing.Imaging;

namespace ElectroDepotClassLibrary.Services
{
    public class ImageStorageService
    {
        private string _path;
        private string RootFolder = "ImageStorage";
        private string UsersFolder = "Users";
        private string ProjectsFolder = "Projects";
        private string ComponentsFolder = "Components";
        private string FullProjectFolder { get { return _path + RootFolder + "\\" + ProjectsFolder + "\\"; } }
        private string FullComponentsFolder { get { return _path + RootFolder + "\\" + ComponentsFolder + "\\"; } }
        private string FullUsersFolder { get { return _path + RootFolder + "\\" + UsersFolder + "\\"; } }
        public ImageStorageService(string path)
        {
            _path = path;
        }

        public string InsertProjectImage(byte[] image)
        {
            return InsertImage(FullProjectFolder, image);
        }

        public string InsertComponentImage(byte[] image)
        {
            return InsertImage(FullComponentsFolder, image);
        }

        public string InsertUserImage(byte[] image)
        {
            return InsertImage(FullUsersFolder, image);
        }

        private string InsertImage(string folder, byte[] imageByteArray)
        {
            using (MemoryStream ms = new MemoryStream(imageByteArray))
            {
                string imageName = GenerateNameForImage(FullProjectFolder);
                Image image = Image.FromStream(ms);
                image.Save(folder + imageName + "." + ImageFormat.Png.ToString().ToLower(), ImageFormat.Png);
                return imageName;
            }
        }

        public void UpdateComponentImage(string imageName, byte[] image)
        {
            UpdateImage(FullComponentsFolder, imageName, image);
        }

        public void UpdateProjectImage(string imageName, byte[] image)
        {
            UpdateImage(FullProjectFolder, imageName, image);
        }

        public void UpdateUserImage(string imageName, byte[] image)
        {
            UpdateImage(FullUsersFolder, imageName, image);
        }

        private void UpdateImage(string folder, string imageName, byte[] imageByteArray)
        {
            using (MemoryStream ms = new MemoryStream(imageByteArray))
            {
                string fullPath = folder + imageName + ".png";

                // Remove current
                RemoveImage(folder, imageName);

                // Replace it with new
                Image image = Image.FromStream(ms);
                image.Save(fullPath, ImageFormat.Png);
            }
        }

        public byte[] RetrieveComponentImage(string imageName)
        {
            return RetrieveImage(FullComponentsFolder, imageName);
        }

        public byte[] RetrieveProjectImage(string imageName)
        {
            return RetrieveImage(FullProjectFolder, imageName);
        }

        public byte[] RetrieveUserImage(string imageName)
        {
            return RetrieveImage(FullUsersFolder, imageName);
        }

        private byte[] RetrieveImage(string folder, string imageName)
        {
            string fullPath = folder + imageName + ".png";
            if (File.Exists(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }
            else
            {
                return new byte[] { };
            }
        }

        public void RemoveProjectImage(string imageName)
        {
            RemoveImage(FullProjectFolder, imageName);
        }

        public void RemoveUserImage(string imageName)
        {
            RemoveImage(FullUsersFolder, imageName);
        }

        public void RemoveImage(string folder, string imageName)
        {
            string fullPath = folder + imageName + ".png";
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private string GenerateNameForImage(string folder)
        {
            Guid guid = Guid.NewGuid();
            string name = CreateName(guid);
            while (File.Exists(folder + name))
            {
                name = CreateName(Guid.NewGuid());
            }
            return name;
        }

        private string CreateName(Guid guid)
        {
            return $"{guid.ToString()}";
        }

        public void Initialize()
        {
            if (Directory.Exists(_path))
            {
                if (!Directory.Exists(_path + RootFolder))
                {
                    // Create it
                    Directory.CreateDirectory(_path + RootFolder);
                    if (!Directory.Exists(_path + RootFolder + "\\" + UsersFolder))
                    {
                        Directory.CreateDirectory(_path + RootFolder + "\\" + UsersFolder);
                    }

                    if (!Directory.Exists(_path + RootFolder + "\\" + ProjectsFolder))
                    {
                        Directory.CreateDirectory(_path + RootFolder + "\\" + ProjectsFolder);
                    }
                }
            }
        }
    }
}
