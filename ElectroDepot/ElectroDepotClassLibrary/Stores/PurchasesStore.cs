using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class PurchasesStore : RootStore
    {
        private readonly PurchaseDataProvider _purchaseDataProvider;
        private readonly PurchaseItemDataProvider _purchaseItemDataProvider;
        private List<Purchase> _purchases;
        private readonly List<DetailedPurchaseContainer> _detailedPurchaseContainers;

        public IEnumerable<Purchase> Purchases { get { return _purchases; } }
        public IEnumerable<DetailedPurchaseContainer> DetailedPurchaseContainers { get { return _detailedPurchaseContainers; } }
        public PurchaseDataProvider PurchaseDP { get { return _purchaseDataProvider; } }
        public PurchaseItemDataProvider PurchaseItemDP { get { return _purchaseItemDataProvider; } }

        public event Action PurchasesLoaded;
        public event Action DetailedPurchaseContainersLoaded;

        public PurchasesStore(DatabaseStore dbStore, PurchaseDataProvider purchaseDataProvider, PurchaseItemDataProvider purchaseItemDataProvider) : base(dbStore)
        {
            _purchaseDataProvider = purchaseDataProvider;
            _purchaseItemDataProvider = purchaseItemDataProvider;
            _purchases = new List<Purchase>();
            _detailedPurchaseContainers = new List<DetailedPurchaseContainer>();
        }

        public async Task Load()
        {
            _purchases.Clear();

            IEnumerable<Purchase> purchasesFromDB = await _purchaseDataProvider.GetAllPurchasesFromUser(MainStore.UsersStore.LoggedInUser);
            _purchases.AddRange(purchasesFromDB);

            PurchasesLoaded?.Invoke();
        }

        public async Task LoadDetailedPurchaseContainers()
        {
            _detailedPurchaseContainers.Clear();

            IEnumerable<Purchase> purchasesFromDB = await _purchaseDataProvider.GetAllPurchasesFromUser(MainStore.UsersStore.LoggedInUser);
            foreach(Purchase purchase in purchasesFromDB)
            {
                Supplier supplier = await MainStore.SupplierStore.SupplierDP.GetSupplierByID(purchase.SupplierID);
                IEnumerable<PurchaseItem> purchaseItemsFromDB = await _purchaseItemDataProvider.GetAllPurchaseItemsFromPurchase(purchase);

                _detailedPurchaseContainers.Add(new DetailedPurchaseContainer(purchase, supplier, purchaseItemsFromDB));
            }

            DetailedPurchaseContainersLoaded?.Invoke();
        }
    }
}
