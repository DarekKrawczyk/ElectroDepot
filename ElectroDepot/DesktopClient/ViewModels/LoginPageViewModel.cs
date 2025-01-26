using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Services;
using ElectroDepotClassLibrary.Stores;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClient.Services;

namespace DesktopClient.ViewModels
{
    public partial class LoginPageViewModel : WindowNavigatorViewModel
    {
        [ObservableProperty]
        private string _usernameText;

        partial void OnUsernameTextChanged(string value)
        {
            CanUserLogin();
        }

        [ObservableProperty]
        private string _passwordText;

        partial void OnPasswordTextChanged(string value)
        {
            CanUserLogin();
        }

        public void CanUserLogin()
        {
            if(PasswordText != null && PasswordText.Length > 0 && UsernameText != null && UsernameText.Length > 0)
            {
                CanLogin = true;
            }
            else
            {
                CanLogin = false;
            }
        }

        public void ClearText()
        {
            UsernameText = string.Empty;
            PasswordText = string.Empty;
        }

        [ObservableProperty]
        private bool _canLogin;

        [RelayCommand]
        public async void Login()
        {
            CanLogin = false;

            LoggingStatus status = await DatabaseStore.UsersStore.UserLogin(UsernameText, PasswordText);

            if(status == LoggingStatus.Success)
            {
                string buttonResult = await MsBoxService.DisplayMessageBox("Login successfull", Icon.Success);
                //_navigator.NavigateTo(Page.Root);
            }
            else
            {
                string buttonResult = await MsBoxService.DisplayMessageBox("Failed to login!", Icon.Error);
                ClearText();
            }

        }

        public LoginPageViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService) : base(windowViewModel, databaseStore, messageBoxService)
        {
        }


        [RelayCommand]
        public void CreateAccount()
        {
            //_navigator.NavigateTo(Page.Register);
        }
    }
}
