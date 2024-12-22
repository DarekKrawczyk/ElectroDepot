using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Reflection.Metadata;

namespace Server.Context
{
    public class DatabaseContext : DbContext
    {
        #region DB Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectComponent> ProjectComponents { get; set; }
        public DbSet<OwnsComponent> OwnsComponent { get; set; }
        #endregion
        #region Constructor
        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
        public DbSet<Server.Models.PredefinedImage> PredefinedImage { get; set; } = default!;
    }
}
