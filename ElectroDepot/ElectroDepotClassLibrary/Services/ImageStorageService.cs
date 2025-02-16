using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using System.IO.Compression;
using System.Text;

namespace ElectroDepotClassLibrary.Services
{
    public class ImageStorageService
    {
        private string _path;
        private string RootFolder = "ImageStorage";
        private string UsersFolder = "Users";
        private string ProjectsFolder = "Projects";
        private string ComponentsFolder = "Components";
        private string FullRootFolder
        {
            get
            {
                return Path.Combine(new string[] { _path, RootFolder });
            }
        }
        private string FullProjectFolder 
        { 
            get 
            { 
                //return _path + "\\" + RootFolder + "\\" + ProjectsFolder + "\\";
                return Path.Combine(new string[] { _path, RootFolder, ProjectsFolder });
            } 
        }
        private string FullComponentsFolder 
        { 
            get 
            { 
                return Path.Combine(new string[] { _path, RootFolder, ComponentsFolder });
            } 
        }
        private string FullUsersFolder 
        { 
            get 
            { 
                return Path.Combine(new string[] { _path, RootFolder, UsersFolder });
            } 
        }

        private static ImageStorageService _selfInstance;
        private ImageStorageService() { }

        private bool _requiresSeeding = true;

        public bool RequiresSeeding
        {
            get
            {
                return _requiresSeeding;
            }
        }

        public static ImageStorageService CreateService()
        {
            if(_selfInstance == null)
            {
                _selfInstance = new ImageStorageService();
            }
            return _selfInstance;
        }

        public void Initialize(string storagePath)
        {
            _path = storagePath;
            InitializeFolders();
        }

        public void DeleteAllImages()
        {
            try
            {
                if (Directory.Exists(FullRootFolder))
                {
                    Directory.Delete(FullRootFolder, true);
                    Console.WriteLine("Directory deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
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
                Image image = Image.Load(ms);
                //var image = ; //, "." + ImageFormat.Png.ToString().ToLower()
                image.Save(Path.Combine(folder, imageName), new PngEncoder());
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
                //string fullPath = folder + imageName + ".png";
                string fullPath = Path.Combine(folder, imageName);

                // Remove current
                RemoveImage(folder, imageName);

                // Replace it with new
                Image image = Image.Load(ms);
                image.Save(fullPath, new PngEncoder());
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
            //string fullPath = folder + imageName + ".png";
            string fullPath = Path.Combine(folder, imageName);
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
            //string fullPath = folder + imageName + ".png";
            string fullPath = Path.Combine(folder, imageName);
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

        private void InitializeFolders()
        {
            bool rootFolderExists = true;
            bool componentsFolderExists = true;
            bool projectsFolderExists = true;
            bool usersFolderExists = true;

            if (Directory.Exists(_path))
            {
                rootFolderExists = Directory.Exists(FullRootFolder);
                if (rootFolderExists == false)
                {
                    Directory.CreateDirectory(FullRootFolder);
                }

                componentsFolderExists = Directory.Exists(FullComponentsFolder);
                if (componentsFolderExists == false)
                {
                    Directory.CreateDirectory(FullComponentsFolder);
                }

                projectsFolderExists = Directory.Exists(FullProjectFolder);
                if (projectsFolderExists == false)
                {
                    Directory.CreateDirectory(FullProjectFolder);
                }

                usersFolderExists = Directory.Exists(FullUsersFolder);
                if (usersFolderExists == false)
                {
                    Directory.CreateDirectory(FullUsersFolder);
                }
            }
            else
            {
                throw new Exception($"Folder '{_path}' provided in config file doesn't exsit!");
            }

            _requiresSeeding = rootFolderExists == false || componentsFolderExists == false || projectsFolderExists == false || usersFolderExists == false;

            if(_requiresSeeding == true)
            {
                // create backup to zip and dump content
                DateTime time = DateTime.Now;
                StringBuilder sb = new StringBuilder();
                sb.Append("ImagesServiceBackup-");
                sb.Append(time.Day);
                sb.Append("-");
                sb.Append(time.Month);
                sb.Append("-");
                sb.Append(time.Year);
                sb.Append("-");
                sb.Append(time.Hour);
                sb.Append("-");
                sb.Append(time.Minute);
                sb.Append("-");
                sb.Append(time.Second);
                sb.Append(".zip");

                string path = Path.Combine(_path, sb.ToString());

                if (!File.Exists(path))
                {
                    ZipFile.CreateFromDirectory(FullRootFolder, path);
                }
            }
        }
    }
}
