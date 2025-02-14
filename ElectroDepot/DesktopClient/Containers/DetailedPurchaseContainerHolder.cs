using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.Services;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class DetailedPurchaseContainerHolder : ObservableObject
    {
        private readonly PurchasesPageViewModel _viewModel;
        private readonly DetailedPurchaseContainer _purchaseContainer;
        public DetailedPurchaseContainer Container { get { return _purchaseContainer; } } 
        public DetailedPurchaseContainerHolder(DetailedPurchaseContainer purchaseContainer, PurchasesPageViewModel viewModel)
        {
            _viewModel = viewModel;
            _purchaseContainer = purchaseContainer;
        }

        [RelayCommand]
        public async Task PreviewPurchase()
        {
            await _viewModel.NavigateTab(PurchasesTab.Preview);
        }

        [RelayCommand]
        public async Task OpenSupplierStore(string componentName)
        {
            WebsiteNavigationService.NavigateTo(_purchaseContainer.Supplier.Website);
        }
    }
}
