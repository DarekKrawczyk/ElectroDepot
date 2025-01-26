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
using Avalonia.Data;
using Avalonia.Notification;
using ElectroDepotClassLibrary.Models;
using System.Threading;

namespace DesktopClient.ViewModels
{
    public partial class LoginPageViewModel : WindowNavigatorViewModel
    {
        private readonly SecureStorageHelperService _secureStorageService;

        [ObservableProperty]
        private bool _rememberMe = true;

        [ObservableProperty]
        private bool _loggingIn = false;

        [ObservableProperty]
        private string _usernameText;

        partial void OnUsernameTextChanged(string value)
        {
            CanUserLogin();
        }

        public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

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

        private async Task AfterLoginAction(LoggingStatus status)
        {
            switch (status)
            {
                case LoggingStatus.Success:
                    if(RememberMe == true)
                    {
                        _secureStorageService.SaveCredentials(DatabaseStore.UsersStore.LoggedInUser);
                    }
                    //string buttonResult = await MsBoxService.DisplayMessageBox("Login successfull", Icon.Success);
                    Navigate("Root");
                    LoggingIn = false;
                    break;
                case LoggingStatus.FailureInvalidCredentials:
                    LoggingIn = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#1D3557")
                        .HasBadge("Failed to login")
                        .HasMessage("Invalid credentials.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case LoggingStatus.FailureUserDoesntExist:
                    LoggingIn = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#1D3557")
                        .HasBadge("Failed to login")
                        .HasMessage("User does not exist.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case LoggingStatus.FatalError:
                    LoggingIn = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#1D3557")
                        .HasBadge("Failed to login")
                        .HasMessage("Server is not responding.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                default:
                    ClearText();
                    break;
            }

            CanUserLogin();
        }

        [RelayCommand]
        public async Task Login()
        {
            LoggingIn = true;
            CanLogin = false;

            LoggingStatus status = await DatabaseStore.UsersStore.UserLogin(UsernameText, PasswordText);
            await AfterLoginAction(status);
        }

        public LoginPageViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, bool forgetUser) : base(windowViewModel, databaseStore, messageBoxService)
        {
            _secureStorageService = new SecureStorageHelperService();
            if(forgetUser == true)
            {
                _secureStorageService.DeleteCredentials();
            }
        }

        public async Task TryAutoLogin()
        {
            User savedUser = _secureStorageService.LoadCredentials();

            if (savedUser == null)
            {
                // User was not saved. Do nothing.
            }
            else
            {
                // Try to login saved user.
                UsernameText = savedUser.Username;
                PasswordText = "";
                LoggingIn = true;
                CanLogin = false;

                LoggingStatus status = await DatabaseStore.UsersStore.UserRememberMeLogin(savedUser.Username, savedUser.Password);
                await AfterLoginAction(status);
            }
        }
    }
}
