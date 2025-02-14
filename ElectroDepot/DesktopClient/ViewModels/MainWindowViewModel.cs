using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace DesktopClient.ViewModels
{
    public partial class MainWindowViewModel : BaseViewModel, INavagatable
    {
        [ObservableProperty]
        protected BaseViewModel _windowViewModel;

        private readonly ApplicationConfig _appConfig;

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

        public MainWindowViewModel(WindowNavigatorViewModel entryViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService, ApplicationConfig appConfig) : base(databaseStore, msgBoxService)
        {
            _windowViewModel = entryViewModel;
            _appConfig = appConfig;
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            WindowNavigatorViewModel destinationViewModel;
            if (destination == "Login")
            {
                destinationViewModel = new LoginPageViewModel(this, DatabaseStore, MsBoxService, _appConfig, true);
            }
            else if (destination == "Root")
            {
                destinationViewModel = new RootPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Register")
            {
                destinationViewModel = new RegistrationPageViewModel(this, DatabaseStore, MsBoxService);
            }
            else
            {
                throw new NotImplementedException();
            }

            WindowViewModel = destinationViewModel;
        }
    }
}
