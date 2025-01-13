using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class MonitoringPageViewModel : ViewModelBase
    {
        public MonitoringPageViewModel(DatabaseStore databaseStore, Navigator navigator) : base(databaseStore, navigator)
        {
        }

        public override void Dispose()
        {
        }
    }
}
