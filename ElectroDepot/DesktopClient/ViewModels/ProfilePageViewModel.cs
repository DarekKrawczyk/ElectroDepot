using DesktopClient.Navigation;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class ProfilePageViewModel : RootNavigatorViewModel
    {
        public ProfilePageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService) : base(defaultRootPageViewModel, databaseStore, messageBoxService)
        {
        }

        //public override void Dispose()
        //{
        //}
    }
}
