using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class MonitoringPageViewModel : RootNavigatorViewModel
    {
        public MonitoringPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore) : base(defaultRootPageViewModel, databaseStore)
        {
        }

        //public override void Dispose()
        //{
        //}
    }
}
