using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class RootNavigatorViewModel : BaseViewModel, INavigator
    {
        [ObservableProperty]
        private RootPageViewModel _rootPage;   
        
        public RootPageViewModel Root
        {
            get
            {
                return RootPage;
            }
            set
            {
                RootPage = value;
            }
        }

        public RootNavigatorViewModel(RootPageViewModel defaultPageViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService) : base(databaseStore, msgBoxService)
        {
            _rootPage = defaultPageViewModel;
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            _rootPage.Navigate(destination);
        }

        [RelayCommand]
        public void NavigatePage(string destination)
        {
            _rootPage.NavigatePage(destination);
        }

        public void NavigatePage(string destination, NavParam navParam = null)
        {
            _rootPage.NavigatePage(destination, navParam);
        }
    }
}
