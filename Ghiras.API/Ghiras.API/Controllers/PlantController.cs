using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly IPlantRepository _plantRepository;

        public PlantsController(IPlantRepository plantRepository)
        {
            _plantRepository = plantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? userId)
        {
            var plants = await _plantRepository.GetAllAsync();
            if (userId.HasValue && userId.Value > 0)
            {
                plants = plants.Where(p => p.UserId == null || p.UserId == userId.Value);
            }
            return Ok(plants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plant = await _plantRepository.GetByIdAsync(id);
            if (plant == null)
                return NotFound($"النبتة رقم {id} غير موجودة.");

            return Ok(plant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Plant plant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _plantRepository.AddAsync(plant);
            return CreatedAtAction(nameof(GetById), new { id = plant.PlantId }, plant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Plant plant)
        {
            if (id != plant.PlantId)
                return BadRequest("رقم الـ ID غير متطابق.");

            var existingPlant = await _plantRepository.GetByIdAsync(id);
            if (existingPlant == null)
                return NotFound($"النبتة رقم {id} غير موجودة.");

            await _plantRepository.UpdateAsync(plant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingPlant = await _plantRepository.GetByIdAsync(id);
            if (existingPlant == null)
                return NotFound($"النبتة رقم {id} غير موجودة.");

            await _plantRepository.DeleteAsync(id);
            return Ok(new { message = "تم الحذف بنجاح." });
        }
    }
}