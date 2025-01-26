using CommunityToolkit.Mvvm.ComponentModel;
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
    public abstract class BaseViewModel : ObservableValidator
    {
        private MessageBoxService _msgBoxService;
        public MessageBoxService MsBoxService
        {
            get
            {
                return _msgBoxService;
            }
        }
        public DatabaseStore DatabaseStore { get; }
        public BaseViewModel(DatabaseStore databaseStore, MessageBoxService messageBoxService)
        {
            DatabaseStore = databaseStore;
            _msgBoxService = messageBoxService;
        } 
    }
}
