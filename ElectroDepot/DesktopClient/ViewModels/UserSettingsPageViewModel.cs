using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    internal class UserSettingsPageViewModel : RootNavigatorViewModel
    {
        public UserSettingsPageViewModel(RootPageViewModel defaultPageViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService, ApplicationConfig appConfig) : base(defaultPageViewModel, databaseStore, msgBoxService, appConfig)
        {
        }
    }
}
