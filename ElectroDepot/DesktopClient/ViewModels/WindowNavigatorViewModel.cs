using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class WindowNavigatorViewModel : BaseViewModel, INavigator
    {
        private readonly MainWindowViewModel _mainWindow;

        public WindowNavigatorViewModel(DatabaseStore databaseStore) : base(databaseStore)
        {
        }

        public WindowNavigatorViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore) : base(databaseStore)
        {
            _mainWindow = windowViewModel;
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            _mainWindow.Navigate(destination);
        }

    }
}
