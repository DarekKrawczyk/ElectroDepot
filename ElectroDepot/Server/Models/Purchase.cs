﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Purchase
    {
        #region Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseID { get; set; }
        #endregion
        #region Foreign Keys
        [Required]
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey(nameof(Supplier))]
        public int SupplierID{ get; set; }
        public Supplier? Supplier { get; set; }
        
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
        #endregion
        #region Fields
        public DateTime PurchasedDate { get; set; }
        public double TotalPrice { get; set; }
        #endregion
    }
}
