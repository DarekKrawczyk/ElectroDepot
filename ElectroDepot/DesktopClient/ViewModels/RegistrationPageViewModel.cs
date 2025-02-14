using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Services;
using ElectroDepotClassLibrary.Stores;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DesktopClient.Services;
using Avalonia.Notification;

namespace DesktopClient.ViewModels
{
    public partial class RegistrationPageViewModel : WindowNavigatorViewModel
    {
        [ObservableProperty]
        private bool _signingUp = false;

        public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

        public void ClearText()
        {
            SetProperty(ref _name, "", false, nameof(Name));
            ClearErrors(nameof(Name));

            SetProperty(ref _username, "", false, nameof(Username));
            ClearErrors(nameof(Username));

            SetProperty(ref _email, "", false, nameof(Email));
            ClearErrors(nameof(Email));

            SetProperty(ref _password, "", false, nameof(Password));
            ClearErrors(nameof(Password));

            SetProperty(ref _confirmedPassword, "", false, nameof(ConfirmedPassword));
            ClearErrors(nameof(ConfirmedPassword));

            //SetProperty(ref _name, "", false, nameof(Name));
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "You have to provide Name!")]
        [MinLength(5, ErrorMessage = "Name is too short!")]
        private string _name;
        
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "You have to provide Username!")]
        [MinLength(5, ErrorMessage = "Username is too short!")]
        private string _username;
        
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [CustomValidation(typeof(RegistrationPageViewModel), nameof(ValidateEmail), ErrorMessage = "Invalid E-mail!")]
        private string _email;

        public static ValidationResult ValidateEmail(string _email, ValidationContext context)
        {
            string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            bool isValid = Regex.IsMatch(_email, emailRegex);
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new("Provided E-mail is not correct!");
            }
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "You need to provide password!")]
        [MinLength(6, ErrorMessage = "Password is too short!")]
        private string _password;

        partial void OnPasswordChanged(string value)
        {
            ValidateProperty(ConfirmedPassword, nameof(ConfirmedPassword));
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "You need to re-enter password!")]
        [CustomValidation(typeof(RegistrationPageViewModel), nameof(ValidatePasswordMath))]
        private string _confirmedPassword;

        public static ValidationResult ValidatePasswordMath(string _password, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            string password = instance.GetType().GetProperty(nameof(Password)).GetValue(instance) as string;
            string confirmPassword = instance.GetType().GetProperty(nameof(ConfirmedPassword)).GetValue(instance) as string;
            if(password != null && confirmPassword != null && password == confirmPassword)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new("Password don't match!");
            }
        }

        [ObservableProperty]
        private bool _canRegister;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.PropertyName == nameof(HasErrors))
            {
                if(Name != null && Username != null && Password != null && ConfirmedPassword != null && Email != null && HasErrors == false)
                {
                    CanRegister = true;
                }
                else
                {
                    CanRegister = false;
                }
            }
        }

        public bool CanUserRegister()
        {
            return HasErrors;
        }

        private async Task AfterRegisterAction(RegistrationStatus status)
        {
            switch (status)
            {
                case RegistrationStatus.Success:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#40DD50")
                        .HasBadge("Success")
                        .HasMessage("User registered.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                        .Dismiss().WithButton("Sign in", button =>
                        {
                            Navigate("Login");
                        })
                        .Queue();
                    ClearText();
                    break;
                case RegistrationStatus.Failure:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#E63946")
                        .HasBadge("Error")
                        .HasMessage("Failed to register user.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case RegistrationStatus.FailureEmailTaken:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#E63946")
                        .HasBadge("Error")
                        .HasMessage("This E-mail is already taken.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case RegistrationStatus.FailureUsernameTaken:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#E63946")
                        .HasBadge("Error")
                        .HasMessage("This username is already taken.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case RegistrationStatus.FailurePasswordDontMath:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#E63946")
                        .HasBadge("Error")
                        .HasMessage("Passwords don't match.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                case RegistrationStatus.FatalError:
                    SigningUp = false;
                    Manager
                        .CreateMessage()
                        .Animates(true)
                        .Background("#333")
                        .Accent("#E63946")
                        .HasBadge("Error")
                        .HasMessage("Server is not responding.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                        .Queue();
                    break;
                default:
                    //ClearText();
                    break;
            }

            //ValidateAllProperties();

            CanRegister = !CanUserRegister();
        }

        [RelayCommand]
        public async void Register()
        {
            CanRegister = false;
            SigningUp = true;

            RegistrationStatus status = await DatabaseStore.UsersStore.UserRegister(Username, Password, ConfirmedPassword, Email, Name);

            await AfterRegisterAction(status);
        }

        public RegistrationPageViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService) : base(windowViewModel, databaseStore, messageBoxService)
        {
        }

    }
}
