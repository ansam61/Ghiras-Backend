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

            modelBuilder.Entity<AIDiagnosis>()
                .HasOne(d => d.Plant)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plant>()
                .HasKey(p => p.PlantId);

            modelBuilder.Entity<Plant>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

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
                new PlantCategory { CategoryId = 1, CategoryName = "نباتات زينة داخلية", Description = "نباتات مخصصة للزينة الداخلية والمنازل", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 2, CategoryName = "نباتات ظلية", Description = "نباتات تناسب المساحات المغلقة", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 3, CategoryName = "أعشاب ونباتات طبية", Description = "نباتات تُستخدم في الطهي أو التداوي والاستخدامات العطرية", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 4, CategoryName = "خضروات وفواكه", Description = "نباتات إنتاجية ذات ثمار ونفع غذائي", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 5, CategoryName = "عصاريات وصبارات", Description = "نباتات متحملة للجفاف وتحتمل قلة الري", CreatedAt = DateTime.UnixEpoch },
                new PlantCategory { CategoryId = 6, CategoryName = "نباتات مائية", Description = "نباتات تنمو وتعيش في البيئة المائية", CreatedAt = DateTime.UnixEpoch }
            );
        }
    }
}
