using Avalonia.Controls;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Navigation
{
    //internal class WindowNavigator : INavigator
    //{
    //    private readonly BaseViewModel _window;
    //    private readonly DatabaseStore _databaseStore;
    //    public WindowNavigator(BaseViewModel window, DatabaseStore store)
    //    {
    //        _window = window;
    //        _databaseStore = store;
    //    }

    //    public void Navigate(string destination)
    //    {
    //        ViewModelBase destination;
    //        if (page == Page.Login)
    //        {
    //            destination = new LoginPageViewModel(databaseStore, this);
    //        }
    //        else if (page == Page.Root)
    //        {
    //            destination = new RootPageViewModel(databaseStore, this);
    //        }
    //        else if (page == Page.Register)
    //        {
    //            destination = new RegistrationPageViewModel(databaseStore, this);
    //        }
    //        else
    //        {
    //            throw new NotImplementedException();
    //        }

    //        _window.Navigate(destination);
    //    }
    //}
}
