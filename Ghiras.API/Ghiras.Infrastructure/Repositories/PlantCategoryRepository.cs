using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Ghiras.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ghiras.Infrastructure.Repositories
{
    public class PlantCategoryRepository : IPlantCategoryRepository
    {
        private readonly AppDbContext _context;

        public PlantCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlantCategory>> GetAllAsync()
        {
            return await _context.PlantCategories
                .Include(c => c.Plants)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlantCategory?> GetByIdAsync(int id)
        {
            return await _context.PlantCategories
                .Include(c => c.Plants)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task AddAsync(PlantCategory category)
        {
            await _context.PlantCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlantCategory category)
        {
            var local = _context.PlantCategories.Local.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.PlantCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.PlantCategories.FindAsync(id);
            if (category != null)
            {
                _context.PlantCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
