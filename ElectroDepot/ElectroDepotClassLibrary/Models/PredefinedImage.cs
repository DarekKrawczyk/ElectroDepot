using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Utility;

namespace ElectroDepotClassLibrary.Models
{
    public class PredefinedImage
    {
        public int ID { get; }
        public string Name { get; set; }
        public string Category { get; set; }
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
        public PredefinedImage(int id, string name, string category, byte[] image)
        {
            ID = id;
            Name = name;
            Category = category;
            ByteImage = image;
        }
        public override string ToString()
        {
            return $"ID: '{ID}', Name: '{Name}', Category: '{Category}'";
        }
    }

    public static class PredefinedImageExtensionMethods
    {
        public static PredefinedImageDTO ToDTO(this PredefinedImage category)
        {
            return new PredefinedImageDTO(ID: category.ID, Name: category.Name, Category: category.Category, Image: category.ByteImage);
        }
        public static UpdatePredefinedImageDTO ToUpdateDTO(this PredefinedImage category)
        {
            return new UpdatePredefinedImageDTO(Name: category.Name, Category: category.Category, Image: category.ByteImage);
        }
        public static CreatePredefinedImageDTO ToCreateDTO(this PredefinedImage category)
        {
            return new CreatePredefinedImageDTO(Name: category.Name, Category: category.Category, Image: category.ByteImage);
        }
        public static PredefinedImage ToModel(this PredefinedImageDTO categoryDTO)
        {
            return new PredefinedImage(id: categoryDTO.ID, name: categoryDTO.Name, category: categoryDTO.Category, image: categoryDTO.Image);
        }
    }
}
