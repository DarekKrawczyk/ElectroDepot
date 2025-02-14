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
using DynamicData;
using System.Reactive.Subjects;
using DynamicData.Binding;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData.Operators;
using System.Globalization;
using HarfBuzzSharp;
using Avalonia.Controls;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using DesktopClient.Utils;
using System.Threading.Tasks;
using DynamicData.PLinq;
using System.Numerics;
using System.Threading;

namespace DesktopClient.ViewModels
{
        public enum SortingType
        {
            Default,
            PriceAsc,
            PriceDesc,
            DateAsc,
            DateDesc,
            ComponentsAsc,
            ComponentsDesc
        }
    public partial class PurchasesPageViewModel : RootNavigatorViewModel, INavParamInterpreter
    {
        #region Tab navigation
        partial void OnSelectedTabChanged(int value)
        {
            Evaluate_AddTabVisibilty();
        }

        public void Evaluate_AddTabVisibilty()
        {
            Evaluate_Add_IsPurchasesTabEnabled();
            Evaluate_Add_IsAddTabEnabled();
            Evaluate_Add_IsImportTabEnabled();
            Evaluate_Add_IsPreviewTabEnabled();
        }

        [ObservableProperty]
        private bool _add_IsPurchasesTabEnabled;

        private void Evaluate_Add_IsPurchasesTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = true;
            }
            else if (SelectedTab == 1)
            {
                isVisible = !Add_CanClear();
            }
            else if (SelectedTab == 2)
            {
                isVisible = true;
            }
            else
            {
                isVisible = true;
            }
            Add_IsPurchasesTabEnabled = isVisible;
        }


        [ObservableProperty]
        private bool _add_IsImportTabEnabled;

        private void Evaluate_Add_IsImportTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = true;
            }
            else if (SelectedTab == 1)
            {
                isVisible = !Add_CanClear();
            }
            else if (SelectedTab == 2)
            {
                isVisible = true;
            }
            else
            {
                isVisible = true;
            }
            Add_IsImportTabEnabled = isVisible;
        }

        [ObservableProperty]
        private bool _add_IsAddTabEnabled;

        private void Evaluate_Add_IsAddTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = true;
            }
            else if (SelectedTab == 1)
            {
                isVisible = true;
            }
            else if (SelectedTab == 2)
            {
                isVisible = true;
            }
            else
            {
                isVisible = true;
            }
            Add_IsAddTabEnabled = isVisible;
        }

        [ObservableProperty]
        private bool _add_IsPreviewTabEnabled;

        private void Evaluate_Add_IsPreviewTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = false; // TODO: Only if selected component!
            }
            else if (SelectedTab == 1)
            {
                isVisible = false;  // Components was not added so why should it be visible?
            }
            else if (SelectedTab == 2)
            {
                isVisible = false;
            }
            else
            {
                isVisible = true;
            }
            Add_IsPreviewTabEnabled = isVisible;
        }

        public async Task NavigateTab(PurchasesTab tab)
        {
            switch (tab)
            {
                case PurchasesTab.Purchases:
                    Purchases_RefreshData();
                    if (SelectedTab == 0) break;    // User is on this Tab so do not change anything.
                    else
                    {
                        if (SelectedTab == 1)
                        {
                            bool wasChanged = Add_CanClear();

                            if (wasChanged == true)
                            {
                                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                                if (result == "Yes")
                                {
                                    // Clear changes and navigate to 'Components' tab.
                                    Add_Clear();
                                    SelectedTab = 0;
                                }
                                else
                                {
                                    // Stay where you are.
                                }
                            }
                            else
                            {
                                SelectedTab = 0;
                            }
                        }
                        else if (SelectedTab == 2)
                        {
                            SelectedTab = 0;
                        }
                        else if (SelectedTab == 3)
                        {
                            Preview_ClearAfterPreview();
                            SelectedTab = 0;
                        }
                    }
                    break;
                case PurchasesTab.Add:
                    SelectedTab = 1;
                    break;
                case PurchasesTab.Preview:
                    //RefreshSelectedComponentsProjectSource();
                    //RefreshSelectedComponentsPurchasesSource();
                    //ChangeToPreviewMode();
                    //PrepareForPreview();
                    SelectedTab = 3;
                    Preview_PrepareForPreview();
                    break;
                case PurchasesTab.Import:
                    //RefreshSelectedComponentsProjectSource();
                    //RefreshSelectedComponentsPurchasesSource();
                    //ChangeToEditMode();
                    //Modify_ClearDataToDefault();
                    if(SelectedTab == 3)
                    {
                        Preview_ClearAfterPreview();
                    }
                    SelectedTab = 2;
                    break;
                default:
                    SelectedTab = 0;
                    break;
            }

        }
        #endregion

        public SortingService SortParameters { get; } = new SortingService();
        #region Purchases
        private readonly PurchaseHolderService _purchasesService;
        private readonly ISubject<PageRequest> _pager;
        private readonly ReadOnlyObservableCollection<DetailedPurchaseContainerHolder> _purchases;
        public ReadOnlyObservableCollection<DetailedPurchaseContainerHolder> PurchasesCollection => _purchases;

        [ObservableProperty]
        private DetailedPurchaseContainerHolder _purchasesCollectionSelectedItem;

        partial void OnPurchasesCollectionSelectedItemChanged(DetailedPurchaseContainerHolder value)
        {
            Preview_PreviewedPurchase = value;
        }

        public int FirstPageIndex = 1;
        public int SelectedPageSize = 10;

        [ObservableProperty]
        private int _totalItems;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearSupplierCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private string _purchases_SelectedSupplier;

        [RelayCommand(CanExecute = nameof(Purchases_CanClearSupplier))]
        public void Purchases_ClearSupplier()
        {
            Purchases_SelectedSupplier = null;
        }

        private bool Purchases_CanClearSupplier()
        {
            return Purchases_SelectedSupplier != null;
        }

        [ObservableProperty]
        private double _purchases_MinPrice = 0;

        partial void OnPurchases_MinPriceChanged(double value)
        {
            //Purchases_SelectedLowPrice = value;
        }

        [ObservableProperty]
        private double _purchases_MaxPrice = 1;

        partial void OnPurchases_MaxPriceChanged(double value)
        {
            //Purchases_SelectedHighPrice = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearPriceFromCommand))]
        private double _purchases_SelectedLowPrice = 0;

        partial void OnPurchases_SelectedLowPriceChanged(double oldValue, double newValue)
        {
            Purchases_PriceFrom = newValue;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearPriceToCommand))]
        private double _purchases_SelectedHighPrice = 1;

        partial void OnPurchases_SelectedHighPriceChanged(double oldValue, double newValue)
        {
            Purchases_PriceTo = newValue;
        }

        [RelayCommand(CanExecute = nameof(Purchases_CanClearPriceFrom))]
        public void Purchases_ClearPriceFrom()
        {
            Purchases_SelectedLowPrice = Purchases_MinPrice;
        }

        private bool Purchases_CanClearPriceFrom()
        {
            return Purchases_SelectedLowPrice != Purchases_MinPrice;
        }

        [RelayCommand(CanExecute = nameof(Purchases_CanClearPriceTo))]
        public void Purchases_ClearPriceTo()
        {
            Purchases_SelectedHighPrice = Purchases_MaxPrice;
        }

        private bool Purchases_CanClearPriceTo()
        {
            return Purchases_SelectedHighPrice != Purchases_MaxPrice;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private double _purchases_PriceFrom;

        partial void OnPurchases_PriceFromChanged(double value)
        {
            Purchases_PriceFromFormated = Purchases_PriceFrom.ToString("C", new CultureInfo("pl-PL"));
        }

        [ObservableProperty]
        private string _purchases_PriceFromFormated;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private double _purchases_PriceTo;

        partial void OnPurchases_PriceToChanged(double value)
        {
            Purchases_PriceToFormated = Purchases_PriceTo.ToString("C", new CultureInfo("pl-PL"));
        }

        [ObservableProperty]
        private string _purchases_PriceToFormated;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearSelectedSortingCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private ComboBoxItem _purchases_SelectedSorting;

        partial void OnPurchases_SelectedSortingChanged(ComboBoxItem value)
        {
            //SortingType type = GetSelectedSortingType(value);
            //SortParameters.ApplySorting(type);
        }

        public bool WasPagingChanged()
        {
            if (CurrentPage != 1 || SelectedPageSizeIndex != 0) return true;
            return false;
        }

        [RelayCommand(CanExecute = nameof(Purchases_WasFromEdited))]
        public void Purchases_ClearForm()
        {
            FirstPage();
            SelectedPageSizeIndex = 0;

            Purchases_SelectedFromDate = Purchases_DateMinYear;
            Purchases_SelectedToDate = Purchases_DateMaxYear;

            Purchases_SelectedSupplier = null;

            Purchases_SelectedLowPrice = Purchases_MinPrice;
            Purchases_SelectedHighPrice = Purchases_MaxPrice;

            Purchases_SelectedSorting = null;
        }

        public bool Purchases_WasFromEdited()
        {
            bool pagingChanged = WasPagingChanged();
            bool dateChanged = Purchases_CanClearFromDate() || Purchases_CanClearToDate();
            bool supplierChangedd = Purchases_CanClearSupplier();
            bool priceChanged = Purchases_CanClearPriceFrom() || Purchases_CanClearPriceTo();
            bool sortingChanged = Purchases_CanClearSelectedSorting();

            bool result = pagingChanged || dateChanged || supplierChangedd || priceChanged || sortingChanged;
            return result;
        }

        private static SortingType GetSelectedSortingType(ComboBoxItem selectedSorting)
        {
            try
            {
                SortingType result = SortingType.Default;
                if (selectedSorting == null) return result;
                string labelContent = ((selectedSorting?.Content as StackPanel).Children[1] as Label).Content as string;
                if (labelContent == null) return result;
                switch (labelContent)
                {
                    case "Price ascending":
                        result = SortingType.PriceAsc;
                        break;
                    case "Price descending":
                        result = SortingType.PriceDesc;
                        break;
                    case "Count ascending":
                        result = SortingType.ComponentsAsc;
                        break;
                    case "Count descending":
                        result = SortingType.ComponentsDesc;
                        break;
                    case "Date ascending":
                        result = SortingType.DateAsc;
                        break;
                    case "Date descending":
                        result = SortingType.DateDesc;
                        break;
                    default:
                        result = SortingType.Default;
                        break;
                }
                return result;
            }
            catch (Exception exception)
            {
                return SortingType.Default;
            }
        }

        [RelayCommand(CanExecute = nameof(Purchases_CanClearSelectedSorting))]
        public void Purchases_ClearSelectedSorting()
        {
            Purchases_SelectedSorting = null;
        }

        private bool Purchases_CanClearSelectedSorting()
        {
            return Purchases_SelectedSorting != null;
        }

        #region Date
        [RelayCommand(CanExecute = nameof(Purchases_CanClearFromDate))]
        public void Purchases_ClearFromDate()
        {
            Purchases_SelectedFromDate = Purchases_DateMinYear;
        }

        private bool Purchases_CanClearFromDate()
        {
            return Purchases_SelectedFromDate != Purchases_DateMinYear;
        }

        [RelayCommand(CanExecute = nameof(Purchases_CanClearToDate))]
        public void Purchases_ClearToDate()
        {
            Purchases_SelectedToDate = Purchases_DateMaxYear;
        }

        private bool Purchases_CanClearToDate()
        {
            return Purchases_SelectedToDate != Purchases_DateMaxYear;
        }

        [ObservableProperty]
        private DateTimeOffset _purchases_DateMinYear;

        [ObservableProperty]
        private DateTimeOffset _purchases_DateMaxYear;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFromDateCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private DateTimeOffset _purchases_SelectedFromDate;

        partial void OnPurchases_SelectedFromDateChanged(DateTimeOffset oldValue, DateTimeOffset newValue)
        {
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearToDateCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private DateTimeOffset _purchases_SelectedToDate;

        partial void OnPurchases_SelectedToDateChanged(DateTimeOffset oldValue, DateTimeOffset newValue)
        {
        }
        #endregion

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private int _selectedPageSizeIndex = 0;

        partial void OnSelectedPageSizeIndexChanged(int value)
        {
            switch (value)
            {
                case 0:
                    SelectedPageSize = 10;
                    break;
                case 1:
                    SelectedPageSize = 25;
                    break;
                case 2:
                    SelectedPageSize = 50;
                    break;
                case 3:
                    SelectedPageSize = 100;
                    break;
                default:
                    SelectedPageSize = 10;
                    break;
            }
            _pager.OnNext(new PageRequest(FirstPageIndex, SelectedPageSize));
        }

        [RelayCommand]
        public void Purchases_RefreshData()
        {
            _purchasesService.LoadData();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Purchases_ClearFormCommand))]
        private int _currentPage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int _totalPages;

        private void PagingUpdate(IPageResponse response)
        {
            TotalItems = response.TotalSize;
            CurrentPage = response.Page;
            TotalPages = response.Pages;
        }

        private static Func<DetailedPurchaseContainerHolder, bool> SupplierFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return trade => true;
            return t => t.Container.Supplier.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<DetailedPurchaseContainerHolder, bool> LowPriceFilter(double lowPrice)
        {
            return x => x.Container.TotalPrice >= lowPrice;
        }

        private static Func<DetailedPurchaseContainerHolder, bool> HighPriceFilter(double highPrice)
        {
            return x => x.Container.TotalPrice <= highPrice;
        }

        private static Func<DetailedPurchaseContainerHolder, bool> FromDateFilter(DateTimeOffset fromDate)
        {
            return x => x.Container.PurchaseDate >= fromDate;
        }

        private static Func<DetailedPurchaseContainerHolder, bool> ToDateFilter(DateTimeOffset toDate)
        {
            return x => x.Container.PurchaseDate <= toDate;
        }



        #region Previous page commands
        [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
        public void PreviousPage()
        {
            _pager.OnNext(new PageRequest(_currentPage - 1, SelectedPageSize));
        }

        private bool CanGoToPreviousPage()
        {
            return CurrentPage > FirstPageIndex;
        }
        #endregion
        #region Next page commands
        [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
        public void NextPage()
        {
            _pager.OnNext(new PageRequest(_currentPage + 1, SelectedPageSize));

        }

        private bool CanGoToNextPage()
        {
            return CurrentPage < TotalPages;
        }
        #endregion
        #region First page commands
        [RelayCommand(CanExecute = nameof(CanGoToFirstPage))]
        public void FirstPage()
        {
            _pager.OnNext(new PageRequest(FirstPageIndex, SelectedPageSize));
        }

        private bool CanGoToFirstPage()
        {
            return CurrentPage > FirstPageIndex;
        }
        #endregion
        #region Last page commands
        [RelayCommand(CanExecute = nameof(CanGoToLastPage))]
        public void LastPage()
        {
            _pager.OnNext(new PageRequest(_totalPages, SelectedPageSize));
        }

        private bool CanGoToLastPage()
        {
            return CurrentPage < TotalPages;
        }
        #endregion
        #endregion
        #region Add
        [RelayCommand]
        public async Task Add_Cancel()
        {
            await NavigateTab(PurchasesTab.Purchases);
        }

        public void CartComponentsSizeChanged()
        {
            Add_AddPurchaseCommand.NotifyCanExecuteChanged();
            Add_ClearCommand.NotifyCanExecuteChanged();
            Evaluate_AddTabVisibilty();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearCartSelectedCategoryFilterCommand))]
        private string _add_CartSelectedCategory;

        partial void OnAdd_CartSelectedCategoryChanged(string value)
        {
            PurchaseComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearCartSelectedCategoryFilter))]
        public void Add_ClearCartSelectedCategoryFilter()
        {
            Add_CartSelectedCategory = null;
        }

        private bool Add_CanClearCartSelectedCategoryFilter()
        {
            return Add_CartSelectedCategory != null;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearCartNameFilterCommand))]
        private string _add_CartNameFilter;

        partial void OnAdd_CartNameFilterChanged(string value)
        {
            PurchaseComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearCartNameFilter))]
        public void Add_ClearCartNameFilter()
        {
            Add_CartNameFilter = null;
        }

        private bool Add_CanClearCartNameFilter()
        {
            return Add_CartNameFilter != null;
        }

        [RelayCommand(CanExecute = nameof(Add_CanCopyPriceToClipboard))]
        public async Task Add_CopyPriceToClipboard()
        {
            await ClipboardManager.SetText(Add_TotalPrice);
        }

        private bool Add_CanCopyPriceToClipboard()
        {
            return Add_TotalPrice != null && Add_TotalPrice != string.Empty;
        }

        private SupplierContainer _defaultSupplier;

        [ObservableProperty]
        private Bitmap _add_SelectedSupplierLogo;

        [RelayCommand(CanExecute = nameof(Add_CanClearSelectedSupplier))]
        public void Add_ClearSelectedSupplier()
        {
            if (_defaultSupplier != null)
            {
                Add_SelectedSupplier = _defaultSupplier.Supplier.Name;
                Add_SelectedSupplierLogo = _defaultSupplier.Supplier.Image;
            }
            else
            {
                Add_SelectedSupplier = null;
                Add_SelectedSupplierLogo = null;
            }
        }

        private bool Add_CanClearSelectedSupplier()
        {
            if (_defaultSupplier != null)
            {
                return Add_SelectedSupplier != _defaultSupplier.Supplier.Name;
            }
            return false;
        }

        public ObservableCollection<SupplierContainer> Suppliers { get; set; }
        public void Add_ReevaluateTotalPrice()
        {
            double totalPrice = PurchaseComponentsSource.Sum(x => x.Quantity * x.UnitPrice);
            string price = totalPrice.ToString("C");
            Add_TotalPrice = price;
        }

        public ObservableCollection<string> Categories { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearAllComponentsCategoryFilterCommand))]
        private string _add_AllComponentsCategoryFilter;

        partial void OnAdd_AllComponentsCategoryFilterChanged(string value)
        {
            AllComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearAllComponentsCategoryFilter))]
        public void Add_ClearAllComponentsCategoryFilter()
        {
            Add_AllComponentsCategoryFilter = null;
        }

        private bool Add_CanClearAllComponentsCategoryFilter()
        {
            return Add_AllComponentsCategoryFilter != null;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearAllComponentsNameFilterCommand))]
        private string _add_AllComponentsNameFilter;
        partial void OnAdd_AllComponentsNameFilterChanged(string value)
        {
            AllComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearAllComponentsNameFilter))]
        public void Add_ClearAllComponentsNameFilter()
        {
            Add_AllComponentsNameFilter = null;
        }

        private bool Add_CanClearAllComponentsNameFilter()
        {
            return Add_AllComponentsNameFilter != null;
        }
        #endregion
        #region Preview
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearCartSelectedCategoryFilterCommand))]
        private string _preview_CartSelectedCategory;

        [RelayCommand(CanExecute = nameof(Preview_CanClearCartSelectedCategoryFilter))]
        public void Preview_ClearCartSelectedCategoryFilter()
        {
            Preview_CartSelectedCategory = null;
        }

        private bool Preview_CanClearCartSelectedCategoryFilter()
        {
            return Preview_CartSelectedCategory != null;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearCartNameFilterCommand))]
        private string _preview_CartNameFilter;

        partial void OnPreview_CartNameFilterChanged(string value)
        {
            if(value == null || value == string.Empty)
            {
                Preview_ClearCartNameFilterCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearCartNameFilter))]
        public void Preview_ClearCartNameFilter()
        {
            Preview_CartNameFilter = null;
        }

        private bool Preview_CanClearCartNameFilter()
        {
            return Preview_CartNameFilter != null;
        }

        [RelayCommand]
        public void Preview_CopySupplierToClipboard()
        {
            ClipboardManager.SetText(Preview_PreviewedPurchase.Container.SupplierName);
        }

        [RelayCommand]
        public void Preview_CopyDateToClipboard()
        {
            ClipboardManager.SetText(Preview_PreviewedPurchase.Container.PurchaseDateAsDateShort);
        }

        [RelayCommand]
        public void Preview_CopyPriceToClipboard()
        {
            ClipboardManager.SetText(Preview_PreviewedPurchase.Container.TotalPriceAsCurrency);
        }

        public void Preview_PrepareForPreview()
        {
            _previewComponentsHolderService.LoadData(Preview_PreviewedPurchase.Container.Purchase);
        }

        public void Preview_ClearAfterPreview()
        {
            _previewComponentsHolderService.ClearData();
        }

        public int Preview_FirstPageIndex = 1;
        public int Preview_SelectedPageSize = 10;

        [ObservableProperty]
        private int _preview_totalItems;

        private readonly PurchaseComponentsHolderService _previewComponentsHolderService;
        private readonly ISubject<PageRequest> _preview_pager;
        private readonly ReadOnlyObservableCollection<PurchaseItemComponentContainerHolder> _preview_components;
        public ReadOnlyObservableCollection<PurchaseItemComponentContainerHolder> Preivew_Components => _preview_components;

        [ObservableProperty]
        private DetailedPurchaseContainerHolder _preview_PreviewedPurchase;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_PreviousPageCommand))]
        private int _preview_selectedPageSizeIndex = 0;

        partial void OnPreview_selectedPageSizeIndexChanged(int value)
        {
            switch (value)
            {
                case 0:
                    Preview_SelectedPageSize = 10;
                    break;
                case 1:
                    Preview_SelectedPageSize = 25;
                    break;
                case 2:
                    Preview_SelectedPageSize = 50;
                    break;
                case 3:
                    Preview_SelectedPageSize = 100;
                    break;
                default:
                    Preview_SelectedPageSize = 10;
                    break;
            }
            _preview_pager.OnNext(new PageRequest(Preview_FirstPageIndex, Preview_SelectedPageSize));
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_PreviousPageCommand))]
        private int _preview_currentPage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_PreviousPageCommand))]
        private int _preview_totalPages;

        private void Preview_PagingUpdate(IPageResponse response)
        {
            Preview_totalItems = response.TotalSize;
            Preview_currentPage = response.Page;
            Preview_totalPages = response.Pages;
        }

        #region Preview Previous page commands
        [RelayCommand(CanExecute = nameof(Preview_CanGoToPreviousPage))]
        public void Preview_PreviousPage()
        {
            _preview_pager.OnNext(new PageRequest(_preview_currentPage - 1, Preview_SelectedPageSize));
        }

        private bool Preview_CanGoToPreviousPage()
        {
            return Preview_currentPage > Preview_FirstPageIndex;
        }
        #endregion
        #region Preview Next page commands
        [RelayCommand(CanExecute = nameof(Preview_CanGoToNextPage))]
        public void Preview_NextPage()
        {
            _pager.OnNext(new PageRequest(_preview_currentPage + 1, Preview_SelectedPageSize));

        }

        private bool Preview_CanGoToNextPage()
        {
            return Preview_currentPage < Preview_totalPages;
        }
        #endregion
        #region Preview First page commands
        [RelayCommand(CanExecute = nameof(Preview_CanGoToFirstPage))]
        public void Preview_FirstPage()
        {
            _preview_pager.OnNext(new PageRequest(Preview_FirstPageIndex, Preview_SelectedPageSize));
        }

        private bool Preview_CanGoToFirstPage()
        {
            return Preview_currentPage > Preview_FirstPageIndex;
        }
        #endregion
        #region Last page commands
        [RelayCommand(CanExecute = nameof(Preview_CanGoToLastPage))]
        public void Preview_LastPage()
        {
            _preview_pager.OnNext(new PageRequest(_preview_totalPages, Preview_SelectedPageSize));
        }

        private bool Preview_CanGoToLastPage()
        {
            return Preview_currentPage < Preview_totalPages;
        }
        #endregion




        #endregion
        [RelayCommand]
        public void Collection_Refresh()
        {
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
        [NotifyCanExecuteChangedFor(nameof(Add_ClearSelectedDateCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearCommand))]
        private DateTime _add_SelectedDate = DateTime.Now;

        [RelayCommand(CanExecute = nameof(Add_CanClearSelectedDate))]
        public void Add_ClearSelectedDate()
        {
            Add_SelectedDate = DateTime.Now;
        }

        private bool Add_CanClearSelectedDate()
        {
            return Add_SelectedDate.Date != DateTime.Now.Date;
        }

        public void Add_ClearTotalPrice()
        {
            Add_TotalPrice = 0.ToString("C2");
        }

        [RelayCommand(CanExecute = nameof(Add_CanClear))]
        private void Add_Clear()
        {
            // Disable flag
            AllComponentsSource.ForEach(x => x.CanAdd = true);

            PurchaseComponentsSource.Clear();
            RefreshPurchasedComponents();
            Add_ClearTotalPrice();
            _hasUserInteractedWithSupplier = false;
            Add_SelectedSupplier = _defaultSupplier.Supplier.Name;
            _hasUserInteractedWithSupplier = false;
            Add_SelectedDate = DateTime.Now;
            Add_CanAdd = false;

            Add_CartNameFilter = null;
            Add_CartSelectedCategory = null;
        }

        private bool Add_CanClear()
        {
            bool hasComponents = PurchaseComponentsSource.Count > 0;
            bool dateChanged = Add_SelectedDate.Date != DateTime.Now.Date;
            bool supplierChanged = false;

            if (_defaultSupplier != null)
            {
                supplierChanged = Add_SelectedSupplier != _defaultSupplier.Supplier.Name;
            }

            bool result = hasComponents || dateChanged || supplierChanged;
            return result;
        }

        private bool Add_CanAddPurchase()
        {
            bool hasComponents = PurchaseComponentsSource.Count > 0;
            bool elementsCorrect = !(PurchaseComponentsSource.Any(x => x.Quantity == 0) || PurchaseComponentsSource.Any(x => x.UnitPrice == 0));

            bool result = hasComponents && elementsCorrect;

            return result;
        }

        [RelayCommand(CanExecute = nameof(Add_CanAddPurchase))]
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

                foreach(PurchaseItem item in purchaseItemToAdd)
                {
                    OwnsComponent ownsComponent = new OwnsComponent(id: 0, userID: DatabaseStore.UsersStore.LoggedInUser.ID, componentID: item.ComponentID, quantity: item.Quantity);
                    OwnsComponent ownsComponentFromDB = await DatabaseStore.ComponentStore.OwnsComponentDP.CreateOwnComponent(ownsComponent);
                }

                if (result == true)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("Purchase added successfully! Do you want to add another purchase?", Icon.Success);
                    Add_Clear();
                    
                    if (buttonResult == "No")
                    {
                        NavigateTab(PurchasesTab.Purchases);
                    }
                }
                else
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("There was an error while adding this purchase. Try again or contact administrator!", Icon.Error);
                }
            }
            catch (Exception exception)
            {

            }

            Add_CanAdd = true;
        }

        [ObservableProperty]
        [Required]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearSelectedSupplierCommand))]
        private string _add_SelectedSupplier;

        partial void OnAdd_SelectedSupplierChanged(string value)
        {
            _hasUserInteractedWithSupplier = true;
            if(value != null)
            {
                Add_SelectedSupplierLogo = Suppliers.FirstOrDefault(x => x.Supplier.Name == value).Supplier.Image;
            }
            else
            {
                //TODO:
            }


            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedSupplier));
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_CopyPriceToClipboardCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_AddPurchaseCommand))]
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

        public List<PurchaseItemComponentContainerHolder> PurchaseComponentsSource { get; set; }
        public List<ComponentWithCategoryContainerHolder> AllComponentsSource { get; set; }
        public DataGridCollectionView PurchaseComponents { get; set; }
        public DataGridCollectionView AllComponents { get; set; }

        [ObservableProperty]
        private bool _previewEnabled;

        [ObservableProperty]
        private int _selectedTab;

        //partial void OnSelectedTabChanged(int oldValue, int newValue)
        //{
        //    switch (newValue)
        //    {
        //        case 0:
        //            // Collection
        //            break;
        //        case 1:
        //            // Add
        //            break;
        //        case 2:
        //            // Import
        //            break;
        //        case 3:
        //            // Preview;
        //            DatabaseStore.PurchaseStore.GetPurchaseItemContainerFromPurchase(SelectedPurchase.Container.Purchase);
        //            break;
        //        default:
        //            break;
        //    }
        //}

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
        private string _collection_SelectedSupplier;

        partial void OnCollection_SelectedSupplierChanged(string? oldValue, string newValue)
        {
        }

        [RelayCommand]
        private void Collection_ClearSettings()
        {
            Purchases_SelectedFromDate = Purchases_DateMinYear;
            Purchases_SelectedToDate = Purchases_DateMaxYear;

            Purchases_SelectedHighPrice = Purchases_MaxPrice;
            Purchases_SelectedLowPrice = Purchases_MinPrice;

            Collection_SelectedSupplier = null;
        }

        [ObservableProperty]
        private double _collection_PriceFrom;
        
        [ObservableProperty]
        private double _collection_PriceTo;

        private static IComparer<DetailedPurchaseContainerHolder> SortData(ComboBoxItem sortOption)
        {
            SortingType st = GetSelectedSortingType(sortOption);
            return st switch
            {
                SortingType.PriceAsc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Ascending(e => e.Container.TotalPrice),
                SortingType.PriceDesc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Descending(e => e.Container.TotalPrice),
                SortingType.DateAsc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Ascending(e => e.Container.PurchaseDate),
                SortingType.DateDesc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Descending(e => e.Container.PurchaseDate),
                SortingType.ComponentsAsc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Ascending(e => e.Container.ComponentsQuantity),
                SortingType.ComponentsDesc => SortExpressionComparer<DetailedPurchaseContainerHolder>.Descending(e => e.Container.ComponentsQuantity),
                _ => SortExpressionComparer<DetailedPurchaseContainerHolder>.Ascending(e => e.Container.ID), // Default sorting
            };
        }

        public List<PurchaseItemContainer> ProjectItems { get; set; }
        public DataGridCollectionView Preview_ProjectItems { get; set; }

        private static Func<PurchaseItemComponentContainerHolder, bool> NameFilter(string name)
        {
            if (string.IsNullOrEmpty(name)) return trade => true;
            return t => t.PurchaseItem.Component.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<PurchaseItemComponentContainerHolder, bool> CategoryFilter(string category)
        {
            if (string.IsNullOrEmpty(category)) return trade => true;
            return t => t.CategoryName.Contains(category, StringComparison.InvariantCultureIgnoreCase);
        }
        private NavParam navParam;
        public ObservableCollection<string> SupplierSource { get; set; }
        public PurchasesPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, ApplicationConfig appConfig) : base(defaultRootPageViewModel, databaseStore, messageBoxService, appConfig)
        {
            _purchasesService = new PurchaseHolderService(this, DatabaseStore.PurchaseStore);
            _previewComponentsHolderService = new PurchaseComponentsHolderService(this, DatabaseStore.PurchaseStore);

            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPageIndex, SelectedPageSize));
            _preview_pager = new BehaviorSubject<PageRequest>(new PageRequest(Preview_FirstPageIndex, Preview_SelectedPageSize));

            var supplierFilter = this.WhenValueChanged(t => t.Purchases_SelectedSupplier)
                //.Throttle(TimeSpan.FromMilliseconds(250))
                .Select(SupplierFilter);

            var lowPriceFilter = this.WhenValueChanged(t => t.Purchases_SelectedLowPrice)
                .Select(LowPriceFilter);

            var highPriceFilter = this.WhenValueChanged(t => t.Purchases_SelectedHighPrice)
                .Select(HighPriceFilter);

            var fromDateFilter = this.WhenValueChanged(t => t.Purchases_SelectedFromDate)
                .Select(FromDateFilter);

            var toDateFilter = this.WhenValueChanged(t => t.Purchases_SelectedToDate)
                .Select(ToDateFilter);

            var sort = SortParameters.WhenValueChanged(t => t.SelectedItem)
                .Select(prop => prop.Comparer);

            //var sortExpression = this.WhenValueChanged(t => t.Purchases_SelectedSorting)
            //    .Select(SortData);

            //var availableFilter = this.WhenValueChanged(t => t.OnlyAvailableFlag)
            //    .Select(AvailableFilterPredicate);

            //var manufacturerFilter = this.WhenValueChanged(t => t.SelectedManufacturer)
            //    .Select(ManufacturerFilterPredicate);

            //var categoryFilter = this.WhenValueChanged(t => t.SelectedCategory)
            //    .Select(CategoryFilterPredicate);

            _purchasesService.EmployeesConnection()
                .Filter(supplierFilter)
                .Filter(lowPriceFilter)
                .Filter(highPriceFilter)
                .Filter(fromDateFilter)
                .Filter(toDateFilter)
                //.Filter(categoryFilter)
                .Sort(SortExpressionComparer<DetailedPurchaseContainerHolder>.Descending(e => e.Container.PurchaseDate))
                //.Sort(sort, SortOptimisations.ComparesImmutableValuesOnly)
                //.Sort(sortExpression)
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .ObserveOn(Scheduler.CurrentThread) // Marshals to the current thread (often used for UI updates)
                .Bind(out _purchases)
                .Subscribe();

            _purchasesService.DataLoaded += PurchasesService_DataLoadedHandler;

            _purchasesService.LoadData();

            var preview_namefilter = this.WhenValueChanged(t => t.Preview_CartNameFilter)
                .Select(NameFilter);

            var preview_categoryfilter = this.WhenValueChanged(t => t.Preview_CartSelectedCategory)
                .Select(CategoryFilter);

            _previewComponentsHolderService.EmployeesConnection()
                .Filter(preview_namefilter)
                .Filter(preview_categoryfilter)
                .Sort(SortExpressionComparer<PurchaseItemComponentContainerHolder>.Descending(e => e.PurchaseItem.Name))
                //.Sort(sort, SortOptimisations.ComparesImmutableValuesOnly)
                .Page(_preview_pager)
                .Do(change => Preview_PagingUpdate(change.Response))
                .ObserveOn(Scheduler.CurrentThread) // Marshals to the current thread (often used for UI updates)
                .Bind(out _preview_components)
                .Subscribe();

            _previewComponentsHolderService.DataLoaded += _previewComponentsHolderService_DataLoadedHandler;

            ProjectItems = new List<PurchaseItemContainer>();
            SupplierSource = new ObservableCollection<string>();

            Preview_ProjectItems = new DataGridCollectionView(ProjectItems);

            // TODO: https://github.com/AvaloniaUI/Avalonia/discussions/16042 implement this for PurchaseItem Component DataGrid

            PurchaseComponentsSource = new List<PurchaseItemComponentContainerHolder>();
            PurchaseComponents = new DataGridCollectionView(PurchaseComponentsSource);
            PurchaseComponents.Filter = ((object arg) =>
            {
                if (arg is PurchaseItemComponentContainerHolder purchase)
                {
                    bool nameOrManufacturer = false;
                    bool category = false;

                    // If == null then skip this one
                    if (Add_CartNameFilter == null)
                    {
                        nameOrManufacturer = true;

                    }
                    else
                    {
                        // If diff then null so we have to compare
                        if (purchase.PurchaseItem.Component.Name.Contains(Add_CartNameFilter, StringComparison.InvariantCultureIgnoreCase) ||
                            purchase.PurchaseItem.Component.Manufacturer.Contains(Add_CartNameFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            nameOrManufacturer = true;
                        }
                    }

                    if (Add_CartSelectedCategory == null)
                    {
                        category = true;
                    }
                    else
                    {
                        if (purchase.PurchaseItem.CategoryName.Contains(Add_CartSelectedCategory, StringComparison.InvariantCultureIgnoreCase))
                        {
                            category = true;
                        }
                    }

                    return category && nameOrManufacturer;
                }
                return false;
            });

            Suppliers = new ObservableCollection<SupplierContainer>();

            Categories = new ObservableCollection<string>();
            DatabaseStore.CategorieStore.CategoriesLoaded += CategorieStore_CategoriesLoadedHandler;
            DatabaseStore.CategorieStore.ReloadCategoriesData();

            AllComponentsSource = new List<ComponentWithCategoryContainerHolder>();
            AllComponents = new DataGridCollectionView(AllComponentsSource);
            AllComponents.Filter = ((object arg) =>
            {
                if (arg is ComponentWithCategoryContainerHolder component)
                {
                    bool nameOrManufacturer = false;
                    bool category = false;

                    // If == null then skip this one
                    if (Add_AllComponentsNameFilter == null)
                    {
                        nameOrManufacturer = true;

                    }
                    else
                    {
                        // If diff then null so we have to compare
                        if (component.Component.Name.Contains(Add_AllComponentsNameFilter, StringComparison.InvariantCultureIgnoreCase) ||
                            component.Component.Manufacturer.Contains(Add_AllComponentsNameFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            nameOrManufacturer = true;
                        }
                    }

                    if (Add_AllComponentsCategoryFilter == null)
                    {
                        category = true;
                    }
                    else
                    {
                        if(component.Component.Category.Name.Contains(Add_AllComponentsCategoryFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            category = true;
                        }
                    }

                    return category && nameOrManufacturer;
                }
                return false;
            });

            DatabaseStore.ComponentStore.ComponentsFromSystemLoaded += ComponentStore_ComponentsFromSystemLoadedHandler;
            DatabaseStore.ComponentStore.LoadComponentsOfSystem();

            DatabaseStore.PurchaseStore.PurchaseItemsContainersLoaded += PurchaseStore_PurchaseItemsContainersLoadedHandler;

            DatabaseStore.SupplierStore.SuppliersLoaded += SupplierStore_SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.SuppliersReloadNotNecessary += SupplierStore_SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.ReloadSuppliersData();

            Add_ClearTotalPrice();

            Evaluate_AddTabVisibilty();
        }

        private async void _previewComponentsHolderService_DataLoadedHandler()
        {
            //throw new NotImplementedException();
        }

        private async void CategorieStore_CategoriesLoadedHandler()
        {
            Categories.Clear();
            Categories.AddRange(DatabaseStore.CategorieStore.Categories.Select(x => x.Name));
        }

        private async void PurchasesService_DataLoadedHandler()
        {
            Purchases_MaxPrice = _purchasesService.MaxPrice();
            Purchases_SelectedHighPrice = Purchases_MaxPrice;

            if(_purchasesService.MaxPrice() == _purchasesService.MinPrice())
            {
                // Invalid Arrange rectangle exception workaround
                Purchases_MinPrice = _purchasesService.MinPrice() - 1;
                Purchases_SelectedLowPrice = Purchases_MinPrice;
            }
            else
            {
                Purchases_MinPrice = _purchasesService.MinPrice();
                Purchases_SelectedLowPrice = Purchases_MinPrice;
            }

            Purchases_DateMaxYear = _purchasesService.MaxYear();
            Purchases_DateMinYear = _purchasesService.MinYear();
            Purchases_SelectedFromDate = Purchases_DateMinYear;
            Purchases_SelectedToDate = Purchases_DateMaxYear;
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
            Evaluate_AddTabVisibilty();
        }

        private async void ComponentStore_ComponentsFromSystemLoadedHandler()
        {
            AllComponentsSource.Clear();
            IEnumerable<ComponentWithCategoryContainer> comp = DatabaseStore.ComponentStore.ComponentsFromSystem;
            foreach(ComponentWithCategoryContainer component in comp)
            {
                AllComponentsSource.Add(new ComponentWithCategoryContainerHolder(component, this));
            }
            AllComponents.Refresh();
        }

        private async void PurchaseStore_PurchaseItemsContainersLoadedHandler()
        {
            ProjectItems.Clear();
            ProjectItems.AddRange(DatabaseStore.PurchaseStore.PurchaseItemsContainers);
            Preview_ProjectItems.Refresh();

        }

        private async void SupplierStore_SuppliersLoadedHandler()
        {
            SupplierSource.Clear();
            IEnumerable<Supplier> suppliersFromDB = DatabaseStore.SupplierStore.Suppliers;
            foreach (Supplier supplier in suppliersFromDB)
            {
                SupplierSource.Add(supplier.Name);
            }


            Suppliers.Clear();
            foreach (Supplier supplier in DatabaseStore.SupplierStore.Suppliers)
            {
                Suppliers.Add(new SupplierContainer(supplier));
            }

            // TODO: For now this will do but in future there has to be entry in db like 'Other' for default supplier...
            if(suppliersFromDB.Count() != 0)
            {
                _defaultSupplier = new SupplierContainer(suppliersFromDB.FirstOrDefault());
                Add_SelectedSupplier = _defaultSupplier.Supplier.Name;
            }
        }

        private void NavigationGoToPreviewHandler()
        {
            try
            {
                PurchasesCollectionSelectedItem = _purchasesService.FindItem((navParam.Payload as Purchase));
            }
            catch(Exception exception)
            {

            }
            _purchasesService.DataLoaded -= NavigationGoToPreviewHandler;
            NavigateTab(PurchasesTab.Preview);
        }

        public void InterpreteNavigationParameter(NavParam navigationParameter)
        {
            switch (navigationParameter.Operation)
            {
                case NavOperation.Add:
                    NavigateTab(PurchasesTab.Add);
                    break;
                case NavOperation.Preview:
                    navParam = navigationParameter;
                    _purchasesService.DataLoaded += NavigationGoToPreviewHandler;
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
