using ElectroDepotClassLibrary.Models;
using System.Text.Json;

namespace ElectroDepotClassLibrary.Utility
{
    public class ApplicationConfig
    {
        private ServerConfig serverConfig;
        private UserConfig userConfig;

        public ServerConfig ServerConfig
        {
            get
            {
                return serverConfig;
            }
        }

        public UserConfig UserConfig
        {
            get
            {
                return userConfig;
            }
        }

        public string DefaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + "\\ElectroDepot\\";

        private static ApplicationConfig _selfInstance;

        private ApplicationConfig()
        {
            serverConfig = new ServerConfig(DefaultConfigPath + "server.json");
            userConfig = new UserConfig(DefaultConfigPath + "user.json");
        }

        public static ApplicationConfig Create()
        {
            if (_selfInstance == null)
            {
                _selfInstance = new ApplicationConfig();
            }
            return _selfInstance;
        }

        public void LoadConfig()
        {
            if (!Directory.Exists(DefaultConfigPath))
            {
                Directory.CreateDirectory(DefaultConfigPath);
            }
            serverConfig.LoadCredentials();
            userConfig.LoadCredentials();
        }

        public void SaveConfig()
        {

        }
    }
    public class ServerData
    {
        public string IP { get; set; }
        public string Port { get; set; }
    }

    public class ServerConfig
    {
        private string _configPath;
        private ServerData _data;

        public string ConnectionURL
        {
            get
            {
                return $"https://{_data.IP}:{_data.Port}/";
            }
        }

        public ServerConfig(string configPath)
        {
            _configPath = configPath;
            _data = new ServerData();
        }

        public void SaveCredentials(ServerData serverData)
        {
            string json = JsonSerializer.Serialize(serverData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }

        public ServerData LoadCredentials()
        {
            ServerData savedServerData = null;
            if (!File.Exists(_configPath))
            {
                using (File.Create(_configPath)) { }

                ServerData settings = new ServerData()
                {
                    IP = "localhost",
                    Port = "7146"
                };

                string jsonCreate = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(_configPath, jsonCreate);
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var json = File.ReadAllText(_configPath);
            ServerData deserializedServerData = JsonSerializer.Deserialize<ServerData>(json, options);

            _data = deserializedServerData;

            return deserializedServerData;
        }
    }

    public class UserConfig
    {
        private string _configPath;
        private class UserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public UserConfig(string configPath)
        {
            _configPath = configPath;
        }

        public void DeleteCredentials()
        {
            if (File.Exists(_configPath))
                File.Delete(_configPath);
        }
        public void SaveCredentials(User user)
        {
            UserData settings = new UserData()
            {
                Username = user.Username,
                Password = user.Password
            };

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }

        public User LoadCredentials()
        {
            User savedUser = null;
            if (!File.Exists(_configPath))
            {
                using (File.Create(_configPath)) { }

                UserData settings = new UserData()
                {
                    Username = "",
                    Password = ""
                };

                string jsonCreate = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(_configPath, jsonCreate);
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var json = File.ReadAllText(_configPath);
            UserData deserializedUser = JsonSerializer.Deserialize<UserData>(json, options);
            if (deserializedUser != null && (deserializedUser.Username != "" && deserializedUser.Password != ""))
            {
                savedUser = new User(0, deserializedUser.Username, "email", deserializedUser.Password, "name");
            }

            return savedUser;
        }
    }
}
