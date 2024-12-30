using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DesktopClient.ViewModels
{
    public partial class PurchasesPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _previewEnabled;

        [ObservableProperty]
        private int _selectedTab;

        partial void OnSelectedTabChanged(int oldValue, int newValue)
        {
            switch (newValue)
            {
                case 0:
                    // Collection
                    break;
                case 1:
                    // Add
                    break;
                case 2:
                    // Import
                    break;
                case 3:
                    // Preview;
                    DatabaseStore.PurchaseStore.GetPurchaseItemContainerFromPurchase(SelectedPurchase.Purchase);
                    break;
                default:
                    break;
            }
        }

        [ObservableProperty]
        private DetailedPurchaseContainer _selectedPurchase;

        partial void OnSelectedPurchaseChanged(DetailedPurchaseContainer? oldValue, DetailedPurchaseContainer newValue)
        {
            if (newValue != null)
            {
                PreviewEnabled = true;
            }
            else
            {
                PreviewEnabled = false;
            }
        }

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
        private DateTimeOffset _collection_SelectedFromDate;

        partial void OnCollection_SelectedFromDateChanged(DateTimeOffset oldValue, DateTimeOffset newValue)
        {
            Purchases.Refresh();
        }

        [ObservableProperty]
        private DateTimeOffset _collection_SelectedToDate;

        partial void OnCollection_SelectedToDateChanged(DateTimeOffset oldValue, DateTimeOffset newValue)
        {
            Purchases.Refresh();
        }

        [ObservableProperty]
        private string _collection_SelectedSupplier;

        partial void OnCollection_SelectedSupplierChanged(string? oldValue, string newValue)
        {
            Purchases.Refresh();
        }

        [RelayCommand]
        private void Collection_ClearSettings()
        {
            Collection_SelectedFromDate = Collection_DateMinYear;
            Collection_SelectedToDate = Collection_DateMaxYear;

            Collection_SelectedHighPrice = Collection_MaxPrice;
            Collection_SelectedLowPrice = Collection_MinPrice;

            Collection_SelectedSupplier = null;
        }

        [ObservableProperty]
        private double _collection_PriceFrom;
        
        [ObservableProperty]
        private double _collection_PriceTo;

        [ObservableProperty]
        private DateTimeOffset _collection_DateMinYear;
        
        [ObservableProperty]
        private DateTimeOffset _collection_DateMaxYear;

        public List<PurchaseItemContainer> ProjectItems { get; set; }
        public DataGridCollectionView Preview_ProjectItems { get; set; }

        public ObservableCollection<string> SupplierSource { get; set; }
        public List<DetailedPurchaseContainer> PurchasesSource { get; set; }
        public DataGridCollectionView Purchases { get; set; }
        public PurchasesPageViewModel(DatabaseStore databaseStore) : base(databaseStore)
        {
            PurchasesSource = new List<DetailedPurchaseContainer>();
            ProjectItems = new List<PurchaseItemContainer>();
            SupplierSource = new ObservableCollection<string>();

            Preview_ProjectItems = new DataGridCollectionView(ProjectItems);

            // TODO: https://github.com/AvaloniaUI/Avalonia/discussions/16042 implement this for PurchaseItem Component DataGrid

            Purchases = new DataGridCollectionView(PurchasesSource);
            Purchases.Filter = (object component) =>
            {
                if (component is DetailedPurchaseContainer detailedPurchase)
                {
                    bool priceOk = false;
                    bool dateFrom = true;
                    bool dateTo = true;
                    bool goodSupplier = true;

                    if (detailedPurchase.TotalPrice >= Collection_SelectedLowPrice && detailedPurchase.TotalPrice <= Collection_SelectedHighPrice)
                    {
                        priceOk = true;
                    }

                    if(Collection_SelectedFromDate != null)
                    {
                        if (Collection_SelectedFromDate != DateTimeOffset.MinValue)
                        {
                            if(Collection_SelectedFromDate > detailedPurchase.PurchaseDate)
                            {
                                dateFrom = false;
                            }
                        }
                    }

                    if (Collection_SelectedToDate != null)
                    {
                        if (Collection_SelectedToDate != DateTimeOffset.MinValue)
                        {
                            if (Collection_SelectedToDate < detailedPurchase.PurchaseDate)
                            {
                                dateTo = false;
                            }
                        }
                    }

                    if(Collection_SelectedSupplier != null)
                    {
                        if(detailedPurchase.SupplierName != Collection_SelectedSupplier)
                        {
                            goodSupplier = false;
                        }
                    }

                    if (priceOk == true && dateFrom == true && dateTo == true && goodSupplier == true)
                    {
                        return true;
                    }
                }
                return false;
            };

            DatabaseStore.PurchaseStore.PurchaseItemsContainersLoaded += PurchaseStore_PurchaseItemsContainersLoadedHandler;

            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded += PurchaseStore_PurchasesLoadedHandler;
            DatabaseStore.PurchaseStore.LoadDetailedPurchaseContainers();

            DatabaseStore.SupplierStore.SuppliersLoaded += SupplierStore_SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.Load();
        }

        private void PurchaseStore_PurchaseItemsContainersLoadedHandler()
        {
            ProjectItems.Clear();
            ProjectItems.AddRange(DatabaseStore.PurchaseStore.PurchaseItemsContainers);
            Preview_ProjectItems.Refresh();

        }

        private void SupplierStore_SuppliersLoadedHandler()
        {
            SupplierSource.Clear();
            IEnumerable<Supplier> suppliersFromDB = DatabaseStore.SupplierStore.Suppliers;
            foreach (Supplier supplier in suppliersFromDB)
            {
                SupplierSource.Add(supplier.Name);
            }
            Purchases.Refresh();
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
            Collection_SelectedFromDate = minDate;
            Collection_SelectedToDate = maxDate;
        }

        private void PurchaseStore_PurchasesLoadedHandler()
        {
            IEnumerable<DetailedPurchaseContainer> purchasesFromDB = DatabaseStore.PurchaseStore.DetailedPurchaseContainers;
            PurchasesSource.Clear();
            PurchasesSource.AddRange(purchasesFromDB);
            AdjustFilters();
            Purchases.Refresh();
        }

        public override void Dispose()
        {
            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded -= PurchaseStore_PurchasesLoadedHandler;
        }
    }
}
