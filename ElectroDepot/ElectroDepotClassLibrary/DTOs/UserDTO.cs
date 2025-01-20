namespace ElectroDepotClassLibrary.DTOs
{
    public record UserDTO(
        int ID,
        string Username,
        string Password,
        string Email,
        string Name
    );

    public record CreateUserDTO(
        string Username,
        string Email,
        string Password,
        string Name
    );

    public record UpdateUserDTO(
        string Username,
        string Email,
        string Password,
        string Name
    );

    public static class UsersDTOsExtensions
    {
        //public static CreateUserDTO ToCreateUserDTO(this UserDTO userDTO)
        //{
        //    return new CreateUserDTO(Username: userDTO.Username, Email: userDTO.Email, Password: userDTO.Pas);
        //}

        //public static UpdateUserDTO ToUpdateUserDTO(this UserDTO componentDTO)
        //{
        //    return new UpdateUserDTO(Name: componentDTO.Name, Manufacturer: componentDTO.Manufacturer, Description: componentDTO.Description);
        //}
    }
}
