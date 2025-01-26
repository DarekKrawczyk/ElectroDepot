using System.Text;
using System.Text.Json;
using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Endpoints;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class UserDataProvider : BaseDataProvider
    {
        public UserDataProvider(string url) : base(url) { }
        #region API Calls
        public async Task<User> CreateUser(User user)
        {
            var json = JsonSerializer.Serialize(user.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = UserEndpoints.Create();
            var response = await HTTPClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var resjson = await response.Content.ReadAsStringAsync();
                UserDTO? userDTO = JsonSerializer.Deserialize<UserDTO>(resjson, options);

                return userDTO.ToModel();
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetUserByUsername(string name)
        {
            try
            {
                string url = UserEndpoints.GetByUsername(name);
                var response = await HTTPClient.GetAsync(url);

                // TODO: Delete dummy wait time. Because of that UI thread is forzen and spinning circle is not spinning.
                //Thread.Sleep(3000);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    UserDTO? user = JsonSerializer.Deserialize<UserDTO>(json, options);

                    return user.ToModel();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UserWithUsernameExists(string username)
        {
            User? found = await GetUserByUsername(username);
            if (found == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User> GetUserByEMail(string EMail)
        {
            try
            {
                string url = UserEndpoints.GetByEMail(EMail);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    UserDTO? user = JsonSerializer.Deserialize<UserDTO>(json, options);

                    return user.ToModel();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UserWithEMailExists(string EMail)
        {
            User? found = await GetUserByEMail(EMail);
            if (found == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User> GetUserByID(int ID)
        {
            try
            {
                string url = UserEndpoints.GetByID(ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    UserDTO? user = JsonSerializer.Deserialize<UserDTO>(json, options);

                    return user.ToModel();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateUser(int UserID, User user)
        {
            var json = JsonSerializer.Serialize(user.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = UserEndpoints.Update(UserID);
            var response = await HTTPClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                string url = UserEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<UserDTO> users = JsonSerializer.Deserialize<IEnumerable<UserDTO>>(json, options);

                    return users.Select(x=>x.ToModel()).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(User user)
        {
            string url = UserEndpoints.Delete(user.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int ID)
        {
            string url = UserEndpoints.Delete(ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
