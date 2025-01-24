using System.ComponentModel.DataAnnotations;

namespace ElectroDepotClassLibrary.DTOs
{
    public record ComponentDTO(
        int ID,
        int CategoryID,
        string Name,
        string Manufacturer,
        string DatasheetLink,
        string ShortDescription,
        string LongDescription,
        byte[] Image
    );

    public record CreateComponentDTO(
        [Required]
        int CategoryID,
        string Name,
        string Manufacturer,
        string DatasheetLink,
        string ShortDescription,
        string LongDescription,
        byte[] Image
    );

    public record UpdateComponentDTO(
        string Name,
        string Manufacturer,
        int CategoryID,
        string DatasheetLink,
        string ShortDescription,
        string LongDescription
    );

    public static class ComponentDTOsExtensions
    {
        public static CreateComponentDTO ToCreateComponentDTO(this ComponentDTO componentDTO)
        {
            return new CreateComponentDTO(
                CategoryID: componentDTO.CategoryID, 
                Name: componentDTO.Name, 
                Manufacturer: componentDTO.Manufacturer, 
                ShortDescription: componentDTO.ShortDescription,
                LongDescription: componentDTO.LongDescription,
                DatasheetLink: componentDTO.DatasheetLink,
                Image: componentDTO.Image);
        }

        public static UpdateComponentDTO ToUpdateComponentDTO(this ComponentDTO componentDTO)
        {
            return new UpdateComponentDTO(
                Name: componentDTO.Name, 
                Manufacturer: componentDTO.Manufacturer, 
                CategoryID: componentDTO.CategoryID,
                ShortDescription: componentDTO.ShortDescription,
                LongDescription: componentDTO.LongDescription,
                DatasheetLink: componentDTO.DatasheetLink);
        }
    }
}
