using Ghiras.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ghiras.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AIDiagnosis> AIDiagnoses { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AIDiagnosis>()
                .HasKey(d => d.DiagnosisId);

            modelBuilder.Entity<Plant>()
                .HasKey(p => p.PlantId);

            modelBuilder.Entity<PlantCategory>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<PlantImage>()
                .HasKey(img => img.ImageId);

            modelBuilder.Entity<PlantImage>()
                .HasOne(img => img.Plant)
                .WithMany(p => p.Images)
                .HasForeignKey(img => img.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlantCategory>().HasData(
                new PlantCategory { CategoryId = 1, CategoryName = "نباتات داخلية", Description = "مناسبة للغرف والمكاتب", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 2, CategoryName = "نباتات خارجية", Description = "للحدائق والبلكونات", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 3, CategoryName = "عصاريات وصبار", Description = "تحتمل الجفاف", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 4, CategoryName = "طبية وعطرية", Description = "نعناع ولافندر ورائحة زكية", CreatedAt = DateTime.UnixEpoch }
            );
        }
    }
}
