using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Navigation
{
    public enum View
    {
        Home,
        Components,
        Projects,
        Purchases,
        Tracking
    }
    public enum Page
    {
        Login,
        Register,
        Root
    }

    //public class Navigator
    //{
    //    public MainWindowViewModel Window { get; set; }
    //    private readonly DatabaseStore databaseStore;
    //    public Navigator(DatabaseStore store)
    //    {
    //        databaseStore = store;
    //    }

    //    public void NavigateTo(Page page)
    //    {
    //        ViewModelBase destination;
    //        if(page == Page.Login)
    //        {
    //            destination = new LoginPageViewModel(databaseStore, this);
    //        }
    //        else if (page == Page.Root)
    //        {
    //            destination = new RootPageViewModel(databaseStore, this);
    //        }
    //        else if(page == Page.Register)
    //        {
    //            destination = new RegistrationPageViewModel(databaseStore, this);
    //        }
    //        else
    //        {
    //            throw new NotImplementedException();
    //        }

    //        Window.Navigate(destination);
    //    }
    //}
}
