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
        public event Action SuppliersReloadNotNecessary;

        public SuppliersStore(DatabaseStore dbStore, SupplierDataProvider supplierDataProvider) : base(dbStore)
        {
            _supplierDataProvider = supplierDataProvider;
            _suppliers = new List<Supplier>();
        }

        private async Task Load()
        {
            _suppliers.Clear();

            IEnumerable<Supplier> suppliersFromDB = await _supplierDataProvider.GetAllSuppliers();
            _suppliers.AddRange(suppliersFromDB);

            SuppliersLoaded?.Invoke();
        }

        public async Task ReloadSuppliersData()
        {
            bool requiredReload = false;

            IEnumerable<Supplier> suppliersFromDB = await _supplierDataProvider.GetAllSuppliers();

            if (suppliersFromDB.Count() == _suppliers.Count)
            {
                for (int i = 0; i < _suppliers.Count; i++)
                {
                    if (suppliersFromDB.ElementAt(i).Compate(_suppliers[i]))
                    {
                        requiredReload = true;
                        break;
                    }
                }
            }
            else
            {
                requiredReload = true;
            }

            if (requiredReload == true)
            {
                _suppliers.Clear();
                _suppliers.AddRange(suppliersFromDB);

                SuppliersLoaded?.Invoke();
            }
            else
            {
                SuppliersReloadNotNecessary?.Invoke();
            }
        }
    }
}
