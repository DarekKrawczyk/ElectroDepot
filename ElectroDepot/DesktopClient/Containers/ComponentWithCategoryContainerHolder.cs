using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class ComponentWithCategoryContainerHolder : ObservableObject
    {
        private readonly PurchasesPageViewModel _viewModel;
        public ComponentWithCategoryContainer Component { get; }
        public ComponentWithCategoryContainerHolder(ComponentWithCategoryContainer component, PurchasesPageViewModel viewModel)
        {
            _viewModel = viewModel;
            Component = component;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddToPurchasedComponentsCommand))]
        public bool _canAdd = true;

        [RelayCommand(CanExecute = nameof(CanAddToPurchasedComponents))]
        private void AddToPurchasedComponents()
        {
            PurchaseItem pItem = new PurchaseItem(0, 0, Component.Component.ID, 1, 0);
            PurchaseItemComponentContainer piContainer = new PurchaseItemComponentContainer(Component.Component, pItem, Component.Category);
            _viewModel.PurchaseComponentsSource?.Add(new PurchaseItemComponentContainerHolder(_viewModel, piContainer));
            CanAdd = false;
            _viewModel.RefreshPurchasedComponents();
            _viewModel.Add_ReevaluateTotalPrice();
            _viewModel.CartComponentsSizeChanged();
        }

        private bool CanAddToPurchasedComponents()
        {
            return _canAdd;
        }
    }
}
