using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Ghiras.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ghiras.Infrastructure.Repositories
{
    public class PlantImageRepository : IPlantImageRepository
    {
        private readonly AppDbContext _context;

        public PlantImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlantImage>> GetByPlantIdAsync(int plantId)
        {
            try
            {
                return await _context.PlantImages
                    .Where(img => img.PlantId == plantId)
                    .OrderByDescending(img => img.IsPrimary)
                    .ThenByDescending(img => img.UploadedAt)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch
            {
                return new List<PlantImage>();
            }
        }

        public async Task<PlantImage?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.PlantImages.FindAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task AddAsync(PlantImage image)
        {
            try
            {
                await _context.PlantImages.AddAsync(image);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Fallback attempt to create table dynamically if missing
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(@"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlantImages')
                        BEGIN
                            CREATE TABLE [dbo].[PlantImages] (
                                [ImageId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                [PlantId] INT NOT NULL,
                                [ImageUrl] NVARCHAR(MAX) NOT NULL,
                                [IsPrimary] BIT NOT NULL DEFAULT 1,
                                [UploadedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
                                CONSTRAINT [FK_PlantImages_Plants_PlantId] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plants] ([PlantId]) ON DELETE CASCADE
                            );
                        END
                    ");

                    await _context.PlantImages.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    // Catch gracefully
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var image = await _context.PlantImages.FindAsync(id);
                if (image != null)
                {
                    _context.PlantImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }
            catch { }
        }
    }
}
