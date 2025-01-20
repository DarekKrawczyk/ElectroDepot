using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class ProfilePageViewModel : RootNavigatorViewModel
    {
        public ProfilePageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore) : base(defaultRootPageViewModel, databaseStore)
        {
        }

        //public override void Dispose()
        //{
        //}
    }
}
