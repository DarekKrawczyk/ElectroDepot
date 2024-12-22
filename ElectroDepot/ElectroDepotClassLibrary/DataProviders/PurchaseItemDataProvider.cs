using System.Text;
using System.Text.Json;
using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Endpoints;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class PurchaseItemDataProvider : BaseDataProvider
    {
        public PurchaseItemDataProvider(string url) : base(url)
        {
        }
        #region API Calls
        public async Task<bool> CreatePurchaseItem(PurchaseItem PurchaseItem)
        {
            var json = JsonSerializer.Serialize(PurchaseItem.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = PurchaseItemEndpoints.Create();
            var response = await HTTPClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        
        public async Task<IEnumerable<PurchaseItem>> GetPurchaseItemsFromComponent(Component component)
        {
            try
            {
                string url = ComponentEndpoints.GetPurchaseItemsFromComponent(component.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseItemDTO> purchaseItems = JsonSerializer.Deserialize<IEnumerable<PurchaseItemDTO>>(json, options);

                    return purchaseItems.Select(x => x.ToModel()).ToList();
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


        public async Task<IEnumerable<PurchaseItem>> GetAllPurchaseItems()
        {
            try
            {
                string url = PurchaseItemEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseItemDTO> purchaseItems = JsonSerializer.Deserialize<IEnumerable<PurchaseItemDTO>>(json, options);

                    return purchaseItems.Select(x=>x.ToModel()).ToList();
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

        public async Task<IEnumerable<Component>> GetAllComponentsFromPurchase(Purchase purchase)
        {
            try
            {
                string url = PurchaseItemEndpoints.GetAllComponentsFromPurchase(purchase.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ComponentDTO> components = JsonSerializer.Deserialize<IEnumerable<ComponentDTO>>(json, options);

                    return components.Select(x=>x.ToModel()).ToList();
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

        public async Task<IEnumerable<PurchaseItem>> GetAllPurchaseItemsFromPurchase(Purchase purchase)
        {
            try
            {
                string url = PurchaseItemEndpoints.GetAllPurchaseItemsFromPurchase(purchase.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseItemDTO> components = JsonSerializer.Deserialize<IEnumerable<PurchaseItemDTO>>(json, options);

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

        public async Task<PurchaseItem> GetPurchaseItemByID(int ID)
        {
            try
            {
                string url = PurchaseItemEndpoints.GetByID(ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    PurchaseItemDTO purchaseItems = JsonSerializer.Deserialize<PurchaseItemDTO>(json, options);

                    return purchaseItems.ToModel();
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

        public async Task<bool> UpdatePurchaseItem(PurchaseItem PurchaseItem)
        {
            var json = JsonSerializer.Serialize(PurchaseItem.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = PurchaseItemEndpoints.Update(PurchaseItem.ID);
            var response = await HTTPClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePuchaseItem(PurchaseItem PurchaseItem)
        {
            string url = PurchaseItemEndpoints.Delete(PurchaseItem.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
