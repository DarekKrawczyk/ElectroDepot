using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public abstract class BaseViewModel : ObservableValidator
    {
        public DatabaseStore DatabaseStore { get; }
        public BaseViewModel(DatabaseStore databaseStore)
        {
            DatabaseStore = databaseStore;
        } 
    }
}
