using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
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
            //_databaseStore.UsersStore.TryLoginUser("test", "test");


            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(_databaseStore),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}