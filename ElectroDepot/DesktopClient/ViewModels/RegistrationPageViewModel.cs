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

namespace DesktopClient.ViewModels
{
    public partial class RegistrationPageViewModel : WindowNavigatorViewModel
    {
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

        [RelayCommand]
        public async void Register()
        {
            CanRegister = false;

            RegistrationStatus status = await DatabaseStore.UsersStore.UserRegister(Username, Password, ConfirmedPassword, Email, Name);

            if (status == RegistrationStatus.Success)
            {
                string buttonResult = await MsBoxService.DisplayMessageBox("User registered successfully", Icon.Success);
                //_navigator.NavigateTo(Page.Login);
            }
            else
            {
                string buttonResult = await MsBoxService.DisplayMessageBox("Failed to register user!", Icon.Error);
            }
        }

        public RegistrationPageViewModel(MainWindowViewModel windowViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService) : base(windowViewModel, databaseStore, messageBoxService)
        {
        }

    }
}
