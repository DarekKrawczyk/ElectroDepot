using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using ElectroDepotClassLibrary.Models;
using System.Text;
using System.Text.Json;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class PredefinedImageDataProvider : BaseDataProvider
    {
        public PredefinedImageDataProvider(string url) : base(url) { }
        
        public async Task<bool> Create(PredefinedImage predefinedImage)
        {
            var json = JsonSerializer.Serialize(predefinedImage.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = PredefinedImageEndpoint.Create();
            var response = HTTPClient.PostAsync(url, content).Result;

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<PredefinedImage>> GetAll()
        {
            try
            {
                string url = PredefinedImageEndpoint.GetAllPredefinedImages();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PredefinedImageDTO> images = JsonSerializer.Deserialize<IEnumerable<PredefinedImageDTO>>(json, options);
                    return images.Select(x => x.ToModel()).ToList();
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

        public async Task<bool> Update(PredefinedImage predefinedImage)
        {
            var json = JsonSerializer.Serialize(predefinedImage.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = PredefinedImageEndpoint.Update(predefinedImage.ID);
            var response = await HTTPClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(PredefinedImage predefinedImage)
        {
            string url = PredefinedImageEndpoint.Delete(predefinedImage.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
