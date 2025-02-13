using DesktopClient.Navigation;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;

namespace DesktopClient.ViewModels
{
    public class MonitoringPageViewModel : RootNavigatorViewModel
    {
        public MonitoringPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, ApplicationConfig appConfig) : base(defaultRootPageViewModel, databaseStore, messageBoxService, appConfig)
        {
        }

        //public override void Dispose()
        //{
        //}
    }
}
