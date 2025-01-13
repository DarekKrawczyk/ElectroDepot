using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;
using System;

namespace DesktopClient.ViewModels
{
    public abstract class ViewModelBase : ObservableValidator, IDisposable
    {
        protected Navigator _navigator;
        protected DatabaseStore DatabaseStore { get; }
        public ViewModelBase(DatabaseStore databaseStore, Navigator navigator)
        {
            DatabaseStore = databaseStore;
            _navigator = navigator;
        }

        public abstract void Dispose();
    }
}
