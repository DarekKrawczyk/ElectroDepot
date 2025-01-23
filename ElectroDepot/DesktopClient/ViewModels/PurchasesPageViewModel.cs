using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using Microsoft.VisualBasic;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Platform.Storage;
using DesktopClient.Navigation;
using DesktopClient.Services;

namespace DesktopClient.ViewModels
{
    public partial class PurchasesPageViewModel : RootNavigatorViewModel, INavParamInterpreter
    {
        [RelayCommand]
        public void Collection_Refresh()
        {
            IEnumerable<DetailedPurchaseContainer> purchasesFromDB = DatabaseStore.PurchaseStore.DetailedPurchaseContainers;
            PurchasesSource.Clear();
            foreach (DetailedPurchaseContainer dPurchase in purchasesFromDB)
            {
                PurchasesSource.Add(new DetailedPurchaseContainerHolder(dPurchase));
            }
            AdjustFilters();
            Purchases.Refresh();
        }

        public void RefreshPurchasedComponents()
        {
            try
            {
                PurchaseComponents.Refresh();
            }
            catch (Exception exception)
            {
                // Error handled
            }
        }

        private bool _hasUserInteractedWithSupplier = false;
        [ObservableProperty]
        private bool _add_CanAdd;

        [ObservableProperty]
        [Required]
        private DateTime _add_SelectedDate = DateTime.Now;

        [RelayCommand]
        private void Add_Clear()
        {
            PurchaseComponentsSource.Clear();
            RefreshPurchasedComponents();
            _hasUserInteractedWithSupplier = false;
            Add_SelectedSupplier = null;
            _hasUserInteractedWithSupplier = false;
            Add_CanAdd = false;
        }

        [RelayCommand]
        private async void Add_AddPurchase()
        {
            Add_CanAdd = false;
            try
            {
                string supplierName = Add_SelectedSupplier as string;

                Supplier sup = DatabaseStore.SupplierStore.Suppliers.FirstOrDefault(x => x.Name == supplierName);
                if(sup == null)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("There is no such supplier in system!", Icon.Error);
                    return;
                }


                User loggedInUser = DatabaseStore.UsersStore.LoggedInUser;
                if (loggedInUser == null)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("User needs to be logged in to execute this operation", Icon.Error);
                    return;
                }

                double totalPrice = PurchaseComponentsSource.Sum(x=>x.Quantity * x.UnitPrice);

                Purchase purchaseToAdd = new Purchase(iD: 0, userID: loggedInUser.ID, loggedInUser, supplierID: sup.ID, supplier: sup, purchaseDate: Add_SelectedDate, totalPrice: totalPrice);
                IEnumerable<PurchaseItem> purchaseItemToAdd = new List<PurchaseItem>(PurchaseComponentsSource.Select(x => x.PurchaseItem.PurchaseItem).ToArray());
                
                bool result = await DatabaseStore.PurchaseStore.InsertNewPurchase(purchaseToAdd, purchaseItemToAdd);

