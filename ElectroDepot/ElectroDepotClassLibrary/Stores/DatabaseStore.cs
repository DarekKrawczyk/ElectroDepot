using ElectroDepotClassLibrary.DataProviders;

namespace ElectroDepotClassLibrary.Stores
{
    public class DatabaseStore
    {
        private SuppliersStore _supplierStore;
        private ComponentsStore _componentsStore;
        private CategoriesStore _categoriesStore;
        private ProjectsStore _projectsStore;
        private PurchasesStore _purchasesStore;
        private UsersStore _usersStore;
        private PredefinedImagesStore _predefinedImagesStore;
        
        public PredefinedImagesStore PredefinedImagesStore
        {
            get
            {
                return _predefinedImagesStore;
            }
        }

        public SuppliersStore SupplierStore
        {
            get
            {
                return _supplierStore;
            }
        }

        public ComponentsStore ComponentStore
        {
            get
            {
                return _componentsStore;
            }
        }

        public CategoriesStore CategorieStore
        {
            get
            {
                return _categoriesStore;
            }
        }

        public ProjectsStore ProjectStore
        {
            get
            {
                return _projectsStore;
            }
        }

        public PurchasesStore PurchaseStore
        {
            get
            {
                return _purchasesStore;
            }
        }

        public UsersStore UsersStore
        {
            get
            {
                return _usersStore;
            }
        }

        public DatabaseStore(
            SupplierDataProvider supplierDataProvider, ComponentDataProvider componentDataProvider, 
            CategoryDataProvider categoryDataProvider, ProjectDataProvider projectDataProvider,
            PurchaseDataProvider purchaseDataProvicer, UserDataProvider userDataProvider,
            OwnsComponentDataProvider ownsComponentDataProvider, ProjectComponentDataProvider projectComponentDataProvider,
            PurchaseItemDataProvider purchaseItemDataProvider, PredefinedImageDataProvider predefinedImageDataProvider)
        {
            _supplierStore = new SuppliersStore(this, supplierDataProvider);
            _componentsStore = new ComponentsStore(this, componentDataProvider, ownsComponentDataProvider);
            _categoriesStore = new CategoriesStore(this, categoryDataProvider);
            _projectsStore = new ProjectsStore(this, projectDataProvider, projectComponentDataProvider);
            _purchasesStore = new PurchasesStore(this, purchaseDataProvicer, purchaseItemDataProvider);
            _usersStore = new UsersStore(this, userDataProvider);
            _predefinedImagesStore = new PredefinedImagesStore(this, predefinedImageDataProvider);
        }
    }
}
