using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;

namespace DesktopClient.ViewModels
{
    public class LogoutPageViewModel : ViewModelBase
    {
        public LogoutPageViewModel(DatabaseStore databaseStore, Navigator navigator) : base(databaseStore, navigator)
        {
            _navigator.NavigateTo(Page.Login);
        }

        public override void Dispose()
        {
        }
    }
}
