using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using System.Text.Json;
using System.Text;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class OwnsComponentDataProvider : BaseDataProvider
    {
        public OwnsComponentDataProvider(string url) : base(url) { }
        #region API Calls
        public async Task<OwnsComponent> CreateOwnComponent(OwnsComponent ownsComponent)
        {
            var json = JsonSerializer.Serialize(ownsComponent.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = OwnsComponentEndpoints.Create();
            var response = HTTPClient.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var resultJson = await response.Content.ReadAsStringAsync();
                OwnsComponentDTO images = JsonSerializer.Deserialize<OwnsComponentDTO>(resultJson, options);
                return images.ToModel();
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<OwnsComponent>> GetAllOwnsComponents()
        {
            try
            {
                string url = OwnsComponentEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<OwnsComponentDTO> images = JsonSerializer.Deserialize<IEnumerable<OwnsComponentDTO>>(json, options);
                    return images.Select(x=>x.ToModel()).ToList();
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

        public async Task<IEnumerable<OwnsComponent>> GetAllUsedComponentsFromUser(User user)
        {
            try
            {
                string url = OwnsComponentEndpoints.GetAllUsedFromUser(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<OwnsComponentDTO> components = JsonSerializer.Deserialize<IEnumerable<OwnsComponentDTO>>(json, options);
                    return components.Select(x => x.ToModel()).ToList();
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

        public async Task<IEnumerable<OwnsComponent>> GetAllFreeToUseComponentsFromUser(User user)
        {
            try
            {
                string url = OwnsComponentEndpoints.GetAllFreeToUseFromUser(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<OwnsComponentDTO> components = JsonSerializer.Deserialize<IEnumerable<OwnsComponentDTO>>(json, options);
                    return components.Select(x => x.ToModel()).ToList();
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

        public async Task<IEnumerable<OwnsComponent>> GetAllUnusedComponents(User user)
        {
            try
            {
                string url = OwnsComponentEndpoints.GetAllUnusedComponents(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<OwnsComponentDTO> components = JsonSerializer.Deserialize<IEnumerable<OwnsComponentDTO>>(json, options);
                    return components.Select(x => x.ToModel()).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<OwnsComponent>> GetAllOwnsComponentsFromUser(User user)
        {
            try
            {
                string url = OwnsComponentEndpoints.GetAllOwnComponentFromUser(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<OwnsComponentDTO> components = JsonSerializer.Deserialize<IEnumerable<OwnsComponentDTO>>(json, options);
                    return components.Select(x => x.ToModel()).ToList();
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

        public async Task<OwnsComponent> GetOwnComponentsFromUser(User user, Component component)
        {
            try
            {
                string url = OwnsComponentEndpoints.GetOwnComponentFromUser(user.ID, component.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    OwnsComponentDTO ownsComponent = JsonSerializer.Deserialize<OwnsComponentDTO>(json, options);
                    return ownsComponent.ToModel();
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

        public async Task<bool> UpdateOwnsComponent(OwnsComponent ownsComponent)
        {
            var json = JsonSerializer.Serialize(ownsComponent.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ComponentEndpoints.Update(ownsComponent.ID);
            var response = await HTTPClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteComponent(OwnsComponent ownsComponent)
        {
            string url = ComponentEndpoints.Delete(ownsComponent.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}