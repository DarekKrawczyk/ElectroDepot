using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class SuppliersStore : RootStore
    {
        private readonly SupplierDataProvider _supplierDataProvider;
        private List<Supplier> _suppliers;

        public IEnumerable<Supplier> Suppliers
        {
            get
            {
                return _suppliers;
            }
        }

        public SupplierDataProvider SupplierDP { get { return _supplierDataProvider; } }

        public event Action SuppliersLoaded;

        public SuppliersStore(DatabaseStore dbStore, SupplierDataProvider supplierDataProvider) : base(dbStore)
        {
            _supplierDataProvider = supplierDataProvider;
            _suppliers = new List<Supplier>();
        }

        public async Task Load()
        {
            _suppliers.Clear();

            IEnumerable<Supplier> suppliersFromDB = await _supplierDataProvider.GetAllSuppliers();
            _suppliers.AddRange(suppliersFromDB);

            SuppliersLoaded?.Invoke();
        }
    }
}
