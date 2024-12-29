using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Component
    {
        #region Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentID { get; set; }
        #endregion
        #region Foreign Keys
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        public ICollection<OwnsComponent> OwnsComponents { get; set; } = new List<OwnsComponent>();
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
        public ICollection<ProjectComponent> ProjectComponents { get; set; } = new List<ProjectComponent>();
        #endregion
        #region Fields
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Manufacturer { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? ShortDescription { get; set; }

        [Column(TypeName = "nvarchar(4000)")]
        public string? LongDescription { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? DatasheetLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ImageURI { get; set; }
        #endregion
    }
}
