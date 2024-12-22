using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class PurchasesStore : RootStore
    {
        private readonly PurchaseDataProvider _purchaseDataProvider;
        private readonly PurchaseItemDataProvider _purchaseItemDataProvider;
        private List<Purchase> _purchases;

        public IEnumerable<Purchase> Purchases { get { return _purchases; } }
        public PurchaseDataProvider PurchaseDP { get { return _purchaseDataProvider; } }
        public PurchaseItemDataProvider PurchaseItemDP { get { return _purchaseItemDataProvider; } }

        public event Action PurchasesLoaded;

        public PurchasesStore(DatabaseStore dbStore, PurchaseDataProvider purchaseDataProvider, PurchaseItemDataProvider purchaseItemDataProvider) : base(dbStore)
        {
            _purchaseDataProvider = purchaseDataProvider;
            _purchaseItemDataProvider = purchaseItemDataProvider;
            _purchases = new List<Purchase>();
        }

        public async Task Load()
        {
            _purchases.Clear();

            IEnumerable<Purchase> purchasesFromDB = await _purchaseDataProvider.GetAllPurchasesFromUser(MainStore.UsersStore.LoggedInUser);
            _purchases.AddRange(purchasesFromDB);

            PurchasesLoaded?.Invoke();
        }
    }
}
