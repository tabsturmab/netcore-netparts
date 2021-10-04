using Microsoft.EntityFrameworkCore;
using NetParts.Models;
using NetParts.Models.ProductAggregator;

namespace NetParts.Database
{
    public class NetPartsContext : DbContext
    {
        public NetPartsContext(DbContextOptions<NetPartsContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<TechnicalAssistance> TechnicalAssistance { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<TechnicalAssistanceManufacturer> TechnicalAssistanceManufacturer { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Advertisement> Advertisement { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderSituation> OrderSituation { get; set; }
        public DbSet<LogEvent> EventLog { get; set; }
        public DbSet<OrderAdvertisement> OrderAdvertisement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TechnicalAssistanceManufacturer>()
                    .HasKey(t => new { t.IdTecAssistance, t.IdManufacturer });

            modelBuilder.Entity<TechnicalAssistanceManufacturer>()
                .HasOne(t => t.TechnicalAssistance)
                .WithMany(t => t.TechnicalAssistanceManufacturer)
                .HasForeignKey(t => t.IdTecAssistance);

            modelBuilder.Entity<TechnicalAssistanceManufacturer>()
                .HasOne(t => t.Manufacturer)
                .WithMany(t => t.TechnicalAssistanceManufacturer)
                .HasForeignKey(t => t.IdManufacturer);

            modelBuilder.Entity<OrderAdvertisement>()
                .HasKey(a => new { a.IdAdvert, a.IdOrder });

            modelBuilder.Entity<OrderAdvertisement>()
                .HasOne(a => a.Advertisement)
                .WithMany(a => a.OrderAdvertisement)
                .HasForeignKey(a => a.IdAdvert);

            modelBuilder.Entity<OrderAdvertisement>()
                .HasOne(a => a.Order)
                .WithMany(a => a.OrderAdvertisement)
                .HasForeignKey(a => a.IdOrder);

            modelBuilder.Entity<Collaborator>()
                .HasIndex(b => b.Email)
                .HasName("Index_Collaborator_Email")
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(b => b.PartNumber)
                .HasName("Index_Product_PartNumber")
                .IsUnique();

            modelBuilder.Entity<Address>()
                .HasIndex(b => b.ZipCode)
                .HasName("Index_Address_ZipCode");

            modelBuilder.Entity<Category>()
                .HasIndex(b => b.NameCategory)
                .HasName("Index_Category_NameCategory")
                .IsUnique();
        }
    }
}
