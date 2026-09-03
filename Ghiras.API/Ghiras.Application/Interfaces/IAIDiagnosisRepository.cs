using Ghiras.Domain.Entities;

namespace Ghiras.Application.Interfaces
{
    public interface IAIDiagnosisRepository
    {
        Task<IEnumerable<AIDiagnosis>> GetAllAsync();
        Task<AIDiagnosis?> GetByIdAsync(int id);
        Task<IEnumerable<AIDiagnosis>> GetByPlantIdAsync(int plantId);
        Task AddAsync(AIDiagnosis diagnosis);
        Task DeleteAsync(int id);
    }
}
