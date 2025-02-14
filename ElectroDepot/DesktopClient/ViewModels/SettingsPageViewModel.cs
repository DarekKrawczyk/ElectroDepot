using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Services;
using DesktopClient.Utils;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class SettingsPageViewModel : RootNavigatorViewModel
    {
        private ApplicationConfig applicationConfig;
        public SettingsPageViewModel(RootPageViewModel defaultPageViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService, ApplicationConfig appConfig) : base(defaultPageViewModel, databaseStore, msgBoxService, appConfig)
        {
            applicationConfig = appConfig;
            IP = applicationConfig.ServerConfig.ConnectionURL;
        }

        [ObservableProperty]
        private string _iP;

        [RelayCommand]
        public void CopyIP()
        {
            ClipboardManager.SetText(IP);
        }
    }
}
