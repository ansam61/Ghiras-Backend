using Ghiras.Domain.Entities;

namespace Ghiras.Application.Interfaces
{
    public interface IPlantImageRepository
    {
        Task<IEnumerable<PlantImage>> GetByPlantIdAsync(int plantId);
        Task<PlantImage?> GetByIdAsync(int id);
        Task AddAsync(PlantImage image);
        Task DeleteAsync(int id);
    }
}
