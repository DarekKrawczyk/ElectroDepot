using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;

namespace DesktopClient.Containers
{
    public partial class ProjectPurchaseComponentHolder : ObservableObject
    {
        private readonly ProjectsPageViewModel _viewModel;
        private readonly PurchaseComponentHolder _purchaseComponentHolder;
        private int _used;
        public PurchaseComponentHolder ComponentFromDBRefrence { get {  return _purchaseComponentHolder; } }
        public int ComponentID { get { return _purchaseComponentHolder.ComponentID; } }
        public string Name { get { return _purchaseComponentHolder.Name; } }
        public string Manufacturer { get { return _purchaseComponentHolder.Manufacturer; } }
        public string Category { get { return _purchaseComponentHolder.Name; } }
        public Bitmap Image { get { return _purchaseComponentHolder.Image; } }
        public int Used
        {
            get
            {
                return _used;
            }
            set
            {
                if(value > 0 || value <= ComponentFromDBRefrence.Avaiable)
                {
                    _used = value;
                    ComponentFromDBRefrence.Used = value;
                }
            }
        }
        public ProjectPurchaseComponentHolder(ProjectsPageViewModel viewmodel, PurchaseComponentHolder purchaseComponentHolder)
        {
            _viewModel = viewmodel;
            _purchaseComponentHolder = purchaseComponentHolder;
            Used = 1;
        }

        [RelayCommand]
        public void RemoveFromProject()
        {
            _viewModel.RemoveComponentFromProject(this);
        }

        public void ClearUsage()
        {
            _used = 0;
            ComponentFromDBRefrence.Used = 0;
        }
    }
}
