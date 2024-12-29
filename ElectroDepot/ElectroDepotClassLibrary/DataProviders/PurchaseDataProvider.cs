using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using System.Text.Json;
using System.Text;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class PurchaseDataProvider : BaseDataProvider
    {
        public PurchaseDataProvider(string url) : base(url)
        {
        }
        #region API Calls
        /// <summary>
        /// Creates new 'Purchase' in database.
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task<Purchase> CreatePurchase(Purchase purchase)
        {
            var json = JsonSerializer.Serialize(purchase);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = PurchaseEndpoints.Create();
            var response = HTTPClient.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var resjson = await response.Content.ReadAsStringAsync();
                PurchaseDTO? dbpurchase = JsonSerializer.Deserialize<PurchaseDTO>(resjson, options);
                return dbpurchase?.ToModel();
            }
            return null;
        }

        public async Task<Purchase> GetPurchaseByID(int ID)
        {
            try
            {
                string url = PurchaseEndpoints.GetByID(ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    PurchaseDTO? purchase = JsonSerializer.Deserialize<PurchaseDTO>(json, options);
                    return purchase?.ToModel();
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

        /// <summary>
        /// Gets all 'Purchase's from database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Purchase>> GetAllPurchases()
        {
            try
            {
                string url = PurchaseEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseDTO> purchases = JsonSerializer.Deserialize<IEnumerable<PurchaseDTO>>(json, options);
                    List<Purchase> purchasesModels = purchases.Select(x=>x.ToModel()).ToList();

                    return purchasesModels;
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
        /// <summary>
        /// Gets all 'Purchases' of given 'User'
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Purchase>> GetAllPurchasesFromUser(User user)
        {
            try
            {
                string url = PurchaseEndpoints.GetAllByUserID(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseDTO> purchases = JsonSerializer.Deserialize<IEnumerable<PurchaseDTO>>(json, options);
                    List<Purchase> purchasesModels = purchases.Select(x=>x.ToModel()).ToList();
                    return purchasesModels;
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
        /// <summary>
        /// Gets all 'Purchases' of given 'Supplier'
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Purchase>> GetAllPurchasesFromSupplier(Supplier supplier)
        {
            try
            {
                string url = PurchaseEndpoints.GetAllBySupplierID(supplier.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<PurchaseDTO> purchases = JsonSerializer.Deserialize<IEnumerable<PurchaseDTO>>(json, options);
                    List<Purchase> purchasesModels = purchases.Select(x=>x.ToModel()).ToList();
                    return purchasesModels;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<IEnumerable<double>> GetSpendingsForLastYearFromUser(User user)
        {
            try
            {
                string url = PurchaseEndpoints.GetSpendingsForLastYearFromUser(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<double> spendings = JsonSerializer.Deserialize<IEnumerable<double>>(json, options);
                    return spendings;
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

        /// <summary>
        /// Updated 'Purchase' in database
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePurchase(Purchase purchase)
        {
            try
            {
                UpdatePurchaseDTO updateDTO = purchase.ToUpdateDTO();

                var json = JsonSerializer.Serialize(updateDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = PurchaseEndpoints.Update(purchase.ID);
                var response = await HTTPClient.PutAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch(Exception exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Deletes 'Purchase' from database.
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task<bool> DeletePuchase(Purchase purchase)
        {
            string url = PurchaseEndpoints.Delete(purchase.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
