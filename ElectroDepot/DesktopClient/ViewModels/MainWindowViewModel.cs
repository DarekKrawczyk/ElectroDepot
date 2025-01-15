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
    public partial class MainWindowViewModel : BaseViewModel, INavagatable
    {
        [ObservableProperty]
        protected BaseViewModel _windowViewModel;

        public BaseViewModel View
        {
            get
            {
                return WindowViewModel;
            }
            set
            {
                WindowViewModel = value;
            }
        }

        public MainWindowViewModel(WindowNavigatorViewModel entryViewModel, DatabaseStore databaseStore) : base(databaseStore)
        {
            _windowViewModel = entryViewModel;
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            WindowNavigatorViewModel destinationViewModel;
            if (destination == "Login")
            {
                destinationViewModel = new LoginPageViewModel(this, DatabaseStore);
            }
            else if (destination == "Root")
            {
                destinationViewModel = new RootPageViewModel(this, DatabaseStore);
            }
            else if (destination == "Register")
            {
                destinationViewModel = new RegistrationPageViewModel(this, DatabaseStore);
            }
            else
            {
                throw new NotImplementedException();
            }

            WindowViewModel = destinationViewModel;
        }
    }
}
