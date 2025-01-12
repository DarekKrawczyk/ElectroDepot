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
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class ProjectsPageViewModel : ViewModelBase
    {
        private bool _hasUserInteracterWithName = false;
        private bool _hasUserInteracterWithDescription = false;

        [ObservableProperty]
        [Required]
        private string _add_SelectedName;

        partial void OnAdd_SelectedNameChanged(string value)
        {
            _hasUserInteracterWithName = true;
            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedName));
            }
        }

        [ObservableProperty]
        [Required(ErrorMessage = "You need to provide name!")]
        private DateTimeOffset _add_SelectedDate = DateTime.Now;

        [ObservableProperty]
        [Required(ErrorMessage = "You need to provide description!")]
        private string _add_SelectedDescription;

        partial void OnAdd_SelectedDescriptionChanged(string value)
        {
            _hasUserInteracterWithDescription = true;
            if (value != null)
            {
                ValidateProperty(value, nameof(Add_SelectedDescription));
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "HasErrors" || e.PropertyName == nameof(Add_SelectedName) || e.PropertyName == nameof(Add_SelectedDescription) || e.PropertyName == nameof(Add_SelectedDate) || e.PropertyName == nameof(ProjectComponentsSource))
            {
                if (_hasUserInteracterWithName == false ||
                    _hasUserInteracterWithDescription == false ||
                    ProjectComponentsSource.Count == 0)
                {
                    Add_CanAddProject = false;
                }
                else
                {
                    Add_CanAddProject = !HasErrors;
                }
            }
        }

        [ObservableProperty]
        private bool _add_CanAddProject;

        [RelayCommand]
        public async void Add_AddProject()
        {
            Add_CanAddProject = false;
            try
            {
                User loggedInUser = DatabaseStore.UsersStore.LoggedInUser;
                if (loggedInUser == null)
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "User needs to be logged in to execute this operation", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                    return;
                }

                string name = Add_SelectedName as string;
                string description = Add_SelectedDescription as string;
                DateTime date = Add_SelectedDate.Date;
                Bitmap image = CurrentAddPredefinedImage;
                Project newProject = new Project(0, loggedInUser.ID, loggedInUser, name, description, date, image);

                Project projectFromDB = await DatabaseStore.ProjectStore.InsertNewProject(newProject);
                if(projectFromDB == null)
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "There was an error while creating project, try again.", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                    return;
                }

                IEnumerable<ProjectComponent> projectComponents = ProjectComponentsSource.Select(x => new ProjectComponent(0, projectFromDB.ID, x.ComponentID, x.ComponentFromDBRefrence.Component, x.Used));
                bool addedToDb = await DatabaseStore.ProjectStore.InsertProjectComponentsToProject(projectFromDB, projectComponents);
                
                if (addedToDb == true)
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "Project added successfully", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                    Add_ClearProject();
                    SelectedTab = 0;
                }
                else
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Electro Depot", "Project couldn't be added", ButtonEnum.Ok);
                    ButtonResult buttonResult = await box.ShowAsync();
                }
            }
            catch (Exception exception)
            {

            }

            Add_CanAddProject = true;
        }

        [RelayCommand]
        public void Add_ClearProject()
        {
            ProjectComponentsSource.Clear();
            RefreshProjectComponents();
            _hasUserInteracterWithDescription = false;
            _hasUserInteracterWithName = false;
            Add_SelectedName = null;
            Add_SelectedDate = DateTime.Now;
            Add_SelectedDescription = null;
            _hasUserInteracterWithDescription = false;
            _hasUserInteracterWithName = false;
            Add_CanAddProject = false;
        }

        [ObservableProperty]
        private Bitmap _currentAddPredefinedImage;

        [RelayCommand]
        private void ClearImage()
        {
            CurrentAddPredefinedImage = null;
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

        public void AddComponentToProject(PurchaseComponentHolder component)
        {
            ProjectPurchaseComponentHolder found = ProjectComponentsSource.FirstOrDefault(x => x.ComponentID == component.ComponentID);
            if (found != null)
            {
                found.Used++;
                //found.ComponentFromDBRefrence.Used++;
            }
            else
            {
                ProjectComponentsSource.Add(new ProjectPurchaseComponentHolder(this, component));
                //component.Used++;
            }
            RefreshProjectComponents();
        }

        public List<ProjectPurchaseComponentHolder> ProjectComponentsSource {  get; set; }
        public DataGridCollectionView ProjectComponents { get; set; }
        
        
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
            SelectedTab = 2; // GOTO preview!
        }

        partial void OnSelectedTabChanged(int value)
        {
            switch (value)
            {
                case 0:
                    // Collection
                    ClickedProject = null;
                    break;
                case 1:
                    // Add
                    ClickedProject = null;
                    break;
                case 2:
                    // Preview
                    break;
                default:
                    // undefined
                    break;
            }
        }

        [ObservableProperty]
        private string _collection_TextInput = string.Empty;

        partial void OnCollection_TextInputChanged(string value)
        {
            RefreshProjectsDataView();
        }

        [ObservableProperty]
        private int _collection_Rows;
        public List<ProjectContainerHolder> ProjectsSource { get; set; }
        public DataGridCollectionView ProjectsDataView { get; set; }
        public ProjectsPageViewModel(DatabaseStore databaseStore) : base(databaseStore)
        {

            DatabaseStore.CategorieStore.Load();
            ProjectsSource = new List<ProjectContainerHolder>();
            DatabaseStore.ProjectStore.ProjectsLoaded += ProjectStore_ProjectsLoadedHandler;
            DatabaseStore.ProjectStore.Load();

            ProjectsDataView = new DataGridCollectionView(ProjectsSource);
            ProjectsDataView.Filter = ((object arg) =>
            {
                if(arg is ProjectContainerHolder projectContainer)
                {
                    bool isContained = false;

                    if (Collection_TextInput != null)
                    {
                        if (projectContainer.Project.Name.Contains(Collection_TextInput, StringComparison.InvariantCultureIgnoreCase)  ||
                            projectContainer.Project.Description.Contains(Collection_TextInput, StringComparison.InvariantCultureIgnoreCase))
                        {
                            isContained = true;
                        }
                    }

                    if(isContained == true)
                    {
                        return true;    
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;   
                }
            });

            ProjectComponentsSource = new List<ProjectPurchaseComponentHolder>();
            ProjectComponents = new DataGridCollectionView(ProjectComponentsSource);
            ProjectComponents.PropertyChanged += ProjectComponents_PropertyChangedHandler;
            ProjectComponents.Filter = ((object arg) =>
            {
                if(arg is ProjectPurchaseComponentHolder component)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            PurchasedComponentsSource = new List<PurchaseComponentHolder>();
            PurchasedComponents = new DataGridCollectionView(PurchasedComponentsSource);
            PurchasedComponents.Filter = ((object arg) =>
            {
                if(arg is PurchaseComponentHolder component)
                {
                    if(component.Avaiable > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            });
            DatabaseStore.ComponentStore.ComponentsLoaded += ComponentStore_ComponentsLoadedHandler;
            DatabaseStore.ComponentStore.Load();
        }

        private void ProjectComponents_PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProjectComponentsSource));
        }

        private void ComponentStore_ComponentsLoadedHandler()
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

        private async void ProjectStore_ProjectsLoadedHandler()
        {
            ProjectsSource.Clear();
            IEnumerable<Project> projectsFromDB = DatabaseStore.ProjectStore.Projects;
            foreach(Project project in projectsFromDB)
            {
                IEnumerable<ProjectComponent> projectComponentFromDB = await DatabaseStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfProject(project);
                //IEnumerable<ProjectComponent> projectComponentFromDB = DatabaseStore.ProjectStore.ProjectsComponents.Where(x => x.ProjectID == project.ID);
                List<ProjectComponentHolder> compHolder = new List<ProjectComponentHolder>();
                foreach(ProjectComponent projectComponent in projectComponentFromDB)
                {
                    ElectroDepotClassLibrary.Models.Component componentFromDB = DatabaseStore.ComponentStore.Components.FirstOrDefault(x => x.ID == projectComponent.ComponentID);
                    compHolder.Add(new ProjectComponentHolder(componentFromDB, projectComponent));
                }
                ProjectsSource.Add(new ProjectContainerHolder(this, project, compHolder));
            }
            RefreshProjectsDataView();
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

        public void RefreshProjectsDataView()
        {
            try
            {
                ProjectsDataView.Refresh();
            }
            catch (Exception exception)
            {
                // Handled
            }
            CalculateColumns();
        }

        public override void Dispose()
        {
        }
    }
}
