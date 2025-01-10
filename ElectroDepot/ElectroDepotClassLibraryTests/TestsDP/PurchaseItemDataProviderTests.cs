using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class PurchaseItemDataProviderTests : BaseDataProviderTest
    {
        public PurchaseItemDataProviderTests(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public async Task Create()
        {
            try
            {
                // Find 'Purchase' and 'Component'
                IEnumerable<Purchase> purchases = await PurchaseDP.GetAllPurchases();
                Assert.NotNull(purchases);
                Assert.NotEmpty(purchases);
                Purchase? purchase = purchases.FirstOrDefault();
                Assert.NotNull(purchase);

                IEnumerable<Component> componenets = await ComponentDP.GetAllComponents();
                Assert.NotNull(componenets);
                Assert.NotEmpty(componenets);
                Component? component = componenets.FirstOrDefault();
                Assert.NotNull(component);

                PurchaseItem purchaseItem = new PurchaseItem(id: 0, purchaseID: purchase.ID, componentID: component.ID, quantity: 30, pricePerUnit: 19.99);
                PurchaseItem purchaseItemFromDB = await PurchaseItemDP.CreatePurchaseItem(purchaseItem);
                Assert.True(purchaseItemFromDB != null);

                Console.WriteLine(purchaseItem.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task GetByID()
        {
            try
            {
                IEnumerable<PurchaseItem> purchases = await PurchaseItemDP.GetAllPurchaseItems();
                Assert.NotNull(purchases);
                Assert.NotEmpty(purchases);
                PurchaseItem? purchase = purchases.FirstOrDefault();
                Assert.NotNull(purchase);

                int ID = purchase.ID;
                purchase = null;

                PurchaseItem purchaseItem = await PurchaseItemDP.GetPurchaseItemByID(ID);
                Assert.NotNull(purchaseItem);
                Console.WriteLine(purchaseItem.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task GetAll()
        {
            try
            {
                IEnumerable<PurchaseItem> purchaseItems = await PurchaseItemDP.GetAllPurchaseItems();
                Assert.NotNull(purchaseItems);
                foreach (PurchaseItem purchaseItem in purchaseItems)
                {
                    Console.WriteLine(purchaseItem.ToString());
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task Update()
        {
            try
            {
                // Find
                IEnumerable<PurchaseItem> allPurchases = await PurchaseItemDP.GetAllPurchaseItems();
                Assert.NotNull(allPurchases);
                Assert.NotEmpty(allPurchases);
                PurchaseItem? purchaseToBeEdited = allPurchases.FirstOrDefault();
                Assert.NotNull(purchaseToBeEdited);

                Console.WriteLine(purchaseToBeEdited.ToString());

                // Update
                PurchaseItem purchase = new PurchaseItem(id: purchaseToBeEdited.ID, purchaseID: purchaseToBeEdited.PurchaseID, componentID: purchaseToBeEdited.ComponentID, quantity: 2000, pricePerUnit: purchaseToBeEdited.PricePerUnit);
                bool wasUpdated = await PurchaseItemDP.UpdatePurchaseItem(purchase);
                Assert.True(wasUpdated);
                Console.WriteLine(purchase.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task Delete()
        {
            try
            {
                IEnumerable<PurchaseItem> allPurchaseItems = await PurchaseItemDP.GetAllPurchaseItems();
                Assert.NotNull(allPurchaseItems);
                Assert.NotEmpty(allPurchaseItems);

                foreach (PurchaseItem singleItem in allPurchaseItems)
                {
                    Console.WriteLine(singleItem.ToString());
                }

                PurchaseItem? firstItem = allPurchaseItems.FirstOrDefault();
                bool wasDeleted = await PurchaseItemDP.DeletePuchaseItem(firstItem);
                Assert.True(wasDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
