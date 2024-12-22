namespace ElectroDepotClassLibrary.DTOs
{
    public record PredefinedImageDTO(
        int ID,
        string Name,
        string Category,
        byte[] Image
    );

    public record CreatePredefinedImageDTO(
        string Name,
        string Category,
        byte[] Image
    );

    public record UpdatePredefinedImageDTO(
        string Name,
        string Category,
        byte[] Image
    );

    public static class PredefinedImageDTOsExtensions
    {
        public static CreateCategoryDTO ToCreateCategoryDTO(this CategoryDTO categoryDTO)
        {
            return new CreateCategoryDTO(Name: categoryDTO.Name, Description: categoryDTO.Description, Image: categoryDTO.Image);
        }

        public static UpdateCategoryDTO ToUpdateCategoryDTO(this CategoryDTO categoryDTO)
        {
            return new UpdateCategoryDTO(Name: categoryDTO.Name, Description: categoryDTO.Description, Image: categoryDTO.Image);
        }
    }
}
