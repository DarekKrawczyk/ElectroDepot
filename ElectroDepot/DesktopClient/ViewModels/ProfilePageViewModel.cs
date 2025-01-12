using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public ProfilePageViewModel(DatabaseStore databaseStore, Navigator navigator) : base(databaseStore, navigator)
        {
        }

        public override void Dispose()
        {
        }
    }
}
