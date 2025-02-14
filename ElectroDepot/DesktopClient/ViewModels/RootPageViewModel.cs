using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class RootPageViewModel : WindowNavigatorViewModel
    {
        [ObservableProperty]
        private BaseViewModel _pageViewModel;

        private readonly ApplicationConfig _appConfig;

        public BaseViewModel PageView
        {
            get
            {
                return PageViewModel;
            }
            set
            {
                PageViewModel = value;  
            }
        }


        public RootPageViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, ApplicationConfig appConfig) : base(windowViewModel, databaseStore, messageBoxService)
        {
            DatabaseStore.PredefinedImagesStore.Load();
            _appConfig = appConfig;
            NavigatePage("Home");
        }

        private RootNavigatorViewModel GetViewModel(string destination)
        {
            RootNavigatorViewModel destinationViewModel;
            if (destination == "Home")
            {
                destinationViewModel = new HomePageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Components")
            {
                destinationViewModel = new ComponentsPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Projects")
            {
                destinationViewModel = new ProjectsPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Purchases")
            {
                destinationViewModel = new PurchasesPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Tracking")
            {
                destinationViewModel = new MonitoringPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "Settings")
            {
                destinationViewModel = new SettingsPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else if (destination == "UserSettings")
            {
                destinationViewModel = new UserSettingsPageViewModel(this, DatabaseStore, MsBoxService, _appConfig);
            }
            else
            {
                throw new NotImplementedException();
            }
            return destinationViewModel;
        }

        [RelayCommand]
        public void NavigatePage(string destination)
        {
            RootNavigatorViewModel destinationViewModel = GetViewModel(destination);
            PageViewModel = destinationViewModel;
        }

        [RelayCommand]
        public void ReportBug()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "mailto:darek9krawczyk@hotmail.com",
                UseShellExecute = true
            });
        }

        public void NavigatePage(string destination, NavParam navParam = null)
        {
            RootNavigatorViewModel destinationViewModel = GetViewModel(destination);

            if(destinationViewModel is INavParamInterpreter interpreter && navParam != null)
            {
                interpreter.InterpreteNavigationParameter(navParam);
            }

            PageViewModel = destinationViewModel;
        }
    }
}
