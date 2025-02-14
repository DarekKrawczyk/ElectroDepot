using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using Svg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class PurchaseHolderService
    {
        private readonly PurchasesPageViewModel _viewModel;
        private readonly PurchasesStore _purchasesStore;
        private readonly SuppliersStore _suppliersStore;
        private readonly ISourceCache<DetailedPurchaseContainerHolder, int> _purchases;

        public event Action DataLoaded;

        public PurchaseHolderService(PurchasesPageViewModel viewModel, PurchasesStore purchasesStore)
        {
            _viewModel = viewModel;
            _purchasesStore = purchasesStore;
            _suppliersStore = purchasesStore.MainStore.SupplierStore;
            _purchases = new SourceCache<DetailedPurchaseContainerHolder, int>(e => e.Container.ID);
        }

        public IObservable<IChangeSet<DetailedPurchaseContainerHolder, int>> EmployeesConnection() => _purchases.Connect();

        public DetailedPurchaseContainerHolder FindItem(Purchase purchase)
        {
            return _purchases.Items.FirstOrDefault(x=>x.Container.Purchase.ID == purchase.ID);
        }

        public double MaxPrice()
        {
            if (_purchases.Items.Count == 0)
            {
                return 1;
            }
            return _purchases.Items.Max(x=>x.Container.TotalPrice);
        }

        public double MinPrice()
        {
            if (_purchases.Items.Count == 0)
            {
                return 0;
            }
            return _purchases.Items.Min(x => x.Container.TotalPrice);
        }

        public DateTime MaxYear()
        {
            if (_purchases.Items.Count == 0)
            {
                return DateTime.Now;
            }
            return _purchases.Items.Max(x => x.Container.PurchaseDate);
        }

        public DateTime MinYear()
        {
            if (_purchases.Items.Count == 0)
            {
                return DateTime.Now;
            }
            return _purchases.Items.Min(x => x.Container.PurchaseDate);
        }

        public async Task LoadData()
        {
            _purchases.Clear();

            IEnumerable<Purchase> purchasesFromDB = await _purchasesStore.PurchaseDP.GetAllPurchasesFromUser(_purchasesStore.MainStore.UsersStore.LoggedInUser);
            List<Purchase> sortedPurchases = purchasesFromDB.OrderByDescending(x=>x.PurchaseDate).ToList();
            foreach (Purchase purchase in sortedPurchases)
            {
                Supplier supplier = await _purchasesStore.MainStore.SupplierStore.SupplierDP.GetSupplierByID(purchase.SupplierID);
                IEnumerable<PurchaseItem> purchaseItemsFromDB = await _purchasesStore.PurchaseItemDP.GetAllPurchaseItemsFromPurchase(purchase);

                _purchases.AddOrUpdate(new DetailedPurchaseContainerHolder(new DetailedPurchaseContainer(purchase, supplier, purchaseItemsFromDB), _viewModel));
            }
            DataLoaded!.Invoke();
        }
    }
}
