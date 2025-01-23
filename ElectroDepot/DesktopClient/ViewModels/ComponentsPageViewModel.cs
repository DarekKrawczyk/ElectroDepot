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
using DynamicData;
using System.Reactive.Subjects;
using DesktopClient.Services;
using DynamicData.Binding;
using System.Drawing.Printing;
using System.Reactive.Linq;
using DynamicData.Operators;
using System.Reactive.Concurrency;
using Avalonia.Controls.Primitives;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Security.Cryptography.X509Certificates;

namespace DesktopClient.ViewModels
{
    public partial class ComponentsPageViewModel : RootNavigatorViewModel, INavParamInterpreter
    {
        #region Tab navigation
        partial void OnSelectedTabChanged(int value)
        {
            Evaluate_AddTabVisibilty();
        }

        public void Evaluate_AddTabVisibilty()
        {
            Evaluate_Add_IsComponentsTabEnabled();
            Evaluate_Add_IsAddTabEnabled();
            Evaluate_Add_IsPreviewTabEnabled();
        }

        [ObservableProperty]
        private bool _add_IsComponentsTabEnabled;

        private void Evaluate_Add_IsComponentsTabEnabled()
        {
            bool isVisible = false;
            if(SelectedTab == 0)
            {
                isVisible = true;
            }
            else if(SelectedTab == 1)
            {
                isVisible = !WasAddFormChanged();
            }
            else if(SelectedTab == 2)
            {
                isVisible = true; // TODO: implement function to check wheter previre.modify was changed.
            }
            else
            {
                isVisible = true;
            }
            Add_IsComponentsTabEnabled = isVisible;
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
                isVisible = true; // TODO: implement function to check wheter previre.modify was changed.
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
                isVisible = true;
            }
            else
            {
                isVisible = true;
            }
            Add_IsPreviewTabEnabled = isVisible;
        }

        public async Task NavigateTab(ComponentTab tab)
        {
            switch (tab)
            {
                case ComponentTab.Components:
                    if (SelectedTab == 0) break;    // User is on this Tab so do not change anything.
                    else
                    {
                        if(SelectedTab == 1)
                        {
                            bool wasChanged = WasAddFormChanged();

                            if (wasChanged == true)
                            {
                                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                                if (result == "Yes")
                                {
                                    // Clear changes and navigate to 'Components' tab.
                                    Add_ClearComponent();
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
                        else if(SelectedTab == 2)
                        {
                            SelectedTab = 0;
                        }
                        else if (SelectedTab == 3)
                        {
                            SelectedTab = 0;
                        }
                    }
                    break;
                case ComponentTab.Add:
                    SelectedTab = 1;
                    break;
                case ComponentTab.Preview:
                    PrepareForPreview();
                    SelectedTab = 2;
                    break;
                case ComponentTab.Edit:
                    Modify_ClearDataToDefault();
                    SelectedTab = 2;
                    break;
                default:
                    SelectedTab = 0;
                    break;
            }

        }
        #endregion
        #region Add tab
        #region Main fields section
        #region Name field
        [RelayCommand]
        public void AddTab_ClearName()
        {
            _hasUserInteractedWithName = false;
            Add_ComponentName = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyNameClipboard))]
        public void AddTab_CopyNameClipboard()
        {
            ClipboardManager.SetText(Add_ComponentName);
        }

        public bool AddTab_CanCopyNameClipboard()
        {
            if (Add_ComponentName != null && Add_ComponentName.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Manufacturer field
        [RelayCommand]
        public void AddTab_ClearManufacturer()
        {
            _hasUserInteractedWithManufacturer = false;
            Add_Manufacturer = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyManufacturerClipboard))]
        public void AddTab_CopyManufacturerClipboard()
        {
            ClipboardManager.SetText(Add_Manufacturer);
        }

        public bool AddTab_CanCopyManufacturerClipboard()
        {
            if (Add_Manufacturer != null && Add_Manufacturer.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Category field
        [RelayCommand]
        public void AddTab_ClearCategory()
        {
            _hasUserInteractedWithCategory = false;
            Add_Category = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyCategoryClipboard))]
        public void AddTab_CopyCategoryClipboard()
        {
            ClipboardManager.SetText(Add_Category as string);
        }

        public bool AddTab_CanCopyCategoryClipboard()
        {
            if (Add_Category != null)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Datasheet
        [RelayCommand]
        public void AddTab_DatasheetName()
        {
            Add_DatasheetLink = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyDatasheetClipboard))]
        public void AddTab_CopyDatasheetClipboard()
        {
            ClipboardManager.SetText(Add_DatasheetLink);
        }

