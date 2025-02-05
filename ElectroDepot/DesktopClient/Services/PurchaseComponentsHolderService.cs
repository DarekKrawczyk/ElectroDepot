using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    internal class PurchaseComponentsHolderService
    {
        private readonly PurchasesPageViewModel _viewModel;
        private readonly PurchasesStore _purchasesStore;
        private readonly SuppliersStore _suppliersStore;
        private readonly ISourceCache<PurchaseItemComponentContainerHolder, int> _purchases;

        public event Action DataLoaded;

        public PurchaseComponentsHolderService(PurchasesPageViewModel viewModel, PurchasesStore purchasesStore)
        {
            _viewModel = viewModel;
            _purchasesStore = purchasesStore;
            _suppliersStore = purchasesStore.MainStore.SupplierStore;
            _purchases = new SourceCache<PurchaseItemComponentContainerHolder, int>(e => e.PurchaseItem.ComponentID);
        }

        public IObservable<IChangeSet<PurchaseItemComponentContainerHolder, int>> EmployeesConnection() => _purchases.Connect();

        public async Task LoadData(Purchase purchase)
        {
            _purchases.Clear();

            IEnumerable<PurchaseItem> purchaseItemsFromDB = await _purchasesStore.PurchaseItemDP.GetAllPurchaseItemsFromPurchase(purchase);
            foreach (PurchaseItem purchaseItem in purchaseItemsFromDB)
            {
                Component componentFromDB = await _purchasesStore.MainStore.ComponentStore.ComponentDP.GetComponentByIDWithImage(purchaseItem.ComponentID);
                Category categoryFromDB = _purchasesStore.MainStore.CategorieStore.Categories.FirstOrDefault(x => x.ID == componentFromDB.CategoryID);

                _purchases.AddOrUpdate(new PurchaseItemComponentContainerHolder(_viewModel, new PurchaseItemComponentContainer(componentFromDB, purchaseItem, categoryFromDB)));
            }
            DataLoaded!.Invoke();
        }

        public void ClearData()
        {
            _purchases.Clear();

            DataLoaded!.Invoke();
        }
    }
}
