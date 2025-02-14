using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class CategoriesStore : RootStore
    {
        private readonly CategoryDataProvider _categoryDataProvider;
        private List<Category> _categories;

        public IEnumerable<Category> Categories { get { return _categories; } }
        public CategoryDataProvider DB { get { return _categoryDataProvider; } }

        public event Action CategoriesLoaded;
        public event Action CategoriesReloadNotNecessary;
        
        public CategoriesStore(DatabaseStore dbStore, CategoryDataProvider categoryDataProvider) : base(dbStore)
        {
            _categoryDataProvider = categoryDataProvider;
            _categories = new List<Category>();
        }

        private async Task Load()
        {
            _categories.Clear();

            IEnumerable<Category> categoriesFromDB = await _categoryDataProvider.GetAllCategories();
            _categories.AddRange(categoriesFromDB);

            CategoriesLoaded?.Invoke();
        }

        public async Task ReloadCategoriesData()
        {
            bool requiredReload = false;    

            IEnumerable<Category> categoriesFromDB = await _categoryDataProvider.GetAllCategories();
            
            if(categoriesFromDB.Count() == _categories.Count)
            {
                for(int i = 0; i < _categories.Count; i++)
                {
                    if (categoriesFromDB.ElementAt(i).ID != _categories[i].ID)
                    {
                        requiredReload = true;
                    }
                }
            }
            else
            {
                requiredReload = true;
            }
            
            if(requiredReload == true)
            {
                _categories.Clear();
                _categories.AddRange(categoriesFromDB);

                CategoriesLoaded?.Invoke();
            }
            else
            {
                CategoriesReloadNotNecessary?.Invoke();
            }
        }
    }
}
