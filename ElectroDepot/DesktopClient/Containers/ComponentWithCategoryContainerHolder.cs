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

        [RelayCommand]
        private void AddToPurchasedComponents()
        {
            Console.WriteLine();
            PurchaseItem pItem = new PurchaseItem(0, 0, Component.Component.ID, 0, 0);
            PurchaseItemComponentContainer piContainer = new PurchaseItemComponentContainer(Component.Component, pItem, Component.Category);
            _viewModel.PurchaseComponentsSource?.Add(new PurchaseItemComponentContainerHolder(_viewModel, piContainer));
            _viewModel.RefreshPurchasedComponents();
        }
    }
}
