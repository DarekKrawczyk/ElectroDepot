using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Containers;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System.Threading;
using ElectroDepotClassLibrary.Utility;
using System.ComponentModel.DataAnnotations;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Diagnostics;
using Avalonia.Input;
using Avalonia;
using DesktopClient.Utils;
using DesktopClient.Navigation;

namespace DesktopClient.ViewModels
{
    public partial class ComponentsPageViewModel : ViewModelBase
    {
        #region Data source
        public List<DetailedComponentContainer> ComponentsSource { get; set; }
        public List<DetailedItemPurchaseContainer> SelectedComponentsPurchasesSource { get; set; }
        public List<DetailedItemProjectContainer> SelectedComponentsProjectSource { get; set; }
        #endregion

        [ObservableProperty]
        private bool _isSelectingPredefinedImagePopupOpen;

        [RelayCommand]
        private void OpenPredefinedImages()
        {
            IsSelectingPredefinedImagePopupOpen = true;
        }

        [ObservableProperty]
        private ImageContainer _selectedPredefinedImage;

        [ObservableProperty]
        private Bitmap _currentAddPredefinedImage;

        [ObservableProperty]
        private bool _preview_CanPreview;

        [RelayCommand]
        private void SelectPredefinedImages()
        {
            // default avares://DesktopClient/Assets/DefaultComponentImage.png
            if (SelectedPredefinedImage == null)
            {
                // No item so do nothing
            }
            else
            {
                CurrentAddPredefinedImage = SelectedPredefinedImage.Image;
                IsSelectingPredefinedImagePopupOpen = false;
            }
        }

        [RelayCommand]
        private async void LoadImageFromDevice()
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            await using var readStream = await file.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await readStream.CopyToAsync(memoryStream);

            byte[] imageData = memoryStream.ToArray();
            Bitmap imageAsBitmap = ImageConverterUtility.BytesToBitmap(imageData);
            CurrentAddPredefinedImage = imageAsBitmap;
        }

