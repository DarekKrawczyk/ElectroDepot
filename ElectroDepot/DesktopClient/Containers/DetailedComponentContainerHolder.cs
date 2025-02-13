using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DesktopClient.Containers
{
    public partial class DetailedComponentContainerHolder : ObservableObject
    {
        private readonly ComponentsPageViewModel _viewModel;
        private readonly DetailedComponentContainer _container;

        public DetailedComponentContainer Container {  get { return _container; } }
        public ObservableCollection<SupplierContainer> Suppliers { get; set; }

        public DetailedComponentContainerHolder(ComponentsPageViewModel viewModel, DetailedComponentContainer container, ObservableCollection<SupplierContainer> suppliers)
        {
            _container = container;
            Suppliers = suppliers;
            _viewModel = viewModel;
        }

        public DetailedComponentContainerHolder(DetailedComponentContainerHolder other)
        {
            Suppliers = other.Suppliers;
            _viewModel = other._viewModel;
            _container = new DetailedComponentContainer(other._container);
        }

        [RelayCommand]
        public async Task Components_EnterComponentPreviewCommand()
        {
            await _viewModel.NavigateTab(ComponentTab.Preview);
        }

        [RelayCommand]
        public async Task Components_EnterComponentEditCommand()
        {
            await _viewModel.NavigateTab(ComponentTab.Edit);
        }

        [RelayCommand(CanExecute = nameof(CanOpenDatasheet))]
        public async Task OpenDatasheet()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Container.Component.DatasheetLink,
                UseShellExecute = true
            });
        }

        private bool CanOpenDatasheet()
        {
            return Container.Component.DatasheetLink != null && Container.Component.DatasheetLink != string.Empty;
        }
    }
}
