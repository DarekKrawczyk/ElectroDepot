using ElectroDepotClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DesktopClient.Services
{
    internal class SecureStorageHelperService
    {
        private class UserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "user_data.json");

        public void SaveCredentials(User user)
        {
            UserData settings = new UserData ()
            { 
                Username = user.Username, 
                Password = user.Password 
            };

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, json);
        }

        public User LoadCredentials()
        {
            User savedUser = null;
            if (!File.Exists(configPath)) return savedUser;

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var json = File.ReadAllText(configPath);
            UserData deserializedUser = JsonSerializer.Deserialize<UserData>(json, options);
            if (deserializedUser != null)
            {
                savedUser = new User(0, deserializedUser.Username, "email", deserializedUser.Password, "name");
            }

            return savedUser;
        }

        public void DeleteCredentials()
        {
            if (File.Exists(configPath))
                File.Delete(configPath);
        }
    }
}
