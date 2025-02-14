using Avalonia.OpenGL;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class ComponentsStore : RootStore
    {
        private readonly ComponentDataProvider _componentDataProvider;
        private readonly OwnsComponentDataProvider _ownsComponentDataProvider;
        private List<Component> _components;
        private List<DetailedComponentContainer> _allComponents; 
        private List<ComponentWithCategoryContainer> _componentsFromSystem;
        private List<OwnsComponent> _ownedComponents;
        private List<OwnsComponent> _unusedComponents;

        public IEnumerable<ComponentWithCategoryContainer> ComponentsFromSystem { get { return _componentsFromSystem; } }
        public IEnumerable<Component> Components { get { return _components; } }
        public IEnumerable<DetailedComponentContainer> AllComponents { get { return _allComponents; } }
        public IEnumerable<OwnsComponent> OwnedComponents { get { return _ownedComponents; } }
        public IEnumerable<OwnsComponent> UnusedComponents { get { return _unusedComponents; } }
        public ComponentDataProvider ComponentDP { get { return _componentDataProvider; } }
        public OwnsComponentDataProvider OwnsComponentDP { get { return _ownsComponentDataProvider; } }

        public event Action AllComponentsReload;
        public event Action AllComponentsReloadNotNecessary;
        public event Action ComponentsLoaded;
        public event Action ComponentsReloadNotNecessary;
        public event Action ComponentsFromSystemLoaded;

        public ComponentsStore(DatabaseStore dbStore, ComponentDataProvider componentDataProvider, OwnsComponentDataProvider ownsComponentDataProvider) : base(dbStore)
        {
            _componentDataProvider = componentDataProvider;
            _ownsComponentDataProvider = ownsComponentDataProvider;
            _allComponents = new List<DetailedComponentContainer>();
            _components = new List<Component>();
            _componentsFromSystem = new List<ComponentWithCategoryContainer>();
            _ownedComponents = new List<OwnsComponent>();
            _unusedComponents = new List<OwnsComponent>();
            // TODO: OwnsComponent model requires implementation of User model just like the rest of Models....
        }

        public async Task<Component> UpdateComponent(Component component)
        {
            Component result = await ComponentDP.UpdateComponent(component);
            if (result != null)
            {
                int index = _components.FindIndex(item => item.ID == component.ID);

                if (index != -1)
                {
                    _components[index].ReplaceWith(result);
                }
                ComponentsLoaded?.Invoke();
            }
            return result;
        }

        public async Task<bool> InsertNewComponent(Component component)
        {
            Component componentFromDB = await ComponentDP.CreateComponent(component);

            if (componentFromDB == null)
            {
                return false;
            }

            OwnsComponent ownsComponent = new OwnsComponent(id: 0, userID: MainStore.UsersStore.LoggedInUser.ID, componentID: componentFromDB.ID, quantity: 0);

            OwnsComponent ownsComponentFromDB = await OwnsComponentDP.CreateOwnComponent(ownsComponent);

            if (ownsComponentFromDB == null)
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
            foreach (Component component in componentsFromDB)
            {
                Category cat = categories.FirstOrDefault(c => c.ID == component.CategoryID);
                _componentsFromSystem.Add(new ComponentWithCategoryContainer(component, cat));
            }

            ComponentsFromSystemLoaded?.Invoke();
        }

        public async Task ReloadAllComponentsData()
        {
            bool reloadRequired = false;

            User loggedInUser = MainStore.UsersStore.LoggedInUser;
            if (loggedInUser == null) throw new Exception("User not logged in!!");

            IEnumerable<Component> componentsFromDB = await ComponentDP.GetAllComponents();
            IEnumerable<OwnsComponent> ownedComponentsFromDB = await OwnsComponentDP.GetAllOwnsComponentsFromUser(loggedInUser);
            IEnumerable<Project> usersProjectFromDB = await MainStore.ProjectStore.ProjectDP.GetAllProjectOfUser(loggedInUser);
            
            List<ProjectComponent> projectComponentsFromDB = new();
            List<DetailedComponentContainer> allComponentsFromDB = new();

            foreach (Project project in usersProjectFromDB)
            {
                projectComponentsFromDB.AddRange(await MainStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfProject(project));
            }

            for (int i = 0; i < componentsFromDB.Count(); i++)
            {
                Component component = componentsFromDB.ElementAt(i);
                Category category = MainStore.CategorieStore.Categories.FirstOrDefault(x => x.ID == component.CategoryID);

                component.Category = category;

                OwnsComponent ownsComponent = ownedComponentsFromDB.FirstOrDefault(x=>x.ComponentID == component.ID);
                int quantityInProject = 0;

                if (ownsComponent == null)
                {
                    ownsComponent = new OwnsComponent(0, loggedInUser.ID, component.ID, quantityInProject);
                }
                else
                {
                    quantityInProject = projectComponentsFromDB.Where(x => x.ComponentID == ownsComponent.ComponentID).Sum(x => x.Quantity);

                    OwnsComponent ownsComponentCopy = new OwnsComponent(ownsComponent);
                    ownsComponentCopy.Quantity -= quantityInProject;

                    ownsComponent = ownsComponentCopy;
                }

                DetailedComponentContainer componentContainer = new DetailedComponentContainer(component, ownsComponent, quantityInProject);
                allComponentsFromDB.Add(componentContainer);
            }

            if(_allComponents.Count == allComponentsFromDB.Count)
            {
                for(int i = 0; i < allComponentsFromDB.Count; i++)
                {
                    Component component = allComponentsFromDB[i].Component;
                    Component other = _allComponents[i].Component;

                    bool isSame = component.Compare(other);

                    if(isSame == false)
                    {
                        reloadRequired = true;
                        break;
                    }
                }
            }
            else
            {
                reloadRequired = true;
            }

            if(reloadRequired == true)
            {
                _allComponents.Clear();
                _allComponents.AddRange(allComponentsFromDB);

                AllComponentsReload?.Invoke();
            }
            else
            {
                AllComponentsReloadNotNecessary?.Invoke();
            }

        }

        public async Task ReloadComponentsData()
        {
            bool reloadRequired = false;

            User loggedInUser = MainStore.UsersStore.LoggedInUser;
            if (loggedInUser == null) throw new Exception("User not logged in!!");

            IEnumerable<OwnsComponent> ownedComponentsFromDB = await OwnsComponentDP.GetAllOwnsComponentsFromUser(loggedInUser);
            IEnumerable<Component> componentsFromDB = await ComponentDP.GetAllAvailableComponentsFromUserWithImage(loggedInUser);
            IEnumerable<Project> usersProjectFromDB = await MainStore.ProjectStore.ProjectDP.GetAllProjectOfUser(loggedInUser);
            List<ProjectComponent> projectComponentsFromDB = new();
            List<OwnsComponent> unusedComponents = new();

            foreach (Project project in usersProjectFromDB)
            {
                projectComponentsFromDB.AddRange(await MainStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfProject(project));
            }

            for (int i = 0; i < ownedComponentsFromDB.Count(); i++)
            {
                OwnsComponent oCMP = ownedComponentsFromDB.ElementAt(i);

                int quantityInProject = projectComponentsFromDB.Where(x => x.ComponentID == oCMP.ComponentID).Sum(x => x.Quantity);

                OwnsComponent unusedComponent = new OwnsComponent(oCMP);
                unusedComponent.Quantity -= quantityInProject;

                unusedComponents.Add(unusedComponent);
            }

            _ownedComponents.Clear();
            _components.Clear();
            _unusedComponents.Clear();

            _ownedComponents.AddRange(ownedComponentsFromDB);
            _components.AddRange(componentsFromDB);
            _unusedComponents.AddRange(unusedComponents);

            ComponentsLoaded?.Invoke();
        }
    }
}
