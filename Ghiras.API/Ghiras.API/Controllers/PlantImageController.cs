using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantImageController : ControllerBase
    {
        private readonly IPlantImageRepository _repository;

        public PlantImageController(IPlantImageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("plant/{plantId}")]
        public async Task<IActionResult> GetByPlantId(int plantId)
        {
            var images = await _repository.GetByPlantIdAsync(plantId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PlantImage image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(image);
            return Ok(image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
