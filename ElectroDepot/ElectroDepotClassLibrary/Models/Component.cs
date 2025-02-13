using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using ElectroDepotClassLibrary.Utility;

namespace ElectroDepotClassLibrary.Models
{
    public class Component
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string DatasheetLink { get; set; }
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
        public Component(int id, int categoryID, Category category, string name, string manufacturer, 
            string shortDescription, string longDescription, string datasheetLink, byte[] byteImage)
        {
            ID = id;
            CategoryID = categoryID;
            Category = category;
            Name = name;
            Manufacturer = manufacturer;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            DatasheetLink = datasheetLink;
            ByteImage = byteImage;
        }

        public Component(Component other)
        {
            ID = other.ID;
            CategoryID = other.CategoryID;
            Category = new Category(other.Category);
            Name = other.Name;
            Manufacturer = other.Manufacturer;
            ShortDescription = other.ShortDescription;
            LongDescription = other.LongDescription;
            DatasheetLink = other.DatasheetLink;
            ByteImage = other.ByteImage;

        }

        public bool Compare(Component other)
        {
            if (ID != other.ID || CategoryID != other.CategoryID || Name != other.Name || Manufacturer != other.Manufacturer || ShortDescription != other.ShortDescription ||
                LongDescription != other.LongDescription || DatasheetLink != other.DatasheetLink || ByteImage != other.ByteImage) return false;
            else return true;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', Name: '{Name}', Manufacturer: '{Manufacturer}', CategoryID: '{CategoryID}'";
        }

        /// <summary>
        /// Copy data from another component
        /// </summary>
        public void ReplaceWith(Component other)
        {
            ID = other.ID;
            CategoryID = other.CategoryID;
            Category = other.Category;
            Name = other.Name;
            Manufacturer = other.Manufacturer;
            ShortDescription = other.ShortDescription;
            LongDescription = other.LongDescription;
            DatasheetLink = other.DatasheetLink;
            if(other.ByteImage != null && other.ByteImage.Length > 0)
            {
                ByteImage = other.ByteImage;
            }
        }
    }

    public static class ComponentExtensionMethods
    {
        public static ComponentDTO ToDTO(this Component component)
        {
            return new ComponentDTO(ID: component.ID, CategoryID: component.CategoryID, Name: component.Name, 
                Manufacturer: component.Manufacturer, ShortDescription: component.ShortDescription,
                LongDescription: component.LongDescription, DatasheetLink: component.DatasheetLink,
                Image: component.ByteImage);
        }
        public static UpdateComponentDTO ToUpdateDTO(this Component component)
        {
            return new UpdateComponentDTO(Name: component.Name, Manufacturer: component.Manufacturer, CategoryID: component.CategoryID, 
                ShortDescription: component.ShortDescription, LongDescription: component.LongDescription,
                DatasheetLink: component.DatasheetLink);
        }
        public static CreateComponentDTO ToCreateDTO(this Component component)
        {
            return new CreateComponentDTO(CategoryID: component.CategoryID, Name: component.Name, 
                Manufacturer: component.Manufacturer, ShortDescription: component.ShortDescription,
                LongDescription: component.LongDescription, DatasheetLink: component.DatasheetLink,
                Image: component.ByteImage);
        }
        public static Component ToModel(this ComponentDTO componentDTO)
        {
            return new Component(id: componentDTO.ID, categoryID: componentDTO.CategoryID, category: null, 
                name: componentDTO.Name, manufacturer: componentDTO.Manufacturer, shortDescription: componentDTO.ShortDescription,
                longDescription: componentDTO.LongDescription, datasheetLink: componentDTO.DatasheetLink, byteImage: componentDTO.Image);
        }
    }
}