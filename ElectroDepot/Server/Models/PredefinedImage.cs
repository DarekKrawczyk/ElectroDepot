using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ElectroDepotClassLibrary.DTOs;

namespace Server.Models
{
    public class PredefinedImage
    {
        #region Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PredefinedImageID { get; set; }
        #endregion
        #region Fields
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Category { get; set; }
        [Required]
        public byte[] Image { get; set; }
        #endregion
    }

    public static class PredefinedImageExtensions
    {
        public static PredefinedImageDTO ToDTO(this PredefinedImage image)
        {
            return new PredefinedImageDTO(
                image.PredefinedImageID,
                image.Name,
                image.Category,
                image.Image
            );
        }

        public static PredefinedImage ToModel(this PredefinedImageDTO imageDTO)
        {
            return new PredefinedImage()
            {
                PredefinedImageID = imageDTO.ID,
                Name = imageDTO.Name,
                Category = imageDTO.Category,
                Image = imageDTO.Image
            };
        }

        public static PredefinedImage ToModel(this CreatePredefinedImageDTO imageDTO)
        {
            return new PredefinedImage()
            {
                Name = imageDTO.Name,
                Category = imageDTO.Category,
                Image = imageDTO.Image
            };
        }

        public static PredefinedImage ToCategory(this UpdatePredefinedImageDTO imageDTO)
        {
            return new PredefinedImage()
            {
                Name = imageDTO.Name,
                Category = imageDTO.Category,
                Image = imageDTO.Image
            };
        }
    }
}
