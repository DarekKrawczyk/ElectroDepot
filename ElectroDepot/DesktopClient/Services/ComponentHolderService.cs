using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    internal class ComponentHolderService
    {
        private readonly ComponentsPageViewModel _viewModel;
        private readonly ComponentsStore _componentsStore;
        private readonly SuppliersStore _suppliersStore;
        private readonly ISourceCache<DetailedComponentContainer, int> _components;

        public ComponentHolderService(ComponentsPageViewModel viewModel, ComponentsStore componentsStore)
        {
            _viewModel = viewModel;
            _componentsStore = componentsStore;
            _suppliersStore = componentsStore.MainStore.SupplierStore;
            _components = new SourceCache<DetailedComponentContainer, int>(e => e.ID);
        }

        public IObservable<IChangeSet<DetailedComponentContainer, int>> EmployeesConnection() => _components.Connect();

        public void LoadData()
        {
            _components.Clear();

            IEnumerable<OwnsComponent> ownedComponents = _componentsStore.OwnedComponents;
            IEnumerable<OwnsComponent> unusedComponents = _componentsStore.UnusedComponents;
            IEnumerable<Component> components = _componentsStore.Components;
            IEnumerable<Supplier> suppliers = _suppliersStore.Suppliers;
            ObservableCollection<SupplierContainer> suppliersCol = new ObservableCollection<SupplierContainer>(suppliers.Select(x=> new SupplierContainer(x)));


            for (int i = 0; i < components.Count(); i++)
            {
                Component component = components.ElementAt(i);
                OwnsComponent ownedComponent = ownedComponents.ElementAt(i);
                OwnsComponent unusedComponent = unusedComponents.ElementAt(i);
                string manufacturer = component.Manufacturer;

                _components.AddOrUpdate(new DetailedComponentContainer(_viewModel, component, ownedComponent, unusedComponent, suppliersCol));
            }
        }
    }
}
