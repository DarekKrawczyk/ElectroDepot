using ElectroDepotClassLibrary.DTOs;

namespace ElectroDepotClassLibrary.Models
{
    public class User
    {
        public int ID { get; }
        public string Username { get; set; }
        public string Email{ get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public User(int id, string username, string email, string password, string name)
        {
            ID = id;
            Name = name;
            Username = username;
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', Name: '{Name}', Email: '{Email}'";
        }
    }

    internal static class UserExtensionMethods
    {
        internal static UserDTO ToDTO(this User user)
        {
            return new UserDTO(
                ID: user.ID,
                Username: user.Name,
                Email: user.Email,
                Password: user.Password,
                Name: user.Name);
        }

        internal static UpdateUserDTO ToUpdateDTO(this User user)
        {
            return new UpdateUserDTO(
                Username: user.Name,
                Email: user.Email,
                Password: user.Password,
                Name: user.Name);
        }

        internal static CreateUserDTO ToCreateDTO(this User user)
        {
            return new CreateUserDTO(
                Username: user.Name,
                Email: user.Email,
                Password: user.Password,
                Name: user.Name);
        }

        internal static User ToModel(this UserDTO userDTO)
        {
            return new User(
                id: userDTO.ID,
                name: userDTO.Name,
                email: userDTO.Email,
                password: userDTO.Password,
                username: userDTO.Username);
        }
    }
}
