using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests
{
    public class BaseDBStoreTestset
    {
        public ITestOutputHelper Console { get; }
        public DatabaseStore DatabaseStore { get; }
        public BaseDBStoreTestset(ITestOutputHelper console)
        {
            ApplicationConfig config = new ApplicationConfig();
            config.LoadConfig();

            SupplierDataProvider supplierDataProvider = new SupplierDataProvider(config.ServerConfig.ConnectionURL);
            ComponentDataProvider componentDataProvider = new ComponentDataProvider(config.ServerConfig.ConnectionURL);
            CategoryDataProvider categoryDataProvider = new CategoryDataProvider(config.ServerConfig.ConnectionURL);
            ProjectDataProvider projectDataProvider = new ProjectDataProvider(config.ServerConfig.ConnectionURL);
            PurchaseDataProvider purchaseDataProvider = new PurchaseDataProvider(config.ServerConfig.ConnectionURL);
            UserDataProvider usersDataProvider = new UserDataProvider(config.ServerConfig.ConnectionURL);
            OwnsComponentDataProvider ownsComponentDataProvider = new OwnsComponentDataProvider(config.ServerConfig.ConnectionURL);
            ProjectComponentDataProvider projectComponentDataProvider = new ProjectComponentDataProvider(config.ServerConfig.ConnectionURL);
            PurchaseItemDataProvider purchaseItemDataProvider = new PurchaseItemDataProvider(config.ServerConfig.ConnectionURL);
            PredefinedImageDataProvider predefinedImageDataProvider = new PredefinedImageDataProvider(config.ServerConfig.ConnectionURL);
            DatabaseStore = new DatabaseStore(supplierDataProvider,
                componentDataProvider,
                categoryDataProvider,
                projectDataProvider,
                purchaseDataProvider,
                usersDataProvider,
                ownsComponentDataProvider,
                projectComponentDataProvider,
                purchaseItemDataProvider,
                predefinedImageDataProvider);
        }
    }
}
