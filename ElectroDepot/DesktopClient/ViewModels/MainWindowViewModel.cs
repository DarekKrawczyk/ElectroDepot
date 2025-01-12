using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace DesktopClient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentView;

        public MainWindowViewModel(DatabaseStore databaseStore, ViewModelBase baseModel, Navigator navigator) : base(databaseStore, navigator)
        {
            _currentView = baseModel;
        }

        public void Navigate(ViewModelBase viewModel)
        {
            CurrentView = viewModel;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
