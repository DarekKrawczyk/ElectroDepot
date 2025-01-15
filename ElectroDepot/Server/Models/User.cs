

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class User
    {
        #region Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        #endregion
        #region Foreign Keys
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<OwnsComponent> OwnsComponents { get; set; } = new List<OwnsComponent>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        #endregion
        #region Fields
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Username { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }
        #endregion
    }
}
