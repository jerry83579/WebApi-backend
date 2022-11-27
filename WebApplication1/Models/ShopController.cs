using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class ShopController : DbContext
    {
        public ShopController()
        {
        }

        public ShopController(DbContextOptions<ShopController> options)
            : base(options)
        {
        }

        public virtual DbSet<Info> Info { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.68.116;Database=Restaurant;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=Jerry83579;Password=1023", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Info>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Location).HasColumnType("geometry");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImgUrl)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            });
        }
    }
}