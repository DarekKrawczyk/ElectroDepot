using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class PurchaseComponentHolder : ObservableObject
    {
        private int _used;
        private ProjectsPageViewModel _viewModel;
        private readonly Component _component;
        private readonly OwnsComponent _ownsComponent;
        private readonly Category _category;
        private ProjectPurchaseComponentHolder _projectComponentRefrence;
        public int ComponentID { get {  return _component.ID; } }
        public string Name { get { return _component.Name; } }
        public string Manufacturer { get { return _component.Manufacturer; } }
        public string Category { get { return _category.Name; } }
        public Bitmap Image { get {  return _component.Image; } }
        public int AvaiableInSystem { get { return _ownsComponent.Quantity; } }
        public int Avaiable { get { return _ownsComponent.Quantity - Used; } }
        public Component Component { get { return _component; } }   
        public int Used
        {
            get
            {
                //_viewModel.RefreshPurchasedComponents();
                return _used;
            }
            set
            {
                _used = value;
                _viewModel.RefreshPurchasedComponents();
            }
        }
        public PurchaseComponentHolder(ProjectsPageViewModel viewModel, Component component, OwnsComponent ownsComponent, Category category)
        {
            _viewModel = viewModel;
            _component = component;
            _ownsComponent = ownsComponent;
            _category = category;
            _used = 0;
        }


        public void RegisterProjectsComponent(ProjectPurchaseComponentHolder projectComponent)
        {
            _projectComponentRefrence = projectComponent;
        }

        [RelayCommand]
        public void AddToProject()
        {
            _viewModel.AddComponentToProject(this);
        }
    }
}
