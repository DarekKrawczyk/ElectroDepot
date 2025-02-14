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
        private readonly List<PurchaseItemContainer> _purchaseItemsContainers;

        public IEnumerable<Purchase> Purchases { get { return _purchases; } }
        public IEnumerable<DetailedPurchaseContainer> DetailedPurchaseContainers { get { return _detailedPurchaseContainers; } }
        public IEnumerable<PurchaseItemContainer> PurchaseItemsContainers { get { return _purchaseItemsContainers; } }
        public PurchaseDataProvider PurchaseDP { get { return _purchaseDataProvider; } }
        public PurchaseItemDataProvider PurchaseItemDP { get { return _purchaseItemDataProvider; } }

        public event Action PurchasesLoaded;
        public event Action DetailedPurchaseContainersLoaded;
        public event Action PurchaseItemsContainersLoaded;

        public PurchasesStore(DatabaseStore dbStore, PurchaseDataProvider purchaseDataProvider, PurchaseItemDataProvider purchaseItemDataProvider) : base(dbStore)
        {
            _purchaseDataProvider = purchaseDataProvider;
            _purchaseItemDataProvider = purchaseItemDataProvider;
            _purchases = new List<Purchase>();
            _detailedPurchaseContainers = new List<DetailedPurchaseContainer>();
            _purchaseItemsContainers = new List<PurchaseItemContainer>();
        }

        private async Task Load()
        {
            _purchases.Clear();

            IEnumerable<Purchase> purchasesFromDB = await _purchaseDataProvider.GetAllPurchasesFromUser(MainStore.UsersStore.LoggedInUser);
            foreach(Purchase purchase in purchasesFromDB)
            {
                purchase.Supplier = await MainStore.SupplierStore.SupplierDP.GetSupplierByID(purchase.SupplierID);
            }
            _purchases.AddRange(purchasesFromDB);

            PurchasesLoaded?.Invoke();
        }

        public async Task ReloadPurchasesData()
        {
            bool reloadRequired = false;
            IEnumerable<Purchase> purchasesFromDB = await _purchaseDataProvider.GetAllPurchasesFromUser(MainStore.UsersStore.LoggedInUser);
            foreach (Purchase purchase in purchasesFromDB)
            {
                purchase.Supplier = await MainStore.SupplierStore.SupplierDP.GetSupplierByID(purchase.SupplierID);
            }

            if(purchasesFromDB.Count() == _purchases.Count)
            {
                for(int i = 0; i <  purchasesFromDB.Count(); i++)
                {
                    if(purchasesFromDB.ElementAt(i).ID != _purchases[i].ID)
                    {
                        reloadRequired = true;
                    }
                }
            }

            if(reloadRequired == true)
            {
                _purchases.Clear();
                _purchases.AddRange(purchasesFromDB);

                PurchasesLoaded?.Invoke();
            }
        }

        public async Task GetPurchaseItemContainerFromPurchase(Purchase purchase)
        {
            _purchaseItemsContainers.Clear();
            IEnumerable<PurchaseItem> purchaseItemsFromDB = await _purchaseItemDataProvider.GetAllPurchaseItemsFromPurchase(purchase);
            foreach (PurchaseItem purchaseItem in purchaseItemsFromDB)
            {
                Component componentFromDB = MainStore.ComponentStore.Components.FirstOrDefault(x => x.ID == purchaseItem.ComponentID);
                _purchaseItemsContainers.Add(new PurchaseItemContainer(purchaseItem, componentFromDB));
            }
            PurchaseItemsContainersLoaded?.Invoke();
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

        public async Task<bool> InsertNewPurchase(Purchase purchase, IEnumerable<PurchaseItem> purchaseItems)
        {
            try
            {
                Purchase purchaseFromDB = await PurchaseDP.CreatePurchase(purchase);

                _purchases.Add(purchaseFromDB);

                List<PurchaseItem> purchaseItemsFromDB = new List<PurchaseItem>();

                foreach(PurchaseItem pItem in purchaseItems)
                {
                    pItem.PurchaseID = purchaseFromDB.ID;
                    purchaseItemsFromDB.Add(await PurchaseItemDP.CreatePurchaseItem(pItem));
                }

                DetailedPurchaseContainer dPurchase = new DetailedPurchaseContainer(purchaseFromDB, purchase.Supplier, purchaseItemsFromDB);
                _detailedPurchaseContainers.Add(dPurchase);

                PurchasesLoaded?.Invoke();
                DetailedPurchaseContainersLoaded?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
    }
}