        private async Task<IStorageFile?> DoOpenFilePickerAsync()
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            FilePickerOpenOptions options = new FilePickerOpenOptions()
            {
                Title = "Load image",
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new FilePickerFileType("PNG Files") { Patterns = new[] { "*.png" } },
                    new FilePickerFileType("JPEG Files") { Patterns = new[] { "*.jpg", "*.jpeg" } }
                }
            };

            var files = await provider.OpenFilePickerAsync(options);

            return files?.Count >= 1 ? files[0] : null;
        }

        [RelayCommand]
        private void ClosePredefinedImages()
        {
            IsSelectingPredefinedImagePopupOpen = false;
        }

        [ObservableProperty]
        private string _myText = "XDXDXD";

        #region Observable for data source
        public ObservableCollection<string> Manufacturers { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<SupplierContainer> Suppliers { get; set; }
        public ObservableCollection<ImageContainer> PredefinedImages { get; set; }

        public DataGridCollectionView Components { get; set; }
        public DataGridCollectionView PurchasesForSelected { get; set; }
        public DataGridCollectionView ProjectsForSelected{ get; set; }
        #endregion
        #region Observable properties
        [ObservableProperty]
        private bool _previewEnabled = false;

        [ObservableProperty]
        private DetailedComponentContainer _selectedComponent;

        [ObservableProperty]
        private string _searchByNameOrDesc = string.Empty;

        [ObservableProperty]
        private bool _onlyAvailableFlag;

        [ObservableProperty]
        private string _selectedManufacturer;

        [ObservableProperty]
        private string _selectedCategory;

        private bool _hasUserInteractedWithName;
        private bool _hasUserInteractedWithManufacturer;
        private bool _hasUserInteractedWithDescription;
        private bool _hasUserInteractedWithCategory;

        [ObservableProperty]
        private int _selectedTab;
        #region Add component tab
        [ObservableProperty]
        private bool _add_CanAdd = false;

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.PropertyName == "HasErrors" || e.PropertyName == nameof(Add_ShortDescription) || e.PropertyName == nameof(Add_Category) ||
               e.PropertyName == nameof(Add_ComponentName) || e.PropertyName == nameof(Add_Manufacturer))
            {
                if (_hasUserInteractedWithCategory == false ||
                    _hasUserInteractedWithDescription == false ||
                    _hasUserInteractedWithManufacturer == false ||
                    _hasUserInteractedWithName == false)
                {
                    Add_CanAdd = false;
                }
                else
                {
                    Add_CanAdd = !HasErrors;
                }
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Name field cannot be empty")]
        private string _add_ComponentName = string.Empty;

        partial void OnAdd_ComponentNameChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithName = true;
            if(newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_ComponentName));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Manufacturer fiend cannot be empty")]
        private string _add_Manufacturer;

        partial void OnAdd_ManufacturerChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithManufacturer = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_Manufacturer));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Description cannot cannot be empty")]
        private string _add_ShortDescription;

        partial void OnAdd_ShortDescriptionChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithDescription = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_ShortDescription));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "You have to select category")]
        private object _add_Category;

        partial void OnAdd_CategoryChanged(object? oldValue, object newValue)
        {
            _hasUserInteractedWithCategory = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_Category));
            }
        }

        [ObservableProperty]
        private string _add_FullDescription;

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [CustomValidation(typeof(ComponentsPageViewModel), nameof(ValidateDatasheetLink), ErrorMessage = "Link is invalid")]
        private string _add_DatasheetLink;

        partial void OnAdd_DatasheetLinkChanged(string? oldValue, string newValue)
        {
            ValidateLinkAsync();
        }
        private CancellationTokenSource _cts;
        private async void ValidateLinkAsync()
        {
            // Cancel any ongoing validation
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                // Debounce: Wait for a short period before starting validation
                await Task.Delay(300, token);

                // Perform asynchronous validation
                ValidationResult validationResult = await LinkValidator.ValidateDatasheetLinkAsync(Add_DatasheetLink, new ValidationContext(this));
                string errorMessage = validationResult != null ? validationResult.ErrorMessage : string.Empty;
                ValidateProperty(errorMessage, nameof(Add_DatasheetLink));
            }
            catch (TaskCanceledException)
            {
                // Ignore cancellations caused by rapid input changes
            }
        }

        [RelayCommand]
        private void GoToPreview()
        {
            SelectedTab = 2;
        }

        [RelayCommand]
        private void GoToAddNew()
        {
            SelectedTab = 1;
        }

        public static ValidationResult ValidateDatasheetLink(string name, ValidationContext context)
        {
            if(name == "")
            {
                return ValidationResult.Success;
            }
            else
            {
                return new(name);
            }
        }

        [RelayCommand]
        private void Add_ClearImage()
        {
            CurrentAddPredefinedImage = PredefinedImages[0].Image;
        }

        [RelayCommand]
        private void Add_ImageFromPredefined()
        {
            Console.WriteLine("XD");
        }

        [RelayCommand]
        private void Add_UploadImage()
        {
            Console.WriteLine("UploadImage");
        }

        [RelayCommand]
        private void Add_ClearComponent()
        {
            _hasUserInteractedWithCategory = false;
            _hasUserInteractedWithDescription = false;
            _hasUserInteractedWithManufacturer = false;
            _hasUserInteractedWithName = false;
            CurrentAddPredefinedImage = PredefinedImages[0].Image;
            Add_ComponentName = null;
            Add_Manufacturer = null;
            Add_Category = null;
            Add_FullDescription = null;
            Add_ShortDescription = null;
            Add_DatasheetLink = null;
            _hasUserInteractedWithCategory = false;
            _hasUserInteractedWithDescription = false;
            _hasUserInteractedWithManufacturer = false;
            _hasUserInteractedWithName = false;
            Add_CanAdd = false;
        }

        [RelayCommand]
        private async void Add_AddComponent()
        {
            if (DatabaseStore.ComponentStore.Components.FirstOrDefault(x => x.Name == Add_ComponentName) != null)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "Component with that name already exists!", ButtonEnum.YesNo);
                ButtonResult buttonResult = await box.ShowAsync();
                return;
            }
            // TODO: implement
            Add_CanAdd = false;

            try
            {
                string categoryName = Add_Category as string;

                Category cat = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x=>x.Name == categoryName);

                string datasheet = Add_DatasheetLink == null ? string.Empty : Add_DatasheetLink;

                Component newComponent = new Component(id: 0, cat.ID, category: cat, name: Add_ComponentName, manufacturer: Add_Manufacturer, shortDescription: Add_ShortDescription,
                    longDescription: Add_FullDescription, datasheetLink: datasheet, byteImage: ImageConverterUtility.BitmapToBytes(CurrentAddPredefinedImage));

                bool result = await DatabaseStore.ComponentStore.InsertNewComponent(newComponent);

                if(result == true)
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "Component added successfully", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                    Add_ClearComponent();
                    SelectedTab = 0;
                }
                else
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "Component couldn't be added", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                }
            }
            catch(Exception exception)
            {

            }

            Add_CanAdd = true;
        }

        #endregion
        #region Preview tab
        [RelayCommand]
        private void Preview_LoadDatasheet()
        {
            // TODO: Waiting for WebScraper implementation for AllDatasheets.com
        }

        [RelayCommand]
        private void Preview_LoadFootprint()
        {
            // TODO: Waiting for WebScraper implementation for SnapEda.com
        }
        #endregion
        #region Modify tab
        [ObservableProperty]
        private bool _modify_CanModify;

        [ObservableProperty]
        private string _modify_ComponentName;

        partial void OnModify_ComponentNameChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentManufacturer;

        partial void OnModify_ComponentManufacturerChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentDatasheetLink;

        partial void OnModify_ComponentDatasheetLinkChanged(string value)
        {
            bool isValid = true; // TODO: Implement checking URL
            if(isValid == true)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentCategory;

        partial void OnModify_ComponentCategoryChanged(string value)
        {
            Modify_EvaluateForm();
        }

        [ObservableProperty]
        private string _modify_ComponentShortDescription;
        partial void OnModify_ComponentShortDescriptionChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentFullDescription;
        partial void OnModify_ComponentFullDescriptionChanged(string value)
        {
            if (value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }
        
        [RelayCommand]
        private void Modify_Preview()
        {
            // TODO: waiting for webscraper implementation
        }
        
        [RelayCommand]
        private void Modify_Modify()
        {
            Console.WriteLine("XD");
        }

        [RelayCommand]
        public void NavigateToCollection()
        {
            SelectedTab = 0;
        }

        [RelayCommand]
        private void Modify_Clear()
        {
            Modify_ClearDataToDefault();
        }

        private void Modify_EvaluateForm()
        {
            bool result = false;
            // TODO: include FullDescription and DatasheetLink in future
            if(Modify_ComponentName != SelectedComponent.Name || Modify_ComponentManufacturer != SelectedComponent.Manufacturer ||
               Modify_ComponentShortDescription != SelectedComponent.ShortDescription  || Modify_ComponentFullDescription != SelectedComponent.ShortDescription || 
               Modify_ComponentCategory != SelectedComponent.Category.Name)
            {
                result = true;
            }
            Modify_CanModify = result;
        }
        #endregion
        #endregion
        #region Observable properties methods
        partial void OnSelectedTabChanged(int value)
        {
            /*
             * 0 - Collection
             * 1 - Add 
             * 2 - Preview
             * 3 - Modify
             */
            //string destinationTab = value.Header.ToString();
            if (value == 0)
            {

            }
            else if (value == 1)
            {

            }
            else if (value == 2)
            {
                // We need to lead newest purchases and projects for selected component
                PrepareForPreview();
            }
            else if (value == 3)
            {
                Modify_ClearDataToDefault();
            }
            Console.WriteLine(value.ToString()); 
        }

        [RelayCommand]
        private void Preview_CopyToClipboard()
        {
            ClipboardManager.SetText(SelectedComponent.DatasheetURL);
        }

        [RelayCommand]
        private void Preview_OpenDatasheetLink()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = SelectedComponent.DatasheetURL,
                UseShellExecute = true
            });
        }

        private void PrepareForPreview()
        {
            RefreshSelectedComponentsProjectSource();
            RefreshSelectedComponentsPurchasesSource();
            
            if(SelectedComponent.DatasheetURL == null || SelectedComponent.DatasheetURL == string.Empty)
            {
                Preview_CanPreview = false;
            }
            else
            {
                Preview_CanPreview = true;
            }
        }

        partial void OnSelectedComponentChanged(DetailedComponentContainer value)
        {
            if(value == null)
            {
                PreviewEnabled = false;
            }
            else if(PreviewEnabled == false)
            {
                PreviewEnabled = true;  
            }
        }

        private void Modify_ClearDataToDefault()
        {
            Modify_ComponentName = SelectedComponent.Name;
            Modify_ComponentCategory = SelectedComponent.Category.Name;
            Modify_ComponentManufacturer = SelectedComponent.Manufacturer;
            Modify_ComponentShortDescription = SelectedComponent.ShortDescription;
            Modify_ComponentFullDescription = SelectedComponent.LongDescription;
            Modify_ComponentDatasheetLink = string.Empty; //TODO: fix!
        }
        partial void OnSearchByNameOrDescChanged(string value)
        {
            Console.WriteLine(value);
            Components.Refresh();
        }

        partial void OnOnlyAvailableFlagChanged(bool value)
        {
            Components.Refresh();
            Console.WriteLine(value);
        }

        partial void OnSelectedManufacturerChanged(string value)
        {
            Components.Refresh();
            Console.WriteLine($"{value}");
            var user = DatabaseStore.UsersStore.LoggedInUser;
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            Components.Refresh();
            Console.WriteLine($"{value}");
        }

        [RelayCommand]
        public void ClearAllFiltersAndSorting()
        {
            SelectedCategory = null;
            SelectedManufacturer = null;
            OnlyAvailableFlag = false;
            SearchByNameOrDesc = string.Empty;
            Components.Refresh();
            Console.WriteLine();
        }
        #endregion
        #region Constructor
        public ComponentsPageViewModel(DatabaseStore databaseStore, Navigator navigator) : base(databaseStore, navigator)
        {
            PredefinedImages = new ObservableCollection<ImageContainer>();

            ComponentsSource = new List<DetailedComponentContainer>();
            
            Components = new DataGridCollectionView(ComponentsSource);
            Components.CurrentChanged += CurrentChangedHandler;
            Components.Filter = (object component) =>
            {
                if(component is DetailedComponentContainer detailedComponent)
                {
                    bool isManufacturer = true;
                    bool isCategory = true;
                    bool isNameOrDesc = false;
                    bool onlyAvailable = true;
                    
                    if(OnlyAvailableFlag == true)
                    {
                        if (detailedComponent.AvailableAmount <= 0)
                        {
                            onlyAvailable = false;
                        }
                    }

                    if(SelectedManufacturer != null)
                    {
                        if (!detailedComponent.Manufacturer.Contains(SelectedManufacturer, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isManufacturer = false;
                        }
                    }

                    if (SelectedCategory!= null)
                    {
                        if (!detailedComponent.Category.Name.Contains(SelectedCategory, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isCategory = false;
                        }
                    }

                    if (detailedComponent.Name.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase) ||
                       detailedComponent.ShortDescription.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase) ||
                       detailedComponent.LongDescription.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isNameOrDesc = true;
                    }

                    if(isNameOrDesc == true && isManufacturer == true && isCategory == true && onlyAvailable == true)
                    {
                        return true;
                    }
                }
                return false;
            };

            SelectedComponentsPurchasesSource = new List<DetailedItemPurchaseContainer>();
            SelectedComponentsProjectSource = new List<DetailedItemProjectContainer>();
            PurchasesForSelected = new DataGridCollectionView(SelectedComponentsPurchasesSource);
            ProjectsForSelected = new DataGridCollectionView(SelectedComponentsProjectSource);

            DatabaseStore.PurchaseStore.Load();

            Manufacturers = new ObservableCollection<string>() { };
            DatabaseStore.ComponentStore.Load();
            DatabaseStore.ComponentStore.ComponentsLoaded += HandleComponentsLoaded;

            Categories = new ObservableCollection<string>() { };
            DatabaseStore.CategorieStore.Load();
            DatabaseStore.CategorieStore.CategoriesLoaded += HandleCategoriesLoaded;

            Suppliers = new ObservableCollection<SupplierContainer>();
            DatabaseStore.SupplierStore.SuppliersLoaded += SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.Load();

            IEnumerable<PredefinedImage> imagesFromDB = DatabaseStore.PredefinedImagesStore.Images;
            foreach (PredefinedImage image in imagesFromDB)
            {
                PredefinedImages.Add(new ImageContainer(image));
            }
            CurrentAddPredefinedImage = PredefinedImages[0].Image;
        }
        #endregion

        private void CurrentChangedHandler(object? sender, EventArgs e)
        {
            if(Components.CurrentItem is null)
            {
                // Unselected
                SelectedComponent = null;
            }
            else
            {
                SelectedComponent = Components.CurrentItem as DetailedComponentContainer;
            }
        }

        private void SuppliersLoadedHandler()
        {
            Suppliers.Clear();
            foreach (Supplier supplier in DatabaseStore.SupplierStore.Suppliers)
            {
                Suppliers.Add(new SupplierContainer(supplier));
            }
        }

        private void HandleComponentsLoaded()
        {
            Manufacturers.Clear();
            ComponentsSource.Clear();

            IEnumerable<OwnsComponent> ownedComponents = DatabaseStore.ComponentStore.OwnedComponents;
            IEnumerable<OwnsComponent> unusedComponents = DatabaseStore.ComponentStore.UnusedComponents;
            IEnumerable<Component> components = DatabaseStore.ComponentStore.Components;

            for(int i = 0; i < components.Count(); i++)
            {
                Component component = components.ElementAt(i);
                OwnsComponent ownedComponent = ownedComponents.ElementAt(i);
                OwnsComponent unusedComponent = unusedComponents.ElementAt(i);
                string manufacturer = component.Manufacturer;

                ComponentsSource.Add(new DetailedComponentContainer(component, ownedComponent, unusedComponent));
                if (!Manufacturers.Contains(manufacturer))
                {
                    Manufacturers.Add(manufacturer);
                }
            }

            Components.Refresh();
        }

        private void HandleCategoriesLoaded()
        {
            Categories.Clear();
            IEnumerable<Category> categories = DatabaseStore.CategorieStore.Categories;
            foreach(Category category in categories)
            {
                Categories.Add(category.Name);
            }
        }

        private async void RefreshSelectedComponentsProjectSource()
        {
            // TODO: Implement sobe better and faster way with buffor. For now this will work
            // Request data for selected component
            IEnumerable<ProjectComponent> componentsPurchaseItem = await DatabaseStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfComponents(SelectedComponent.Component);
            
            SelectedComponentsProjectSource.Clear();
            foreach(ProjectComponent component in componentsPurchaseItem)
            {
                Project proj = DatabaseStore.ProjectStore.Projects.FirstOrDefault(x=>x.ID == component.ProjectID);
                SelectedComponentsProjectSource.Add(new DetailedItemProjectContainer(proj, component));
            }
            ProjectsForSelected.Refresh();
        }

        private async void RefreshSelectedComponentsPurchasesSource()
        {
            // TODO: Implement sobe better and faster way with buffor. For now this will work
            // Request data for selected component
            IEnumerable<PurchaseItem> componentsPurchaseItems = await DatabaseStore.PurchaseStore.PurchaseItemDP.GetPurchaseItemsFromComponent(SelectedComponent.Component);

            SelectedComponentsPurchasesSource.Clear();
            foreach (PurchaseItem purchaseItem in componentsPurchaseItems)
            {
                Purchase purchase = DatabaseStore.PurchaseStore.Purchases.FirstOrDefault(x => x.ID == purchaseItem.PurchaseID);
                Supplier supplier = DatabaseStore.SupplierStore.Suppliers.FirstOrDefault(x=>x.ID == purchase.SupplierID);
                SelectedComponentsPurchasesSource.Add(new DetailedItemPurchaseContainer(purchase, purchaseItem, supplier));
            }
            PurchasesForSelected.Refresh();
        }

        public override void Dispose()
        {
            DatabaseStore.CategorieStore.CategoriesLoaded -= HandleCategoriesLoaded;
            DatabaseStore.ComponentStore.ComponentsLoaded -= HandleComponentsLoaded;
            DatabaseStore.SupplierStore.SuppliersLoaded -= SuppliersLoadedHandler;
        }
    }
}