        public bool AddTab_CanCopyDatasheetClipboard()
        {
            if (Add_DatasheetLink != null && Add_DatasheetLink.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion
        #region Main buttons section
        private bool WasAddFormChanged()
        {
            bool nameChanged = Add_ComponentName != null && Add_ComponentName != string.Empty;
            bool manufacturerChanged = Add_Manufacturer != null && Add_Manufacturer != string.Empty;
            bool categoryChanged = Add_Category != null;
            bool datasheetChanged = Add_DatasheetLink != null && Add_DatasheetLink != string.Empty;
            //bool imageChanged = CurrentAddPredefinedImage != ?; // TODO: Think about that.
            bool aboutChanged = Add_ShortDescription != null && Add_ShortDescription != string.Empty;
            bool descriptionChanged = Add_FullDescription != null && Add_FullDescription != string.Empty;

            bool ifAny = (nameChanged || manufacturerChanged || categoryChanged || datasheetChanged || aboutChanged || descriptionChanged);
            return ifAny;
        }
        [RelayCommand]
        public async Task Add_Cancel()
        {
            /*  User can 'Cancel' the operation everytime and app should navigate him to 'Components' tab.
             *  If User applied any changes to 'Add' tab 'Form' then he should be warned with 'MessageBox'
             */
            await NavigateTab(ComponentTab.Components);
        }
        #endregion
        #region About section
        [RelayCommand]
        public void AddTab_DownloadAbout()
        {
            // TODO: Implement!
        }

        [RelayCommand]
        public void AddTab_ClearAbout()
        {
            _hasUserInteractedWithDescription = false;
            Add_ShortDescription = null;
        }
        #endregion
        #region Description section
        [RelayCommand]
        public void AddTab_DownloadDescription()
        {
            // TODO: Implement!
        }

        [RelayCommand]
        public void AddTab_ClearDescription()
        {
            Add_FullDescription = null;
        }
        #endregion
        #endregion

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int _selectedPageSizeIndex = 0;

        public int FirstPageIndex = 1;
        public int SelectedPageSize = 10;

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

        #region Data source
        //public List<DetailedComponentContainer> ComponentsSource { get; set; }
        public List<DetailedItemPurchaseContainer> SelectedComponentsPurchasesSource { get; set; }
        public List<DetailedItemProjectContainer> SelectedComponentsProjectSource { get; set; }
        private readonly ComponentHolderService _componentsService;
        private readonly ISubject<PageRequest> _pager;
        private readonly ReadOnlyObservableCollection<DetailedComponentContainer> _components;
        public ReadOnlyObservableCollection<DetailedComponentContainer> ComponentsCollection => _components;
        #endregion

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
                SelectedPredefinedImage = null;
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
            SelectedPredefinedImage = null;
        }

        [ObservableProperty]
        private string _myText = "XDXDXD";

        #region Observable for data source
        public ObservableCollection<string> Manufacturers { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<SupplierContainer> Suppliers { get; set; } = new ObservableCollection<SupplierContainer>() { new SupplierContainer(new Supplier(0, "XD", "XD", null)) };
        public ObservableCollection<ImageContainer> PredefinedImages { get; set; }

        //public DataGridCollectionView Components { get; set; }
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
                if (_hasUserInteractedWithCategory == false || Add_Category == null ||
                    _hasUserInteractedWithDescription == false || Add_ShortDescription == null || Add_ShortDescription == string.Empty ||
                    _hasUserInteractedWithManufacturer == false || Add_Manufacturer == null || Add_Manufacturer == string.Empty ||
                    _hasUserInteractedWithName == false || Add_ComponentName == null || Add_ComponentName == string.Empty
                    )
                {
                    Add_CanAdd = false;
                }
                else
                {
                    Add_CanAdd = !HasErrors;
                }
                Evaluate_AddTabVisibilty();
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Name field cannot be empty")]
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyNameClipboardCommand))]
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
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyManufacturerClipboardCommand))]
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
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyCategoryClipboardCommand))]
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
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyDatasheetClipboardCommand))]
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
            NavigateTab(ComponentTab.Preview);
        }

        [RelayCommand]
        private void GoToAddNew()
        {
            NavigateTab(ComponentTab.Add);
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
                string buttonResult = await MsBoxService.DisplayMessageBox("Component with that name already exists!", Icon.Error);
                return;
            }
            // TODO: implement
            Add_CanAdd = false;

            try
            {
                string categoryName = Add_Category as string;

                Category cat = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x=>x.Name == categoryName);

                string datasheet = Add_DatasheetLink == null ? string.Empty : Add_DatasheetLink;
                string longDesc = Add_FullDescription == null ? string.Empty : Add_FullDescription;

                Component newComponent = new Component(id: 0, cat.ID, category: cat, name: Add_ComponentName, manufacturer: Add_Manufacturer, shortDescription: Add_ShortDescription,
                    longDescription: longDesc, datasheetLink: datasheet, byteImage: ImageConverterUtility.BitmapToBytes(CurrentAddPredefinedImage));

                bool result = await DatabaseStore.ComponentStore.InsertNewComponent(newComponent);

                if(result == true)
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("Components added successfully! Do you want to add another component?", Icon.Question);

                    Add_ClearComponent();
                    if (dialogResult == "No")
                    {
                        NavigateTab(ComponentTab.Components);
                    }
                }
                else
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("There was an error while adding component. Try again or contact administrator!", Icon.Error);
                }
            }
            catch(Exception exception)
            {

            }
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
            NavigateTab(ComponentTab.Components);
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

        [RelayCommand]
        public void ClearSelectedManufacturer()
        {
            SelectedManufacturer = null;
        }

        [RelayCommand]
        public void ClearSelectedCategory()
        {
            SelectedCategory = null;
        }

        [RelayCommand]
        public void ClearSearchBar()
        {
            SearchByNameOrDesc = null;
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
            //Components.Refresh();
        }

        partial void OnOnlyAvailableFlagChanged(bool value)
        {
            //Components.Refresh();
            Console.WriteLine(value);
        }

        partial void OnSelectedManufacturerChanged(string value)
        {
            //Components.Refresh();
            Console.WriteLine($"{value}");
            var user = DatabaseStore.UsersStore.LoggedInUser;
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            //Components.Refresh();
            Console.WriteLine($"{value}");
        }

        [RelayCommand]
        public void ClearAllFiltersAndSorting()
        {
            SelectedCategory = null;
            SelectedManufacturer = null;
            OnlyAvailableFlag = false;
            SearchByNameOrDesc = string.Empty;
            //Components.Refresh();
            Console.WriteLine();
        }

        [ObservableProperty]
        private int _totalItems;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int _currentPage;

        [ObservableProperty]
        private int _totalPages;

        private void PagingUpdate(IPageResponse response)
        {
            TotalItems = response.TotalSize;
            CurrentPage = response.Page;
            TotalPages = response.Pages;
        }

        #endregion
        private static Func<DetailedComponentContainer, bool> BuildFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return trade => true;
            return t => t.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        t.ShortDescription.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        t.LongDescription.Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<DetailedComponentContainer, bool> AvailableFilterPredicate(bool available)
        {
            if (available == false) return x => true;
            return x => x.AvailableAmount > 0;
        }

        private static Func<DetailedComponentContainer, bool> ManufacturerFilterPredicate(string manufacturer)
        {
            if (string.IsNullOrEmpty(manufacturer)) return trade => true;
            return t => t.Manufacturer.Contains(manufacturer, StringComparison.InvariantCultureIgnoreCase);
        }
        private static Func<DetailedComponentContainer, bool> CategoryFilterPredicate(string category)
        {
            if (string.IsNullOrEmpty(category)) return trade => true;
            return t => t.Category.Name.Contains(category, StringComparison.InvariantCultureIgnoreCase);
        }

        #region Constructor
        public ComponentsPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService) : base(defaultRootPageViewModel, databaseStore, msgBoxService)
        {
            _componentsService = new ComponentHolderService(DatabaseStore.ComponentStore);

            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPageIndex, SelectedPageSize));

            var nameFilter = this.WhenValueChanged(t => t.SearchByNameOrDesc)
                //.Throttle(TimeSpan.FromMilliseconds(250))
                .Select(BuildFilter);

            var availableFilter = this.WhenValueChanged(t => t.OnlyAvailableFlag)
                .Select(AvailableFilterPredicate);

            var manufacturerFilter = this.WhenValueChanged(t => t.SelectedManufacturer)
                .Select(ManufacturerFilterPredicate);

            var categoryFilter = this.WhenValueChanged(t => t.SelectedCategory)
                .Select(CategoryFilterPredicate);

            _componentsService.EmployeesConnection()
                .Filter(nameFilter)
                .Filter(availableFilter)
                .Filter(manufacturerFilter)
                .Filter(categoryFilter)
                .Sort(SortExpressionComparer<DetailedComponentContainer>.Ascending(e => e.ID))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .ObserveOn(Scheduler.CurrentThread) // Marshals to the current thread (often used for UI updates)
                .Bind(out _components)
                .Subscribe();

            _componentsService.LoadData();

            PredefinedImages = new ObservableCollection<ImageContainer>();

            //ComponentsSource = new List<DetailedComponentContainer>();
            
            //Components = new DataGridCollectionView(ComponentsSource);
            //Components.CurrentChanged += CurrentChangedHandler;
            //Components.Filter = (object component) =>
            //{
            //    if(component is DetailedComponentContainer detailedComponent)
            //    {
            //        bool isManufacturer = true;
            //        bool isCategory = true;
            //        bool isNameOrDesc = false;
            //        bool onlyAvailable = true;
                    
            //        if(OnlyAvailableFlag == true)
            //        {
            //            if (detailedComponent.AvailableAmount <= 0)
            //            {
            //                onlyAvailable = false;
            //            }
            //        }

            //        if(SelectedManufacturer != null)
            //        {
            //            if (!detailedComponent.Manufacturer.Contains(SelectedManufacturer, StringComparison.InvariantCultureIgnoreCase))
            //            {
            //                isManufacturer = false;
            //            }
            //        }

            //        if (SelectedCategory!= null)
            //        {
            //            if (!detailedComponent.Category.Name.Contains(SelectedCategory, StringComparison.InvariantCultureIgnoreCase))
            //            {
            //                isCategory = false;
            //            }
            //        }

            //        if (detailedComponent.Name.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase) ||
            //           detailedComponent.ShortDescription.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase) ||
            //           detailedComponent.LongDescription.Contains(_searchByNameOrDesc, StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            isNameOrDesc = true;
            //        }

            //        if(isNameOrDesc == true && isManufacturer == true && isCategory == true && onlyAvailable == true)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //};

            SelectedComponentsPurchasesSource = new List<DetailedItemPurchaseContainer>();
            SelectedComponentsProjectSource = new List<DetailedItemProjectContainer>();
            PurchasesForSelected = new DataGridCollectionView(SelectedComponentsPurchasesSource);
            ProjectsForSelected = new DataGridCollectionView(SelectedComponentsProjectSource);

            DatabaseStore.PurchaseStore.Load();

            Manufacturers = new ObservableCollection<string>() { };
            DatabaseStore.ComponentStore.Load();

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

            Evaluate_AddTabVisibilty();
        }
        #endregion

        //private void CurrentChangedHandler(object? sender, EventArgs e)
        //{
        //    if(Components.CurrentItem is null)
        //    {
        //        // Unselected
        //        SelectedComponent = null;
        //    }
        //    else
        //    {
        //        SelectedComponent = Components.CurrentItem as DetailedComponentContainer;
        //    }
        //}

        private void SuppliersLoadedHandler()
        {
            Suppliers.Clear();
            foreach (Supplier supplier in DatabaseStore.SupplierStore.Suppliers)
            {
                Suppliers.Add(new SupplierContainer(supplier));
            }
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

        public void InterpreteNavigationParameter(NavParam navigationParameter)
        {
            switch (navigationParameter.Operation)
            {
                case NavOperation.Add:
                    NavigateTab(ComponentTab.Add);
                    break;
                case NavOperation.Preview:
                    break;
                default:
                    break;
            }
        }

        //public override void Dispose()
        //{
        //    DatabaseStore.CategorieStore.CategoriesLoaded -= HandleCategoriesLoaded;
        //    DatabaseStore.ComponentStore.ComponentsLoaded -= HandleComponentsLoaded;
        //    DatabaseStore.SupplierStore.SuppliersLoaded -= SuppliersLoadedHandler;
        //}
    }
}
