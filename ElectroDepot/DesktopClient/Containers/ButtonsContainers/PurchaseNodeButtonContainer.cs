using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers.ButtonsContainers
{
    public partial class PurchaseNodeButtonContainer : ObservableObject
    {
        private readonly DetailedPurchaseContainer _node;
        private readonly RootNavigatorViewModel _viewModel;
        public DetailedPurchaseContainer Node
        {
            get
            {
                return _node;
            }
        }
        public PurchaseNodeButtonContainer(RootNavigatorViewModel viewModel, DetailedPurchaseContainer node)
        {
            _node = node;
            _viewModel = viewModel;
        }

        [RelayCommand]
        public void Execute()
        {
            _viewModel.NavigatePage("Purchases", NavParam.Create(NavOperation.Preview, _node.Purchase));
        }
    }
}
