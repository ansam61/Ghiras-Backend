using Ghiras.Domain.Entities;

namespace Ghiras.Application.Interfaces
{
    public interface IPlantRepository
    {
        Task<IEnumerable<Plant>> GetAllAsync();
        Task<Plant?> GetByIdAsync(int id);
        Task AddAsync(Plant plant);
        Task UpdateAsync(Plant plant);
        Task DeleteAsync(int id);
    }
}
