using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Ghiras.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ghiras.Infrastructure.Repositories
{
    public class AIDiagnosisRepository : IAIDiagnosisRepository
    {
        private readonly AppDbContext _context;

        public AIDiagnosisRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AIDiagnosis>> GetAllAsync()
        {
            return await _context.AIDiagnoses
                .Include(d => d.Plant)
                .AsNoTracking()
                .OrderByDescending(d => d.DiagnosisDate)
                .ToListAsync();
        }

        public async Task<AIDiagnosis?> GetByIdAsync(int id)
        {
            return await _context.AIDiagnoses
                .Include(d => d.Plant)
                .FirstOrDefaultAsync(d => d.DiagnosisId == id);
        }

        public async Task<IEnumerable<AIDiagnosis>> GetByPlantIdAsync(int plantId)
        {
            return await _context.AIDiagnoses
                .Where(d => d.PlantId == plantId)
                .OrderByDescending(d => d.DiagnosisDate)
                .ToListAsync();
        }

        public async Task AddAsync(AIDiagnosis diagnosis)
        {
            await _context.AIDiagnoses.AddAsync(diagnosis);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var diagnosis = await _context.AIDiagnoses.FindAsync(id);
            if (diagnosis != null)
            {
                _context.AIDiagnoses.Remove(diagnosis);
                await _context.SaveChangesAsync();
            }
        }
    }
}
