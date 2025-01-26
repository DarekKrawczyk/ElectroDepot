using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class DetailedComponentContainer : ObservableObject
    {
        private readonly ComponentsPageViewModel _viewModel;
        private readonly Component _component;
        private readonly OwnsComponent _ownedComponent;
        private readonly OwnsComponent _unusedComponent;
        public Component Component { get { return _component; } }
        public Category Category
        {
            get
            {
                return _component.Category;
            }
            set
            {
                _component.Category = value;
            }
        }
        public Bitmap Image
        {
            get
            {
                return _component.Image;
            }
            set
            {
                _component.Image = value;
            }
        }
        public int ID { get { return _component.ID; } }
        public int CategoryID { get { return _component.CategoryID; } }
        public string Name
        {
            get
            {
                return _component.Name;
            }
            set
            {
                _component.Name = value;
            }
        }
        public string Manufacturer
        {
            get
            {
                return _component.Manufacturer;
            }
            set
            {
                _component.Manufacturer = value;
            }
        }
        public string ShortDescription
        {
            get
            {
                return _component.ShortDescription;
            }
            set
            {
                _component.ShortDescription = value;
            }
        }
        public string LongDescription
        {
            get
            {
                return _component.LongDescription;
            }
            set
            {
                _component.LongDescription = value;
            }
        }
        public string DatasheetURL
        {
            get
            {
                return _component.DatasheetLink;
            }
            set
            {
                _component.DatasheetLink = value;
            }
        }
        public int OwnedAmount { get { return _ownedComponent.Quantity; } }
        public int AvailableAmount { get { return _unusedComponent.Quantity; } }
        public int UsedInProjects { get {  return _ownedComponent.Quantity - _unusedComponent.Quantity; } }
        public ObservableCollection<SupplierContainer> Suppliers { get; set; }

        public DetailedComponentContainer(ComponentsPageViewModel viewModel, Component component, OwnsComponent ownedComponent, OwnsComponent unusedComponent, ObservableCollection<SupplierContainer> suppliers)
        {
            _component = component;
            _ownedComponent = ownedComponent;
            _unusedComponent = unusedComponent;
            Suppliers = suppliers;
            _viewModel = viewModel;
        }

        public DetailedComponentContainer(DetailedComponentContainer other)
        {
            _component = new Component(other.Component);
            _ownedComponent = new OwnsComponent(other._ownedComponent);
            _unusedComponent = new OwnsComponent(other._unusedComponent);
            Suppliers = other.Suppliers;
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
    }
}
