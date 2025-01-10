using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class PurchaseDataProviderTests : BaseDataProviderTest
    {
        public PurchaseDataProviderTests(ITestOutputHelper output) : base(output) { }
        [Fact]
        public async Task Create()
        {
            try
            {
                // Find 'User'
                IEnumerable<User> users = await UserDP.GetAllUsers();
                Assert.NotNull(users);
                Assert.NotEmpty(users);
                User? user = users.FirstOrDefault();
                Assert.NotNull(user);

                // Find 'Supplier'
                IEnumerable<Supplier> suppliers = await SupplierDP.GetAllSuppliers();
                Assert.NotNull(suppliers);
                Assert.NotEmpty(suppliers);
                Supplier? supplier = suppliers.FirstOrDefault();
                Assert.NotNull(supplier);

                // Create new 'Purchase'
                Purchase newPurchase = new Purchase(iD: 0, userID: user.ID, user: user, supplierID: supplier.ID, supplier: supplier, purchaseDate: DateTime.Now, totalPrice: 0);
                Purchase purchaseFromDB = await PurchaseDP.CreatePurchase(newPurchase);
                Assert.True(purchaseFromDB != null);

                Console.WriteLine(newPurchase.ToString());
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
                IEnumerable<Purchase> purchases = await PurchaseDP.GetAllPurchases();
                Assert.NotNull(purchases);
                foreach (Purchase purchase in purchases)
                {
                    Console.WriteLine(purchase.ToString());
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task GetAllByUserID()
        {
            try
            {
                // Find 'User'
                IEnumerable<User> users = await UserDP.GetAllUsers();
                Assert.NotNull(users);
                Assert.NotEmpty(users);
                User? user = users.FirstOrDefault();
                Assert.NotNull(user);

                IEnumerable<Purchase> purchaeses = await PurchaseDP.GetAllPurchasesFromUser(user);
                Assert.NotNull(purchaeses);

                foreach (Purchase purchase in purchaeses)
                {
                    Console.WriteLine(purchase.ToString());
                }

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task GetAllBySupplierID()
        {
            try
            {
                // Find 'Supplier'
                IEnumerable<Supplier> suppliers = await SupplierDP.GetAllSuppliers();
                Assert.NotNull(suppliers);
                Assert.NotEmpty(suppliers);
                Supplier? supplier = suppliers.FirstOrDefault();
                Assert.NotNull(supplier);

                IEnumerable<Purchase> purchaeses = await PurchaseDP.GetAllPurchasesFromSupplier(supplier);
                Assert.NotNull(purchaeses);

                foreach (Purchase purchase in purchaeses)
                {
                    Console.WriteLine(purchase.ToString());
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
                IEnumerable<Purchase> allPurchases = await PurchaseDP.GetAllPurchases();
                Assert.NotNull(allPurchases);
                Assert.NotEmpty(allPurchases);
                Purchase? purchaseToBeEdited = allPurchases.FirstOrDefault();
                Assert.NotNull(purchaseToBeEdited);

                Console.WriteLine(purchaseToBeEdited.ToString());

                // Update
                Purchase purchase = new Purchase(iD: purchaseToBeEdited.ID, userID: purchaseToBeEdited.UserID, user: null, supplierID: purchaseToBeEdited.SupplierID, supplier: null, purchaseDate: purchaseToBeEdited.PurchaseDate, totalPrice: 201);
                bool wasUpdated = await PurchaseDP.UpdatePurchase(purchase);
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
                // Find any purchase
                IEnumerable<Supplier> suppliers = await SupplierDP.GetAllSuppliers();
                Assert.NotNull(suppliers);
                Assert.NotEmpty(suppliers);
                Supplier? lastSupplier = suppliers.LastOrDefault();
                Assert.NotNull(lastSupplier);

                // Delete
                bool wasDeleted = await SupplierDP.DeleteSupplier(lastSupplier);
                Assert.True(wasDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
