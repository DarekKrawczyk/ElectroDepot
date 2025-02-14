using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DesktopClient.Navigation;
using DesktopClient.Services;
using DynamicData;
using System.Reactive.Subjects;
using DynamicData.Operators;
using DynamicData.Binding;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Controls;
using DesktopClient.Utils;
using System.ComponentModel;

using ModelType = ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.DataProviders;

namespace DesktopClient.ViewModels
{
    public partial class ProjectsPageViewModel : RootNavigatorViewModel, INavParamInterpreter
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
            Evaluate_Add_IsPreviewTabEnabled();
        }

        [ObservableProperty]
        private bool _add_IsProjectsTabEnabled;

        private void Evaluate_Add_IsPurchasesTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = true;
            }
            else if (SelectedTab == 1)
            {
                isVisible = !Add_CanClearProject();
            }
            else if (SelectedTab == 2)
            {
                if (Preview_IsEditing == true)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else
            {
                isVisible = true;
            }
            Add_IsProjectsTabEnabled = isVisible;
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
                if (Preview_IsEditing == true)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
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

        private async void ClearPreviewData()
        {
            //Preview_PreviewedComponent = new DetailedComponentContainer(Components_SelectedComponent);
            Preview_Image = ProjectsCollection_SelectedItem.Project.Image;
            Preview_NameField = ProjectsCollection_SelectedItem.Project.Name;
            Preview_DateField = ProjectsCollection_SelectedItem.Project.CreatedAt.ToString("d");
            //Preview_ManufacturerField = Preview_PreviewedComponent.Manufacturer;
            //Preview_CategoryField = Preview_PreviewedComponent.Category.Name;
            //if (Preview_CategoryField != string.Empty)
            //{
            //    Preview_CategoryComboBoxItem = Preview_CategoryField;
            //}
            //Preview_DatasheetField = Preview_PreviewedComponent.DatasheetURL;
            //Preview_AboutField = Preview_PreviewedComponent.ShortDescription;
            Preview_DescriptionField = ProjectsCollection_SelectedItem.Project.Description;

            Preview_ProjectComponentsSource.Clear();
            IEnumerable<ProjectComponent> projectComponents = await DatabaseStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfProject(ProjectsCollection_SelectedItem.Project);
            foreach(ProjectComponent projComp in projectComponents)
            {
                ModelType.Component component = DatabaseStore.ComponentStore.Components.FirstOrDefault(x=>x.ID == projComp.ComponentID);
                Preview_ProjectComponentsSource.Add(new ProjectComponentHolder(this, component, projComp));
            }
            Preview_ProjectComponents.Refresh();

            //if (Preview_DatasheetField != null && Preview_DatasheetField != string.Empty)
            //{
            //    Preview_CanDisplayDatasheet = true;
            //}
        }

        private void NavigationPreparePreviewTab()
        {
            ClearPreviewData();
        }

        public async Task NavigateTab(ComponentTab tab)
        {
            switch (tab)
            {
                case ComponentTab.Components:
                    _projectsService.LoadData();    // Get latest values from db.
                    if (SelectedTab == 0) break;    // User is on this Tab so do not change anything.
                    else
                    {
                        if (SelectedTab == 1)
                        {
                            bool wasChanged = Add_CanClearProject();

                            if (wasChanged == true)
                            {
                                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                                if (result == "Yes")
                                {
                                    // Clear changes and navigate to 'Components' tab.
                                    Add_ClearProject();
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
                            SelectedTab = 0;
                        }
                    }
                    break;
                case ComponentTab.Add:
                    SelectedTab = 1;
                    break;
                case ComponentTab.Preview:
                    //RefreshSelectedComponentsProjectSource();
                    //RefreshSelectedComponentsPurchasesSource();
                    NavigationPreparePreviewTab();
                    //ChangeToPreviewMode();
                    //PrepareForPreview();
                    SelectedTab = 2;
                    break;
                case ComponentTab.Edit:
                    //RefreshSelectedComponentsProjectSource();
                    //RefreshSelectedComponentsPurchasesSource();
                    NavigationPreparePreviewTab();
                    ChangeToEditMode();
                    //ChangeToEditMode();
                    //Modify_ClearDataToDefault();
                    SelectedTab = 2;
                    break;
                default:
                    SelectedTab = 0;
                    break;
            }

        }
        #endregion
        #region Projects
        private ProjectContainerHolder Previewed_ProjectsCollection_SelectedItem;

        [ObservableProperty]
        private ProjectContainerHolder _projectsCollection_SelectedItem;

        partial void OnProjectsCollection_SelectedItemChanged(ProjectContainerHolder value)
        {
            if(value != null)
            {
                Previewed_ProjectsCollection_SelectedItem = new ProjectContainerHolder(value);
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearNameFilterCommand))]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearAllFiltersCommand))]
        private string _projects_NameFilter;
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearDescFilterCommand))]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearAllFiltersCommand))]
        private string _projects_DescFilter;

        [RelayCommand(CanExecute = nameof(Projects_CanClearNameFilter))]
        public void Projects_ClearNameFilter()
        {
            Projects_NameFilter = null;
        }
        
        private bool Projects_CanClearNameFilter()
        {
            return Projects_NameFilter != null && Projects_NameFilter != string.Empty;
        }

        [RelayCommand(CanExecute = nameof(Projects_CanClearDescFilter))]
        public void Projects_ClearDescFilter()
        {
            Projects_DescFilter = null;
        }

        private bool Projects_CanClearDescFilter()
        {
            return Projects_DescFilter != null && Projects_DescFilter != string.Empty;
        }

        #region Date
        [ObservableProperty]
        private DateTimeOffset _projects_DateMaxYear;
        
        [ObservableProperty]
        private DateTimeOffset _projects_DateMinYear;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearAllFiltersCommand))]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearDateFromFilterCommand))]
        private DateTimeOffset _projects_DateFromFilter;

        partial void OnProjects_DateFromFilterChanged(DateTimeOffset value)
        {
            Console.WriteLine();
        }

        [RelayCommand(CanExecute = nameof(Projects_CanClearDateFromFilter))]
        public void Projects_ClearDateFromFilter()
        {
            Projects_DateFromFilter = Projects_DateMinYear;
        }

        private bool Projects_CanClearDateFromFilter()
        {
            return Projects_DateFromFilter.Date != Projects_DateMinYear.Date;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearAllFiltersCommand))]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearDateToFilterCommand))]
        private DateTimeOffset _projects_DateToFilter;

        [RelayCommand(CanExecute = nameof(Projects_CanClearDateToFilter))]
        public void Projects_ClearDateToFilter()
        {
            Projects_DateToFilter = Projects_DateMaxYear;
        }

        private bool Projects_CanClearDateToFilter()
        {
            return Projects_DateToFilter.Date != Projects_DateMaxYear.Date;
        }
        #endregion

        #region Sorting
        private static Func<ProjectContainerHolder, bool> NameFilter(string name)
        {
            if (string.IsNullOrEmpty(name)) return trade => true;
            return t => t.Project.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<ProjectContainerHolder, bool> DescriptionFilter(string description)
        {
            if (string.IsNullOrEmpty(description)) return trade => true;
            return t => t.Project.Description.Contains(description, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<ProjectContainerHolder, bool> FromDateFilter(DateTimeOffset fromDate)
        {
            return x => x.Project.CreatedAt.Date >= fromDate.Date;
        }

        private static Func<ProjectContainerHolder, bool> ToDateFilter(DateTimeOffset fromDate)
        {
            return x => x.Project.CreatedAt.Date <= fromDate.Date;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearSelectedSortingCommand))]
        [NotifyCanExecuteChangedFor(nameof(Projects_ClearAllFiltersCommand))]
        private ComboBoxItem _projects_SelectedSorting;

        [RelayCommand(CanExecute = nameof(Projects_CanClearSelectedSorting))]
        public void Projects_ClearSelectedSorting()
        {
            Projects_SelectedSorting = null;
        }

        private bool Projects_CanClearSelectedSorting()
        {
            return Projects_SelectedSorting != null;
        }
        #endregion
        [RelayCommand(CanExecute = nameof(Projects_CanClearAllFilters))]
        public void Projects_ClearAllFilters()
        {
            //FirstPage();
            //SelectedPageSizeIndex = 0;

            Projects_DateFromFilter = Projects_DateMinYear;
            Projects_DateToFilter = Projects_DateMaxYear;

            Projects_NameFilter = null;
            Projects_DescFilter = null;
            Projects_SelectedSorting = null;
        }

        public bool WasPagingChanged()
        {
            if (CurrentPage != 1 || SelectedPageSizeIndex != 0) return true;
            return false;
        }

        private bool Projects_CanClearAllFilters()
        {
            //bool pagingChanged = WasPagingChanged();
            bool dateChanged = Projects_CanClearDateFromFilter() || Projects_CanClearDateToFilter();
            bool nameFilterChaned = Projects_CanClearNameFilter();
            bool descFilterChaned = Projects_CanClearDescFilter();
            bool sortingChanged = Projects_CanClearSelectedSorting();

            bool result = dateChanged || nameFilterChaned || descFilterChaned || sortingChanged;
            return result;
        }
        #region Data source

        private readonly ProjectHolderService _projectsService;
        private readonly ISubject<PageRequest> _pager;
        private readonly ReadOnlyObservableCollection<ProjectContainerHolder> _projects;
        public ReadOnlyObservableCollection<ProjectContainerHolder> ProjectsCollection => _projects;
        #endregion

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

        [RelayCommand]
        public void Projects_Refresh()
        {
            _projectsService.LoadData();
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

        #endregion

        #region Add tab
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

            // Load but where
            if (Preview_IsEditing == true)
            {
                Preview_Image = imageAsBitmap;
            }
            else
            {
                Add_CurrentAddImage = imageAsBitmap;
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectComponentsNameFilterCommand))]
        private string _add_ProjectComponentsNameFilter;

        partial void OnAdd_ProjectComponentsNameFilterChanged(string value)
        {
            RefreshProjectComponents();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearProjectComponentsNameFilter))]
        public void Add_ClearProjectComponentsNameFilter()
        {
            Add_ProjectComponentsNameFilter = string.Empty;
        }

        private bool Add_CanClearProjectComponentsNameFilter()
        {
            return Add_ProjectComponentsNameFilter != null && Add_ProjectComponentsNameFilter != string.Empty;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectComponentsManufacturerFilterCommand))]
        private string _add_ProjectComponentsManufacturerFilter;

        partial void OnAdd_ProjectComponentsManufacturerFilterChanged(string value)
        {
            RefreshProjectComponents();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearProjectComponentsManufacturerFilter))]
        public void Add_ClearProjectComponentsManufacturerFilter()
        {
            Add_ProjectComponentsManufacturerFilter = string.Empty;
        }

        private bool Add_CanClearProjectComponentsManufacturerFilter()
        {
            return Add_ProjectComponentsManufacturerFilter != null && Add_ProjectComponentsManufacturerFilter != string.Empty;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearAvailableComponentsNameFilterCommand))]
        private string _add_AvailableComponentsNameFilter;

        partial void OnAdd_AvailableComponentsNameFilterChanged(string value)
        {
            RefreshProjectComponents();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearAvailableComponentsNameFilter))]
        public void Add_ClearAvailableComponentsNameFilter()
        {
            Add_AvailableComponentsNameFilter = string.Empty;
        }

        private bool Add_CanClearAvailableComponentsNameFilter()
        {
            return Add_AvailableComponentsNameFilter != null && Add_AvailableComponentsNameFilter != string.Empty;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearAvailableComponentsManufacturerFilterCommand))]
        private string _add_AvailableComponentsManufacturerFilter;

        partial void OnAdd_AvailableComponentsManufacturerFilterChanged(string value)
        {
            RefreshProjectComponents();
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearAvailableComponentsManufacturerFilter))]
        public void Add_ClearAvailableComponentsManufacturerFilter()
        {
            Add_AvailableComponentsManufacturerFilter = string.Empty;
        }

        private bool Add_CanClearAvailableComponentsManufacturerFilter()
        {
            return Add_AvailableComponentsManufacturerFilter != null && Add_AvailableComponentsManufacturerFilter != string.Empty;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearNameCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectCommand))]
        private string _add_Name;

        [RelayCommand(CanExecute = nameof(Add_CanClearName))]
        public void Add_ClearName()
        {
            Add_Name = string.Empty;
        }

        private bool Add_CanClearName()
        {
            return Add_Name != null && Add_Name != string.Empty;
        }

        private Bitmap _defaultImage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearImageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectCommand))]
        private Bitmap _add_CurrentAddImage;

        [RelayCommand(CanExecute = nameof(Add_CanClearImage))]
        public void Add_ClearImage()
        {
            Add_CurrentAddImage = _defaultImage;
        }

        private bool Add_CanClearImage()
        {
            return Add_CurrentAddImage != _defaultImage;
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearSelectedDate))]
        public void Add_ClearSelectedDate()
        {
            Add_SelectedDate = DateTime.Now.Date;
        }

        private bool Add_CanClearSelectedDate()
        {
            return Add_SelectedDate != DateTime.Now.Date;
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearDescription))]
        public void Add_ClearDescription()
        {
            Add_SelectedDescription = null;
        }

        private bool Add_CanClearDescription()
        {
            return Add_SelectedDescription != null && Add_SelectedDescription != string.Empty;   
        }

        [RelayCommand]
        public void Add_Cancel()
        {
            NavigateTab(ComponentTab.Components);
        }
        #endregion

        #region Preview / Edit tab
        #region Name
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_NameEditingSaveChangesCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_NameClearCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearAllEditingCommand))]
        private string _preview_NameField = string.Empty;

        partial void OnPreview_NameFieldChanged(string value)
        {
            Previewed_ProjectsCollection_SelectedItem.Project.Name = value;
            Preview_NameEditingSaveChangesCommand.NotifyCanExecuteChanged();
            Preview_NameClearCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_NameNotEditing = true;

        partial void OnPreview_NameNotEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        private bool _preview_NameEditing;

        [RelayCommand(CanExecute = nameof(Preview_CanNameClear))]
        public void Preview_NameClear()
        {
            Preview_NameField = ProjectsCollection_SelectedItem.Project.Name;
        }

        public bool Preview_CanNameClear()
        {
            if (ProjectsCollection_SelectedItem == null || Preview_NameField == ProjectsCollection_SelectedItem.Project.Name) return false;
            else return true;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanExecuteNameEditingSaveChanges))]
        public void Preview_NameEditingSaveChanges()
        {
            Preview_NameEditing = false;
            Preview_NameNotEditing = true;
        }

        public bool Preview_CanExecuteNameEditingSaveChanges()
        {
            if (Preview_NameField == null || Preview_NameField == string.Empty) return false;
            else return true;
        }

        [RelayCommand]
        public void Preview_NameEditingStart()
        {
            Preview_NameNotEditing = false;
            Preview_NameEditing = true;
        }

        [RelayCommand]
        public async Task Preview_NameCopy()
        {
            await ClipboardManager.SetText(Preview_NameField);
        }
        #endregion

        #region Date
        [ObservableProperty]
        private string _preview_DateField = string.Empty;

        [RelayCommand]
        public async Task Preview_DateCopyToClipboardCommand()
        {
            await ClipboardManager.SetText(Preview_DateField);
        }
        #endregion

        #region Description
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_DescriptionRevertCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearAllEditingCommand))]
        private string _preview_DescriptionField = string.Empty;

        partial void OnPreview_DescriptionFieldChanged(string value)
        {
            Previewed_ProjectsCollection_SelectedItem.Project.Description = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_DescriptionNotEditing = true;

        [ObservableProperty]
        private bool _preview_DescriptionEditing;

        partial void OnPreview_DescriptionEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        public void Preview_DescriptionClear()
        {
            Preview_DescriptionField = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanDescriptionRevert))]
        public void Preview_DescriptionRevert()
        {
            Preview_DescriptionField = ProjectsCollection_SelectedItem.Project.Description;
        }

        public bool Preview_CanDescriptionRevert()
        {
            if (ProjectsCollection_SelectedItem != null && ProjectsCollection_SelectedItem.Project.Description != Preview_DescriptionField) return true;
            else return false;
        }

        [RelayCommand]
        public void Preview_DescriptionEditingSaveChanges()
        {
            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;
        }

        [RelayCommand]
        public void Preview_DescriptionEditingStart()
        {
            Preview_DescriptionNotEditing = false;
            Preview_DescriptionEditing = true;
        }

        [RelayCommand]
        public async Task Preview_DescriptionCopy()
        {
            await ClipboardManager.SetText(Preview_DescriptionField);
        }
        #endregion

        #region Image
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearImageCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearAllEditingCommand))]
        private Bitmap _preview_Image;

        partial void OnPreview_ImageChanged(Bitmap value)
        {
           Previewed_ProjectsCollection_SelectedItem.Project.Image = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_ImageNotEditing = true;

        [ObservableProperty]
        private bool _preview_ImageEditing;

        partial void OnPreview_ImageEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        public void Preview_ImageEditingSaveChanges()
        {
            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        [RelayCommand]
        public void Preview_ImageEditingStart()
        {
            Preview_ImageNotEditing = false;
            Preview_ImageEditing = true;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearImage))]
        public void Preview_ClearImage()
        {
            Preview_Image = ProjectsCollection_SelectedItem.Project.Image;
        }

        public bool Preview_CanClearImage()
        {
            if (ProjectsCollection_SelectedItem != null && ProjectsCollection_SelectedItem.Project.Image != Preview_Image) return true;
            else return false;
        }
        #endregion

        #region Components
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearProjectComponentsNameFilterCommand))]
        private string _preview_ProjectComponentsNameFilter;

        partial void OnPreview_ProjectComponentsNameFilterChanged(string value)
        {
            Preview_ProjectComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearProjectComponentsNameFilter))]
        public void Preview_ClearProjectComponentsNameFilter()
        {
            Preview_ProjectComponentsNameFilter = string.Empty;
        }

        private bool Preview_CanClearProjectComponentsNameFilter()
        {
            return Preview_ProjectComponentsNameFilter != null && Preview_ProjectComponentsNameFilter != string.Empty;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearProjectComponentsManufacturerFilterCommand))]
        private string _preview_ProjectComponentsManufacturerFilter;

        partial void OnPreview_ProjectComponentsManufacturerFilterChanged(string value)
        {
            Preview_ProjectComponents.Refresh();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearProjectComponentsManufacturerFilter))]
        public void Preview_ClearProjectComponentsManufacturerFilter()
        {
            Add_ProjectComponentsManufacturerFilter = string.Empty;
        }

        private bool Preview_CanClearProjectComponentsManufacturerFilter()
        {
            return Preview_ProjectComponentsManufacturerFilter != null && Preview_ProjectComponentsManufacturerFilter != string.Empty;
        }
        public List<ProjectComponentHolder> Preview_ProjectComponentsSource { get; set; }
        public DataGridCollectionView Preview_ProjectComponents { get; set; }
        #endregion

        [ObservableProperty]
        private bool _preview_IsPreviewing = true;

        [ObservableProperty]
        private bool _preview_IsEditing;

        #region Main buttons
        private void ChangeToPreviewMode()
        {
            Preview_IsPreviewing = true;
            Preview_IsEditing = false;

            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;

            Preview_NameEditing = false;
            Preview_NameNotEditing = true;

            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        private void ChangeToEditMode()
        {
            Preview_IsPreviewing = false;
            Preview_IsEditing = true;

            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;

            Preview_NameEditing = false;
            Preview_NameNotEditing = true;

            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        [RelayCommand]
        public void Preview_EditWhole()
        {
            ChangeToEditMode();
        }
        
        [RelayCommand]
        public async Task Preview_SaveWhole()
        {
            try
            {
                Project result = await DatabaseStore.ProjectStore.ProjectDP.UpdateProject(Previewed_ProjectsCollection_SelectedItem.Project);

                if (result != null)
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("Project modified successfully! Do you to be navigated to 'Projects' tab?", Icon.Question);

                    //Add_ClearComponent();
                    if (dialogResult == "Yes")
                    {
                        NavigateTab(ComponentTab.Components);
                    }
                }
                else
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("There was an error while editing this project. Try again or contact administrator!", Icon.Error);
                }
            }
            catch (Exception exception)
            {

            }

            ChangeToPreviewMode();
        }

        private bool WasEditFormChanged()
        {
            if (ProjectsCollection_SelectedItem == null) return false;
            bool nameChanged = Preview_NameField != ProjectsCollection_SelectedItem.Project.Name;
            bool imageChanged = ProjectsCollection_SelectedItem.Project.Image != Preview_Image;
            bool descriptionChanged = Preview_DescriptionField != ProjectsCollection_SelectedItem.Project.Description;

            bool ifAny = (nameChanged || imageChanged || descriptionChanged);
            return ifAny;
        }

        private bool IsEditFormInPreviewState()
        {
            if (Preview_NameEditing == false && Preview_DescriptionEditing == false && Preview_ImageEditing == false) return true;
            else return false;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearAllEditing))]
        public void Preview_ClearAllEditing()
        {
            ClearPreviewData();
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
            Preview_SaveWholeCommand.NotifyCanExecuteChanged();
        }

        public bool Preview_CanClearAllEditing()
        {
            bool canClear = WasEditFormChanged() && IsEditFormInPreviewState();
            return canClear;
        }

        [RelayCommand]
        public async Task Preview_CancelEditing()
        {
            bool wasChanged = WasEditFormChanged();

            if (wasChanged == true)
            {
                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                if (result == "Yes")
                {
                    ClearPreviewData();
                    ChangeToPreviewMode();
                }
                else
                {
                    // Stay where you are.
                }
            }
            else
            {
                ChangeToPreviewMode();
            }
        }
        #endregion
        #endregion

        [ObservableProperty]
        [Required]
        private string _add_SelectedName;

        partial void OnAdd_SelectedNameChanged(string value)
        {
            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedName));
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearSelectedDateCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectCommand))]
        private DateTimeOffset _add_SelectedDate = DateTime.Now.Date;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearDescriptionCommand))]
        [NotifyCanExecuteChangedFor(nameof(Add_ClearProjectCommand))]
        private string _add_SelectedDescription;

        partial void OnAdd_SelectedDescriptionChanged(string value)
        {
            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedDescription));
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "HasErrors" || e.PropertyName == nameof(Add_CurrentAddImage) || e.PropertyName == nameof(Add_SelectedName) || e.PropertyName == nameof(Add_SelectedDescription) || e.PropertyName == nameof(Add_SelectedDate) || e.PropertyName == nameof(ProjectComponentsSource))
            {
                Add_ClearProjectCommand.NotifyCanExecuteChanged();
                Add_AddProjectCommand.NotifyCanExecuteChanged();
            }
            Evaluate_AddTabVisibilty();
        }

        [RelayCommand(CanExecute = nameof(Add_CanAddProject))]
        public async void Add_AddProject()
        {
            try
            {
                User loggedInUser = DatabaseStore.UsersStore.LoggedInUser;
                if (loggedInUser == null)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("User needs to be logged in to execute this operation", Icon.Error);
                    return;
                }

                string name = Add_Name as string;
                string description = Add_SelectedDescription as string;
                DateTime date = Add_SelectedDate.DateTime;
                Bitmap image = Add_CurrentAddImage;
                Project newProject = new Project(0, loggedInUser.ID, loggedInUser, name, description, date, image);

                Project projectFromDB = await DatabaseStore.ProjectStore.InsertNewProject(newProject);
                if (projectFromDB == null)
                {
                    string buttonResult = await MsBoxService.DisplayMessageBox("There was an error while creating project, try again.", Icon.Error);
                    return;
                }

                IEnumerable<ProjectComponent> projectComponents = ProjectComponentsSource.Select(x => new ProjectComponent(0, projectFromDB.ID, x.ComponentID, x.ComponentFromDBRefrence.Component, x.Used));
                bool addedToDb = await DatabaseStore.ProjectStore.InsertProjectComponentsToProject(projectFromDB, projectComponents);

                if (addedToDb == true)
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("Project added successfully! Do you want to add another project?", Icon.Question);
                    Add_ClearProject();
                    if (dialogResult == "No")
                    {
                        NavigateTab(ComponentTab.Components);
                    }
                }
                else
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("There was an error while adding project. Try again or contact administrator!", Icon.Error);
                }
            }
            catch (Exception exception)
            {

            }
        }

        private bool Add_CanAddProject()
        {
            bool nameProvided = Add_CanClearName();
            bool descriptionProvided = Add_CanClearDescription();
            bool componentsAdded = ProjectComponentsSource.Count > 0;

            return nameProvided && descriptionProvided && componentsAdded;
        }

        [RelayCommand(CanExecute = nameof(Add_CanClearProject))]
        public void Add_ClearProject()
        {
            ProjectComponentsSource.Clear();
            RefreshProjectComponents();

            Add_Name = string.Empty;
            Add_SelectedDate = DateTime.Now.Date;
            Add_SelectedDescription = string.Empty;
        }

        private bool Add_CanClearProject()
        {
            bool imageChanged = Add_CanClearImage();
            bool nameProvided = Add_CanClearName();
            bool dateProvided = Add_CanClearSelectedDate();
            bool descriptionProvided = Add_CanClearDescription();
            bool componentsAdded = ProjectComponentsSource.Count > 0;

            return imageChanged || nameProvided || dateProvided || descriptionProvided || componentsAdded;
        }

        [ObservableProperty]
        private Bitmap _currentAddPredefinedImage;

        [RelayCommand]
        private void ClearImage()
        {
            CurrentAddPredefinedImage = null;
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

        public void RefreshProjectComponents()
        {
            try
            {
                ProjectComponents.Refresh();
                PurchasedComponents.Refresh();
            }
            catch(Exception exception)
            {
                // Handled
            }
        }

        public void RemoveComponentFromProject(ProjectPurchaseComponentHolder componentHolder)
        {
            componentHolder.ClearUsage();
            ProjectComponentsSource.Remove(componentHolder);
            RefreshProjectComponents();
        }

        public void AddComponentToProject(PurchaseComponentHolder avaiableComponent)
        {
            ProjectPurchaseComponentHolder found = ProjectComponentsSource.FirstOrDefault(x => x.ComponentID == avaiableComponent.ComponentID);
            if (found != null)
            {
                //found.Used++;
                //found.ComponentFromDBRefrence.Used++;
            }
            else
            {
                ProjectPurchaseComponentHolder newProjectComponent = new ProjectPurchaseComponentHolder(this, avaiableComponent);
                ProjectComponentsSource.Add(newProjectComponent);
               // avaiableComponent.RegisterProjectsComponent(newProjectComponent);

                newProjectComponent.Used++;
                //component.Used++;
            }
            RefreshProjectComponents();
        }
       
        
        public List<PurchaseComponentHolder> PurchasedComponentsSource {  get; set; }
        public DataGridCollectionView PurchasedComponents { get; set; }

        [ObservableProperty]
        private bool _previewEnabled = false;

        [ObservableProperty]
        private int _selectedTab;

        [ObservableProperty]
        private ProjectContainerHolder _clickedProject;

        partial void OnClickedProjectChanging(ProjectContainerHolder value)
        {
            if(value != null)
            {
                PreviewEnabled = true;
            }
            else
            {
                PreviewEnabled = false;
            }
        }

        public void CollectionProjectClickedCallback(ProjectContainerHolder project)
        {
            ClickedProject = project;
            NavigateTab(ComponentTab.Preview);
        }

        [ObservableProperty]
        private string _collection_TextInput = string.Empty;

        partial void OnCollection_TextInputChanged(string value)
        {
           // RefreshProjectsDataView();
        }
        private NavParam navParam;

        [ObservableProperty]
        private int _collection_Rows;
        public List<ProjectPurchaseComponentHolder> ProjectComponentsSource { get; set; }
        public DataGridCollectionView ProjectComponents { get; set; }
        public ProjectsPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, ApplicationConfig appConfig) : base(defaultRootPageViewModel, databaseStore, messageBoxService, appConfig)
        {
            #region Projects tab pagination
            _projectsService = new ProjectHolderService(this, DatabaseStore.ProjectStore);

            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPageIndex, SelectedPageSize));

            var nameFilter = this.WhenValueChanged(t => t.Projects_NameFilter)
                .Select(NameFilter);

            var descriptionFilter = this.WhenValueChanged(t => t.Projects_DescFilter)
                .Select(DescriptionFilter);

            var dateFromFilter = this.WhenValueChanged(t => t.Projects_DateFromFilter)
                .Select(FromDateFilter);

            var dateToFilter = this.WhenValueChanged(t => t.Projects_DateToFilter)
                .Select(ToDateFilter);

            _projectsService.EmployeesConnection()
                .Filter(nameFilter)
                .Filter(descriptionFilter)
                .Filter(dateFromFilter)
                .Filter(dateToFilter)
                .Sort(SortExpressionComparer<ProjectContainerHolder>.Descending(e => e.Project.CreatedAt))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .ObserveOn(Scheduler.CurrentThread) // Marshals to the current thread (often used for UI updates)
                .Bind(out _projects)
                .Subscribe();

            _projectsService.DataLoaded += _projectsService_DataLoadedHandler;


            _projectsService.LoadData();
            #endregion
            _defaultImage = ImageConverterUtility.BytesToBitmap(File.ReadAllBytes("D:\\Repo\\ElectronDepot\\ElectroDepot\\DesktopClient\\Assets\\DefaultProjectImage.png"));
            Add_CurrentAddImage = _defaultImage;

            DatabaseStore.CategorieStore.ReloadCategoriesData();

            #region Add tab - Projects components
            ProjectComponentsSource = new List<ProjectPurchaseComponentHolder>();
            ProjectComponents = new DataGridCollectionView(ProjectComponentsSource);
            ProjectComponents.PropertyChanged += ProjectComponents_PropertyChangedHandler;
            ProjectComponents.Filter = ((object arg) =>
            {
                if (arg is ProjectPurchaseComponentHolder component)
                {
                    bool hasName = true;
                    bool hasManufacturer = true;

                    if (Add_ProjectComponentsNameFilter != null && Add_ProjectComponentsNameFilter != string.Empty)
                    {
                        if (!component.Name.Contains(Add_ProjectComponentsNameFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            hasName = false;
                        }
                    }

                    if (Add_ProjectComponentsManufacturerFilter != null && Add_ProjectComponentsManufacturerFilter != string.Empty)
                    {
                        if (!component.Manufacturer.Contains(Add_ProjectComponentsManufacturerFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            hasManufacturer = false;
                        }
                    }

                    bool result = hasName & hasManufacturer;

                    return result;
                }
                else
                {
                    return false;
                }
            });
            #endregion
            #region Preview tab - Projects components
            Preview_ProjectComponentsSource = new List<ProjectComponentHolder>();
            Preview_ProjectComponents = new DataGridCollectionView(Preview_ProjectComponentsSource);
            Preview_ProjectComponents.Filter = ((object arg) =>
            {
                if (arg is ProjectComponentHolder component)
                {
                    bool hasName = true;
                    bool hasManufacturer = true;

                    if (Preview_ProjectComponentsNameFilter != null && Preview_ProjectComponentsNameFilter != string.Empty)
                    {
                        if (!component.Name.Contains(Preview_ProjectComponentsNameFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            hasName = false;
                        }
                    }

                    if (Preview_ProjectComponentsManufacturerFilter != null && Preview_ProjectComponentsManufacturerFilter != string.Empty)
                    {
                        if (!component.Manufacturer.Contains(Preview_ProjectComponentsManufacturerFilter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            hasManufacturer = false;
                        }
                    }

                    bool result = hasName & hasManufacturer;

                    return result;
                }
                else
                {
                    return false;
                }
            });
            #endregion

            PurchasedComponentsSource = new List<PurchaseComponentHolder>();
            PurchasedComponents = new DataGridCollectionView(PurchasedComponentsSource);
            PurchasedComponents.Filter = ((object arg) =>
            {
                if(arg is PurchaseComponentHolder component && component.AvaiableInSystem > 0)
                {
                        bool hasName = true;
                        bool hasManufacturer = true;

                        if(Add_AvailableComponentsNameFilter != null && Add_AvailableComponentsNameFilter != string.Empty)
                        {
                            if(!component.Component.Name.Contains(Add_AvailableComponentsNameFilter, StringComparison.InvariantCultureIgnoreCase))
                            {
                                hasName = false;
                            }
                        }

                        if (Add_AvailableComponentsManufacturerFilter != null && Add_AvailableComponentsManufacturerFilter != string.Empty)
                        {
                            if (!component.Component.Manufacturer.Contains(Add_AvailableComponentsManufacturerFilter, StringComparison.InvariantCultureIgnoreCase))
                            {
                                hasManufacturer = false;
                            }
                        }


                        bool result = hasName & hasManufacturer;

                        return result;
                }
                else
                {
                    return false;
                }
            });

            DatabaseStore.ComponentStore.ComponentsLoaded += ComponentStore_ComponentsLoadedHandler;
            DatabaseStore.ComponentStore.ComponentsReloadNotNecessary += ComponentStore_ComponentsLoadedHandler;
            DatabaseStore.ComponentStore.ReloadComponentsData();

            Evaluate_AddTabVisibilty();
        }

        private void NavigationGoToPreviewHandler()
        {
            ProjectsCollection_SelectedItem = ProjectsCollection.First(x=>x.Project.ID == (navParam.Payload as Project).ID);
            _projectsService.DataLoaded -= NavigationGoToPreviewHandler;
            NavigateTab(ComponentTab.Preview);
        }

        private async void _projectsService_DataLoadedHandler()
        {
            Projects_DateMaxYear = _projectsService.MaxYear();
            Projects_DateMinYear = _projectsService.MinYear();
            Projects_DateFromFilter = Projects_DateMinYear;
            Projects_DateToFilter = Projects_DateMaxYear;
        }

        private async void ProjectComponents_PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProjectComponentsSource));
        }

        private async void ComponentStore_ComponentsLoadedHandler()
        {
            PurchasedComponentsSource.Clear();
            IEnumerable<OwnsComponent> unusedComponentsFromDB = DatabaseStore.ComponentStore.UnusedComponents;
            foreach(OwnsComponent component in unusedComponentsFromDB)
            {
                ElectroDepotClassLibrary.Models.Component componentFromDB = DatabaseStore.ComponentStore.Components.FirstOrDefault(x=>x.ID == component.ComponentID);
                Category categoryFromDB = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x => x.ID == componentFromDB.CategoryID);
                PurchasedComponentsSource.Add(new PurchaseComponentHolder(this, componentFromDB, component, categoryFromDB));
            }
            RefreshPurchasedComponents();
        }

        public void RefreshPurchasedComponents()
        {
            try
            {
                PurchasedComponents.Refresh();
            }
            catch(Exception ex)
            {
                // Handled;
            }
        }

        public void CalculateColumns()
        {
            const int width = 200; // TODO: Raw dog imeplementation, in future check for with of item in collection.
            const int windowWidth = 1400; //TODO: get current width

            int res = (windowWidth / width);

            //int itemCount = ProjectsDataView.Count;
            
            //Collection_Rows = itemCount > 0 ? Math.Min(itemCount, 4) : 1;
            Collection_Rows = res;
        }

        //public void RefreshProjectsDataView()
        //{
        //    try
        //    {
        //        ProjectsDataView.Refresh();
        //    }
        //    catch (Exception exception)
        //    {
        //        // Handled
        //    }
        //    CalculateColumns();
        //}

        public void InterpreteNavigationParameter(NavParam navigationParameter)
        {
            switch (navigationParameter.Operation)
            {
                case NavOperation.Add:
                    NavigateTab(ComponentTab.Add);
                    break;
                case NavOperation.Preview:
                    navParam = navigationParameter;
                    Projects_NameFilter = (navigationParameter.Payload as Project).Name;
                    _projectsService.DataLoaded += NavigationGoToPreviewHandler;
                    break;
                default:
                    break;
            }
        }
    }
}
