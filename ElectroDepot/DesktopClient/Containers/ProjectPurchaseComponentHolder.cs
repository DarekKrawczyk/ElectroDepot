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
        private readonly PurchaseComponentHolder _avalableComponentRefrence;
        private int _used;
        public PurchaseComponentHolder ComponentFromDBRefrence { get {  return _avalableComponentRefrence; } }
        public int ComponentID { get { return _avalableComponentRefrence.ComponentID; } }
        public string Name { get { return _avalableComponentRefrence.Name; } }
        public string Manufacturer { get { return _avalableComponentRefrence.Manufacturer; } }
        public string Category { get { return _avalableComponentRefrence.Name; } }
        public Bitmap Image { get { return _avalableComponentRefrence.Image; } }
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
            _avalableComponentRefrence = purchaseComponentHolder;
        }

        //public void IncrementUsed()
        //{
        //    if(_avalableComponentRefrence != null)
        //    {
        //        _avalableComponentRefrence.ConsumeComponent();
        //        Used++;
        //        _viewModel.RefreshProjectComponents();
        //    }
        //}

        //public void DecrementUsed()
        //{
        //    if(_avalableComponentRefrence != null)
        //    {
        //        _avalableComponentRefrence.ReturnComponent();
        //        Used--;
        //        _viewModel.RefreshProjectComponents();
        //    }
        //}

        [RelayCommand]
        public void IncreaseUsed()
        {
            //IncrementUsed();
        }

        [RelayCommand]
        public void DecreaseUsed()
        {
            //DecrementUsed();
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
