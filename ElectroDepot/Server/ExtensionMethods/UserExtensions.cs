using ElectroDepotClassLibrary.DTOs;
using Server.Models;

namespace Server.ExtensionMethods
{
    public static class UserExtensions
    {
        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO(
                user.UserID,
                user.Username,
                user.Password,
                user.Email,
                user.Name
            );
        }

        public static User ToUser(this UserDTO userDTO)
        {
            return new User()
            {
                UserID = userDTO.ID,
                Username = userDTO.Username,
                Email = userDTO.Email,
                Name = userDTO.Name,
            };
        }

        public static User ToUser(this CreateUserDTO userDTO)
        {
            return new User()
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Name = userDTO.Name,
            };
        }

        public static User ToUser(this UpdateUserDTO userDTO)
        {
            return new User()
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Name = userDTO.Name
            };
        }
    }
}
