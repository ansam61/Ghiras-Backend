using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Ghiras.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ghiras.Infrastructure.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly AppDbContext _context;

        public PlantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plant>> GetAllAsync()
        {
            try
            {
                return await _context.Plants
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch
            {
                return await _context.Plants
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<Plant?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Plants
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlantId == id);
            }
            catch
            {
                return await _context.Plants
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlantId == id);
            }
        }

        public async Task AddAsync(Plant plant)
        {
            await _context.Plants.AddAsync(plant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Plant plant)
        {
            var local = _context.Plants.Local.FirstOrDefault(p => p.PlantId == plant.PlantId);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Plants.Update(plant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant != null)
            {
                // إزالة سجلات الفحص والتشخيص المرتبطة بالنبتة لتجنب تعارض القيود الخارجية FK_AIDiagnoses_Plants_PlantId
                var diagnoses = await _context.AIDiagnoses.Where(d => d.PlantId == id).ToListAsync();
                if (diagnoses.Any())
                {
                    _context.AIDiagnoses.RemoveRange(diagnoses);
                }

                // إزالة سجلات الصور المرتبطة للنبتة
                var images = await _context.PlantImages.Where(img => img.PlantId == id).ToListAsync();
                if (images.Any())
                {
                    _context.PlantImages.RemoveRange(images);
                }

                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
