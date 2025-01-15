using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using Microsoft.AspNetCore.Identity;

namespace ElectroDepotClassLibrary.Services
{
    public enum RegistrationStatus
    {
        Success,
        Failure,
        FailureUsernameTaken,
        FailurePasswordDontMath,
        FailureEmailTaken,
        FatalError
    }

    public class UserRegisterService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UsersStore _usersStore;
        public UserRegisterService(UsersStore usersStore)
        {
            _usersStore = usersStore;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task<RegistrationStatus> Register(string username, string password, string confirmPassword, string email, string name)
        {
            try
            {
                if(password != confirmPassword)
                {
                    return RegistrationStatus.FailurePasswordDontMath;
                }

                User fromDB = null;

                fromDB = await _usersStore.UsersDP.GetUserByUsername(username);
                if(fromDB != null)
                {
                    return RegistrationStatus.FailureUsernameTaken;
                }

                fromDB = await _usersStore.UsersDP.GetUserByEMail(email);
                if(fromDB != null)
                {
                    return RegistrationStatus.FailureEmailTaken;
                }

                User newUser = new User(0, username, email, password, name);
                string hashedPassword = _passwordHasher.HashPassword(newUser, password);
                newUser.Password = hashedPassword;

                fromDB = await _usersStore.UsersDP.CreateUser(newUser);
                if(fromDB == null)
                {
                    return RegistrationStatus.Failure;
                }
                
                return RegistrationStatus.Success;
            }
            catch(Exception exception)
            {
                return RegistrationStatus.FatalError;
            }
        }
    }
}
