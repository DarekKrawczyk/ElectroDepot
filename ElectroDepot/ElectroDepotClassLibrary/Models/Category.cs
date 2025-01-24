using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Utility;

namespace ElectroDepotClassLibrary.Models
{
    public class Category
    {
        public int ID { get; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public byte[] ByteImage { get; set; }
        public Bitmap Image
        {
            get
            {
                return ImageConverterUtility.BytesToBitmap(ByteImage);
            }
            set
            {
                ByteImage = ImageConverterUtility.BitmapToBytes(value);
            }
        }
        public Category(int id, string name, string description, byte[] image)
        {
            ID = id;
            Name = name;
            Description = description;
            ByteImage = image;
        }

        public Category(Category other)
        {
            this.ID = other.ID;
            this.Name = other.Name;
            this.Description = other.Description;
            this.ByteImage = other.ByteImage;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', Name: '{Name}', Description: '{Description}'";
        }
    }

    public static class CategoryExtensionMethods
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            return new CategoryDTO(ID: category.ID, Name: category.Name, Description: category.Description, Image: category.ByteImage);
        }
        public static UpdateCategoryDTO ToUpdateDTO(this Category category)
        {
            return new UpdateCategoryDTO(Name: category.Name, Description: category.Description, Image: category.ByteImage);
        }
        public static CreateCategoryDTO ToCreateDTO(this Category category)
        {
            return new CreateCategoryDTO(Name: category.Name, Description: category.Description, Image: category.ByteImage);
        }
        public static Category ToModel(this CategoryDTO categoryDTO)
        {
            return new Category(id: categoryDTO.ID, name: categoryDTO.Name, description: categoryDTO.Description, image: categoryDTO.Image);
        }
    }
}
