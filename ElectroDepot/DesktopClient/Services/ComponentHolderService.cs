using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData;
using ElectroDepotClassLibrary.Containers;
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
        private readonly ISourceCache<DetailedComponentContainerHolder, int> _components;

        public ComponentHolderService(ComponentsPageViewModel viewModel, ComponentsStore componentsStore)
        {
            _viewModel = viewModel;
            _componentsStore = componentsStore;
            _suppliersStore = componentsStore.MainStore.SupplierStore;
            _components = new SourceCache<DetailedComponentContainerHolder, int>(e => e.Container.Component.ID);

            _componentsStore.AllComponentsReload += _componentsStore_ComponentsLoadedHandler;
            _componentsStore.AllComponentsReloadNotNecessary += _componentsStore_ComponentsLoadedHandler;
        }

        private void _componentsStore_ComponentsLoadedHandler()
        {
            _components.Clear();

            IEnumerable<DetailedComponentContainer> components = _componentsStore.AllComponents;

            IEnumerable<Supplier> suppliers = _suppliersStore.Suppliers;
            ObservableCollection<SupplierContainer> suppliersCol = new ObservableCollection<SupplierContainer>(suppliers.Select(x => new SupplierContainer(x)));

            foreach(DetailedComponentContainer componentContainer in components)
            {

                _components.AddOrUpdate(new DetailedComponentContainerHolder(_viewModel, componentContainer, suppliersCol));
            }

            DataLoaded?.Invoke();
        }

        public IObservable<IChangeSet<DetailedComponentContainerHolder, int>> EmployeesConnection() => _components.Connect();

        public event Action DataLoaded;

        public async Task ReloadComponentsData()
        {
            _componentsStore.ReloadAllComponentsData();
        }
    }
}
