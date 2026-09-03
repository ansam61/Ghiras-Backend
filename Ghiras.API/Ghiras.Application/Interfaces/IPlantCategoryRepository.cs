using Ghiras.Domain.Entities;

namespace Ghiras.Application.Interfaces
{
    public interface IPlantCategoryRepository
    {
        Task<IEnumerable<PlantCategory>> GetAllAsync();
        Task<PlantCategory?> GetByIdAsync(int id);
        Task AddAsync(PlantCategory category);
        Task UpdateAsync(PlantCategory category);
        Task DeleteAsync(int id);
    }
}