                if (result == true)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("Purchase added successfully", Icon.Success);
                    Add_Clear();
                    SelectedTab = 0;
                }
                else
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("Purchase couldn't be added", Icon.Error);
                }
            }
            catch (Exception exception)
            {

            }

            Add_CanAdd = true;
        }

        [ObservableProperty]
        [Required]
        private string _add_SelectedSupplier;

        partial void OnAdd_SelectedSupplierChanged(string value)
        {
            _hasUserInteractedWithSupplier = true;
            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedSupplier));
            }
        }

        [ObservableProperty]
        private string _add_TotalPrice = string.Empty;

        [RelayCommand]
        private void Purchase_ClearFiltersForAllComponents()
        {
            Console.WriteLine();
        }

        [RelayCommand]
        private void AddComponentToPurchasedComponent()
        {
            Console.WriteLine();
        }

        [ObservableProperty]
        private string _purchase_AddAllComponentsFilter;
        partial void OnPurchase_AddAllComponentsFilterChanged(string value)
        {
            AllComponents.Refresh();
        }
        
        public List<PurchaseItemComponentContainerHolder> PurchaseComponentsSource { get; set; }
        public List<ComponentWithCategoryContainerHolder> AllComponentsSource { get; set; }
        public DataGridCollectionView PurchaseComponents { get; set; }
        public DataGridCollectionView AllComponents { get; set; }

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
                    DatabaseStore.PurchaseStore.GetPurchaseItemContainerFromPurchase(SelectedPurchase.Container.Purchase);
                    break;
                default:
                    break;
            }
        }

        [ObservableProperty]
        private DetailedPurchaseContainerHolder _selectedPurchase;

        partial void OnSelectedPurchaseChanged(DetailedPurchaseContainerHolder? oldValue, DetailedPurchaseContainerHolder newValue)
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
        public List<DetailedPurchaseContainerHolder> PurchasesSource { get; set; }
        public DataGridCollectionView Purchases { get; set; }
        public PurchasesPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService) : base(defaultRootPageViewModel, databaseStore, messageBoxService)
        {
            PurchasesSource = new List<DetailedPurchaseContainerHolder>();
            ProjectItems = new List<PurchaseItemContainer>();
            SupplierSource = new ObservableCollection<string>();

            Preview_ProjectItems = new DataGridCollectionView(ProjectItems);

            // TODO: https://github.com/AvaloniaUI/Avalonia/discussions/16042 implement this for PurchaseItem Component DataGrid

            Purchases = new DataGridCollectionView(PurchasesSource);
            Purchases.Filter = (object component) =>
            {
                if (component is DetailedPurchaseContainerHolder detailedPurchase)
                {
                    bool priceOk = false;
                    bool dateFrom = true;
                    bool dateTo = true;
                    bool goodSupplier = true;

                    if (detailedPurchase.Container.TotalPrice >= Collection_SelectedLowPrice && detailedPurchase.Container.TotalPrice <= Collection_SelectedHighPrice)
                    {
                        priceOk = true;
                    }

                    if(Collection_SelectedFromDate != null)
                    {
                        if (Collection_SelectedFromDate != DateTimeOffset.MinValue)
                        {
                            if(Collection_SelectedFromDate > detailedPurchase.Container.PurchaseDate)
                            {
                                dateFrom = false;
                            }
                        }
                    }

                    if (Collection_SelectedToDate != null)
                    {
                        if (Collection_SelectedToDate != DateTimeOffset.MinValue)
                        {
                            if (Collection_SelectedToDate < detailedPurchase.Container.PurchaseDate)
                            {
                                dateTo = false;
                            }
                        }
                    }

                    if(Collection_SelectedSupplier != null)
                    {
                        if(detailedPurchase.Container.SupplierName != Collection_SelectedSupplier)
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

            PurchaseComponentsSource = new List<PurchaseItemComponentContainerHolder>();
            PurchaseComponents = new DataGridCollectionView(PurchaseComponentsSource);

            //PurchaseComponents.CollectionChanged += Purchases_CollectionChangedHandler;
            PurchaseComponents.PropertyChanged += PurchaseComponents_PropertyChangedHandler;

            AllComponentsSource = new List<ComponentWithCategoryContainerHolder>();
            AllComponents = new DataGridCollectionView(AllComponentsSource);
            AllComponents.Filter = ((object arg) =>
            {
                if (arg is ComponentWithCategoryContainerHolder component)
                {
                    bool nameOrManufacturer = false;

                    if(Purchase_AddAllComponentsFilter == null)
                    {
                        nameOrManufacturer = true;
                    }
                    else if (component.Component.Name.Contains(Purchase_AddAllComponentsFilter, StringComparison.InvariantCultureIgnoreCase) ||
                       component.Component.Manufacturer.Contains(Purchase_AddAllComponentsFilter, StringComparison.InvariantCultureIgnoreCase))
                    {
                        nameOrManufacturer = true;
                    }

                    if (nameOrManufacturer == true)
                    {
                        return true;
                    }
                }
                return false;
            });

            DatabaseStore.ComponentStore.ComponentsFromSystemLoaded += ComponentStore_ComponentsFromSystemLoadedHandler;
            DatabaseStore.ComponentStore.LoadComponentsOfSystem();

            DatabaseStore.PurchaseStore.PurchaseItemsContainersLoaded += PurchaseStore_PurchaseItemsContainersLoadedHandler;

            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded += PurchaseStore_PurchasesLoadedHandler;
            DatabaseStore.PurchaseStore.LoadDetailedPurchaseContainers();

            DatabaseStore.SupplierStore.SuppliersLoaded += SupplierStore_SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.Load();
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "HasErrors" || e.PropertyName == nameof(Add_SelectedSupplier) || e.PropertyName == nameof(PurchaseComponents))
            {
                bool contains = !(PurchaseComponentsSource.Any(x => x.Quantity == 0) || PurchaseComponentsSource.Any(x => x.UnitPrice == 0));
                bool hasElements = PurchaseComponentsSource.Count != 0;

                bool canAdd = contains && hasElements;

                if (_hasUserInteractedWithSupplier == false)
                {
                    Add_CanAdd = false;
                }
                else
                {
                    if(canAdd == false)
                    {
                        Add_CanAdd = false;
                    }
                    else
                    {
                        Add_CanAdd = !HasErrors;
                    }
                }
            }
        }

        private void PurchaseComponents_PropertyChangedHandler(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine();
            double totalPrice = PurchaseComponentsSource.Sum(x => x.Quantity * x.UnitPrice);
            string price = totalPrice.ToString("C");
            Add_TotalPrice = price;
            OnPropertyChanged(nameof(PurchaseComponents));
        }

        private void Purchases_CollectionChangedHandler(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine();
            double totalPrice = PurchaseComponentsSource.Sum(x => x.Quantity * x.UnitPrice);
            string price = totalPrice.ToString("C");
            Add_TotalPrice = price;
        }

        private void ComponentStore_ComponentsFromSystemLoadedHandler()
        {
            AllComponentsSource.Clear();
            IEnumerable<ComponentWithCategoryContainer> comp = DatabaseStore.ComponentStore.ComponentsFromSystem;
            foreach(ComponentWithCategoryContainer component in comp)
            {
                AllComponentsSource.Add(new ComponentWithCategoryContainerHolder(component, this));
            }
            AllComponents.Refresh();
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
            double minPrice = PurchasesSource.Min(x => x.Container.TotalPrice);
            double maxPrice = PurchasesSource.Max(x => x.Container.TotalPrice);

            Collection_MaxPrice = maxPrice;
            Collection_SelectedHighPrice = maxPrice;
            Collection_MinPrice = minPrice;
            Collection_SelectedLowPrice = minPrice;

            DateTime minDate = PurchasesSource.Min(x => x.Container.PurchaseDate);
            DateTime maxDate = PurchasesSource.Max(x => x.Container.PurchaseDate);

            Collection_DateMaxYear = maxDate;
            Collection_DateMinYear = minDate;
            Collection_SelectedFromDate = minDate;
            Collection_SelectedToDate = maxDate;
        }

        private void PurchaseStore_PurchasesLoadedHandler()
        {
            IEnumerable<DetailedPurchaseContainer> purchasesFromDB = DatabaseStore.PurchaseStore.DetailedPurchaseContainers;
            PurchasesSource.Clear();
            foreach(DetailedPurchaseContainer dPurchase in purchasesFromDB)
            {
                PurchasesSource.Add(new DetailedPurchaseContainerHolder(dPurchase));
            }
            AdjustFilters();
            Purchases.Refresh();
        }

        public void InterpreteNavigationParameter(NavParam navigationParameter)
        {
            switch (navigationParameter.Operation)
            {
                case NavOperation.Add:
                    SelectedTab = 1;
                    break;
                case NavOperation.Preview:
                    break;
                default:
                    break;
            }
        }

        //public override void Dispose()
        //{
        //    DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded -= PurchaseStore_PurchasesLoadedHandler;
        //}
    }
}
