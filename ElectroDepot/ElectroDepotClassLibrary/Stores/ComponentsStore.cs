using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ElectroDepotClassLibrary.Stores
{
    public class ComponentsStore : RootStore
    {
        private readonly ComponentDataProvider _componentDataProvider;
        private readonly OwnsComponentDataProvider _ownsComponentDataProvider;
        private List<Component> _components;
        private List<ComponentWithCategoryContainer> _componentsFromSystem;
        private List<OwnsComponent> _ownedComponents;
        private List<OwnsComponent> _unusedComponents;

        public IEnumerable<ComponentWithCategoryContainer> ComponentsFromSystem {  get { return _componentsFromSystem; } }
        public IEnumerable<Component> Components {  get { return _components; } }
        public IEnumerable<OwnsComponent> OwnedComponents {  get { return _ownedComponents; } }
        public IEnumerable<OwnsComponent> UnusedComponents {  get { return _unusedComponents; } }
        
        public ComponentDataProvider ComponentDP { get { return _componentDataProvider; } }
        public OwnsComponentDataProvider OwnsComponentDP { get { return _ownsComponentDataProvider; } }

        public event Action ComponentsLoaded;
        public event Action ComponentsFromSystemLoaded;

        public ComponentsStore(DatabaseStore dbStore, ComponentDataProvider componentDataProvider, OwnsComponentDataProvider ownsComponentDataProvider) : base(dbStore)
        {
            _componentDataProvider = componentDataProvider;
            _ownsComponentDataProvider = ownsComponentDataProvider;
            _components = new List<Component>();
            _componentsFromSystem = new List<ComponentWithCategoryContainer>();
            _ownedComponents = new List<OwnsComponent>();
            _unusedComponents = new List<OwnsComponent>();
            // TODO: OwnsComponent model requires implementation of User model just like the rest of Models....
        }

        public async Task<bool> InsertNewComponent(Component component)
        {


            Component componentFromDB = await ComponentDP.CreateComponent(component);

            if(componentFromDB == null)
            {
                return false;
            }

            OwnsComponent ownsComponent = new OwnsComponent(id: 0, userID: MainStore.UsersStore.LoggedInUser.ID, componentID: componentFromDB.ID, quantity: 0);

            OwnsComponent ownsComponentFromDB = await OwnsComponentDP.CreateOwnComponent(ownsComponent);

            if(ownsComponentFromDB == null)
            {
                return false;
            }

            componentFromDB.ByteImage = component.ByteImage;

            // TODO: Maybe change endpoint to return data if operation was correct? Because we have to have current id's in those lists. For now this must do.
            _components.Add(componentFromDB);
            _ownedComponents.Add(ownsComponentFromDB);
            _unusedComponents.Add(ownsComponentFromDB);

            ComponentsLoaded?.Invoke();

            return true;
        }

        public async Task LoadComponentsOfSystem()
        {
            _componentsFromSystem.Clear();

            IEnumerable<Category> categories = await MainStore.CategorieStore.DB.GetAllCategories();
            IEnumerable<Component> componentsFromDB = await _componentDataProvider.GetAllComponents();
            foreach(Component component in componentsFromDB)
            {
                Category cat = categories.FirstOrDefault(c => c.ID == component.CategoryID);
                _componentsFromSystem.Add(new ComponentWithCategoryContainer(component, cat));
            }

            ComponentsFromSystemLoaded?.Invoke();
        }

        public async Task Load()
        {
            User loggedInUser = MainStore.UsersStore.LoggedInUser;
            if (loggedInUser == null) throw new Exception("User not logged in!!");

            _ownedComponents.Clear();
            _components.Clear();
            _unusedComponents.Clear();

            IEnumerable<OwnsComponent> ownedComponentsFromDB = await OwnsComponentDP.GetAllOwnsComponentsFromUser(loggedInUser);
            IEnumerable<Component> componentsFromDB = await ComponentDP.GetAllAvailableComponentsFromUserWithImage(loggedInUser);
            IEnumerable<OwnsComponent> unusedComponentsFromDB = await OwnsComponentDP.GetAllUnusedComponents(loggedInUser);

            if (ownedComponentsFromDB.Count() != componentsFromDB.Count() || componentsFromDB.Count() != unusedComponentsFromDB.Count()) throw new Exception("Data retrieved from db doesn't match!!!");

            _ownedComponents.AddRange(ownedComponentsFromDB);
            _components.AddRange(componentsFromDB);
            _unusedComponents.AddRange(unusedComponentsFromDB);

            ComponentsLoaded?.Invoke();
        }
    }
}
