using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Utility;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests
{
    public class BaseDataProviderTest
    {
        public ITestOutputHelper Console { get; }
        public ComponentDataProvider ComponentDP { get; }
        public CategoryDataProvider CategoryDP { get; }
        public UserDataProvider UserDP { get; }
        public OwnsComponentDataProvider OwnsComponentDP{ get; }
        public ProjectDataProvider ProjectDP { get; }
        public ProjectComponentDataProvider ProjectComponentDP { get; }
        public SupplierDataProvider SupplierDP { get; }
        public PurchaseDataProvider PurchaseDP { get; }
        public PurchaseItemDataProvider PurchaseItemDP { get; }

        public BaseDataProviderTest(ITestOutputHelper output)
        {
            Console = output;

            ApplicationConfig config = ApplicationConfig.Create();
            config.LoadConfig();

            ComponentDP = new ComponentDataProvider(config.ServerConfig.ConnectionURL);
            CategoryDP = new CategoryDataProvider(config.ServerConfig.ConnectionURL);
            UserDP = new UserDataProvider(config.ServerConfig.ConnectionURL);
            OwnsComponentDP = new OwnsComponentDataProvider(config.ServerConfig.ConnectionURL);
            ProjectDP = new ProjectDataProvider(config.ServerConfig.ConnectionURL);
            ProjectComponentDP = new ProjectComponentDataProvider(config.ServerConfig.ConnectionURL);
            SupplierDP = new SupplierDataProvider(config.ServerConfig.ConnectionURL);
            PurchaseDP = new PurchaseDataProvider(config.ServerConfig.ConnectionURL);
            PurchaseItemDP = new PurchaseItemDataProvider(config.ServerConfig.ConnectionURL);
        }
    }
}
