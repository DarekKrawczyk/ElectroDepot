using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Stores;
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
        public string ConnectionURL = Utility.ConnectionURL;
        public ITestOutputHelper Console { get; }
        public DatabaseStore DatabaseStore { get; }
        public BaseDBStoreTestset(ITestOutputHelper console)
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
