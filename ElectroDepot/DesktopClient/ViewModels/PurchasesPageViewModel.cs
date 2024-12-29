using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DesktopClient.ViewModels
{
    public partial class PurchasesPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private double _collection_MinPrice = 0;

        [ObservableProperty]
        private double _collection_MaxPrice = 1;

        [ObservableProperty]
        private double _collection_SelectedLowPrice = 0;

        partial void OnCollection_SelectedLowPriceChanged(double oldValue, double newValue)
        {
            Collection_PriceFrom = newValue;
            Purchases.Refresh();
        }

        [ObservableProperty]
        private double _collection_SelectedHighPrice = 1;

        partial void OnCollection_SelectedHighPriceChanged(double oldValue, double newValue)
        {
            Collection_PriceTo = newValue;
            Purchases.Refresh();
        }

        [ObservableProperty]
        private double _collection_PriceFrom;
        
        [ObservableProperty]
        private double _collection_PriceTo;

        [ObservableProperty]
        private DateTimeOffset _collection_DateMinYear;
        
        [ObservableProperty]
        private DateTimeOffset _collection_DateMaxYear;

        public List<DetailedPurchaseContainer> PurchasesSource { get; set; }
        public DataGridCollectionView Purchases { get; set; }
        public PurchasesPageViewModel(DatabaseStore databaseStore) : base(databaseStore)
        {
            PurchasesSource = new List<DetailedPurchaseContainer>();
            Purchases = new DataGridCollectionView(PurchasesSource);
            Purchases.CurrentChanged += Purchases_CurrentChangedHandler;
            Purchases.Filter = (object component) =>
            {
                if (component is DetailedPurchaseContainer detailedPurchase)
                {
                    bool priceOk = false;

                    if (detailedPurchase.TotalPrice >= Collection_SelectedLowPrice && detailedPurchase.TotalPrice <= Collection_SelectedHighPrice)
                    {
                        priceOk = true;
                    }

                    if (priceOk == true)
                    {
                        return true;
                    }
                }
                return false;
            };

            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded += PurchaseStore_PurchasesLoadedHandler;
            DatabaseStore.PurchaseStore.LoadDetailedPurchaseContainers();
        }

        private void AdjustFilters()
        {
            double minPrice = PurchasesSource.Min(x => x.TotalPrice);
            double maxPrice = PurchasesSource.Max(x => x.TotalPrice);

            Collection_MaxPrice = maxPrice;
            Collection_SelectedHighPrice = maxPrice;
            Collection_MinPrice = minPrice;
            Collection_SelectedLowPrice = minPrice;

            DateTime minDate = PurchasesSource.Min(x => x.PurchaseDate);
            DateTime maxDate = PurchasesSource.Max(x => x.PurchaseDate);

            Collection_DateMaxYear = maxDate;
            Collection_DateMinYear = minDate;
        }

        private void PurchaseStore_PurchasesLoadedHandler()
        {
            IEnumerable<DetailedPurchaseContainer> purchasesFromDB = DatabaseStore.PurchaseStore.DetailedPurchaseContainers;
            PurchasesSource.Clear();
            PurchasesSource.AddRange(purchasesFromDB);
            AdjustFilters();
            Purchases.Refresh();
        }

        private void Purchases_CurrentChangedHandler(object? sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded -= PurchaseStore_PurchasesLoadedHandler;
        }
    }
}
