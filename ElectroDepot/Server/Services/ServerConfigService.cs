using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

namespace Server.Services
{
    public class ServerConfiguration
    {
        public string StoragePath { get; set; }
        public bool Seed { get; set; }
        public bool IsEmpty()
        {
            if(StoragePath == string.Empty && Seed == false)
            {
                return true;
            }
            return false;
        }
    }

    public class ServerConfigService
    {
        private string _defaultConfigPath;
        private ServerConfiguration _serverConfig;
        private static ServerConfigService _selfInstance;
        
        public string ConfigFilePath
        {
            get
            {
                return Path.Combine(_defaultConfigPath, "server_config.json");
            }
        }

        public bool ShouldSeed
        {
            get
            {
                return _serverConfig.Seed;
            }
            set
            {
                _serverConfig.Seed = value;
            }
        }

        public string StoragePath
        {
            get
            {
                return _serverConfig.StoragePath;
            }
        }

        private ServerConfigService()
        {
            _serverConfig = new ServerConfiguration()
            {
                Seed = false,
                StoragePath = string.Empty,
            };
            //_defaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + "\\ElectroDepotServer\\";
            
            _defaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Services");
        }

        public void SaveImageServiceConfig()
        {
            string jsonCreate = JsonSerializer.Serialize(_serverConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFilePath, jsonCreate);
        }

        public static ServerConfigService Create()
        {
            if(_selfInstance == null)
            {
                _selfInstance = new ServerConfigService();
            }
            return _selfInstance;
        }

        private void CreateDefultConfigFile()
        {
            if (!File.Exists(ConfigFilePath))
            {
                using (FileStream fs = File.Create(ConfigFilePath))
                {
                    fs.Close(); // Ensure file is properly closed before setting permissions
                }

                Console.WriteLine($"Creating config folder at: '{ConfigFilePath}'");
            }

            ServerConfiguration config = new ServerConfiguration()
            {
                StoragePath = $"{_defaultConfigPath}",
                Seed = true,
            };

            string jsonCreate = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(ConfigFilePath, jsonCreate);
        }

        public void LoadConfigFile()
        {
            if (!File.Exists(ConfigFilePath))
            {
                Directory.CreateDirectory(_defaultConfigPath);

                CreateDefultConfigFile();
            }
            else
            {
                Console.WriteLine($"'{ConfigFilePath}' file exsits");
            }

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

            try
            {
                var json = File.ReadAllText(ConfigFilePath);
                _serverConfig = JsonSerializer.Deserialize<ServerConfiguration>(json, options);
            }
            catch (Exception)
            {
                CreateDefultConfigFile();

                var json = File.ReadAllText(ConfigFilePath);
                _serverConfig = JsonSerializer.Deserialize<ServerConfiguration>(json, options);
            }

            if (_serverConfig.IsEmpty())
            {
                throw new Exception($"Configuration file at '{ConfigFilePath}' needs to be completed!");
            }

        }
    }
}
