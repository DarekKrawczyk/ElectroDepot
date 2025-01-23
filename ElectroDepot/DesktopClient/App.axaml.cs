using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DesktopClient.Navigation;
using DesktopClient.Services;
using DesktopClient.ViewModels;
using DesktopClient.Views;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient
{
    public partial class App : Application
    {
        public string ConnectionURL { get { return "https://localhost:7146/"; } }
        private DatabaseStore _databaseStore;
        public override void Initialize()
        {
            SupplierDataProvider supplierDataProvider = new SupplierDataProvider(ConnectionURL);
            ComponentDataProvider componentDataProvider = new ComponentDataProvider(ConnectionURL);
            CategoryDataProvider categoryDataProvider = new CategoryDataProvider(ConnectionURL);
            ProjectDataProvider projectDataProvider = new ProjectDataProvider(ConnectionURL);
            PurchaseDataProvider purchaseDataProvider = new PurchaseDataProvider(ConnectionURL);
            UserDataProvider usersDataProvider = new UserDataProvider(ConnectionURL);
            OwnsComponentDataProvider ownsComponentDataProvider = new OwnsComponentDataProvider(ConnectionURL);
            ProjectComponentDataProvider projectComponentDataProvider = new ProjectComponentDataProvider(ConnectionURL);
            PurchaseItemDataProvider purchaseItemDataProvider = new PurchaseItemDataProvider(ConnectionURL);
            PredefinedImageDataProvider predefinedImageDataProvider = new PredefinedImageDataProvider(ConnectionURL);
            _databaseStore = new DatabaseStore(supplierDataProvider, 
                componentDataProvider, 
                categoryDataProvider, 
                projectDataProvider, 
                purchaseDataProvider,
                usersDataProvider,
                ownsComponentDataProvider,
                projectComponentDataProvider,
                purchaseItemDataProvider,
                predefinedImageDataProvider);
            // TODO: Implement login, for now this will do
            
            


            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);

                MessageBoxService _msgService = new MessageBoxService();

                _databaseStore.UsersStore.UserLogin("username", "password"); // While using  async on this method, windows doesn't show up!

                //Navigator navigator = new Navigator(_databaseStore);
                MainWindowViewModel windowViewModel = new MainWindowViewModel(null, _databaseStore, _msgService);

                HomePageViewModel home = new HomePageViewModel(null, _databaseStore, _msgService);
                RootPageViewModel root = new RootPageViewModel(windowViewModel, _databaseStore, _msgService);
                home.Root = root;

                root.PageView = home;

                windowViewModel.View = root;

                //MainWindowViewModel window = new MainWindowViewModel(_databaseStore, root);
                //navigator.Window = window;

                desktop.MainWindow = new MainWindow
                {
                    //DataContext = new LoginPageViewModel(),

                    DataContext = windowViewModel,
                };
                _msgService.SetWindow(desktop.MainWindow);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}