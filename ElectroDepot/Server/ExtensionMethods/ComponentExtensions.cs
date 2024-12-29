using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.ExtensionMethods
{
    public static class ComponentExtensions
    {
        public static ComponentDTO ToDTOWithImage(this Component component, ImageStorageService ISS)
        {
            byte[] image = new byte[0] { };

            try
            {
                image = ISS.RetrieveComponentImage(component.ImageURI);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retrieve image");
            }

            return new ComponentDTO(
                component.ComponentID,
                component.CategoryID,
                component.Name,
                component.Manufacturer,
                component.DatasheetLink,
                component.ShortDescription,
                component.LongDescription,
                image
            );
        }

        public static ComponentDTO ToDTO(this Component component)
        {
            return new ComponentDTO(
                component.ComponentID,
                component.CategoryID,
                component.Name,
                component.Manufacturer,
                component.DatasheetLink,
                component.ShortDescription,
                component.LongDescription,
                new byte[0] { }
            );
        }

        public static Component ToModel(this ComponentDTO componentDTO)
        {
            return new Component()
            {
                ComponentID = componentDTO.ID,
                CategoryID = componentDTO.CategoryID,
                Name = componentDTO.Name,
                Manufacturer = componentDTO.Manufacturer,
                ShortDescription = componentDTO.ShortDescription,
                LongDescription = componentDTO.LongDescription,
                DatasheetLink = componentDTO.DatasheetLink,
            };
        }

        public static Component ToModel(this CreateComponentDTO componentDTO)
        {
            return new Component()
            {
                CategoryID = componentDTO.CategoryID,
                Name = componentDTO.Name,
                Manufacturer = componentDTO.Manufacturer,
                ShortDescription = componentDTO.ShortDescription,
                LongDescription = componentDTO.LongDescription,
                DatasheetLink = componentDTO.DatasheetLink,
                ImageURI = string.Empty
            };
        }

        public static Component ToModel(this UpdateComponentDTO componentDTO)
        {
            return new Component()
            {
                Name = componentDTO.Name,
                Manufacturer = componentDTO.Manufacturer,
                ShortDescription = componentDTO.ShortDescription,
                LongDescription = componentDTO.LongDescription,
                DatasheetLink = componentDTO.DatasheetLink,
                ImageURI = string.Empty
            };
        }
    }
}
