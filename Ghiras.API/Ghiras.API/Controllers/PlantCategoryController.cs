using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantCategoryController : ControllerBase
    {
        private readonly IPlantCategoryRepository _repository;

        public PlantCategoryController(IPlantCategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCategory category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlantCategory category)
        {
            if (id != category.CategoryId)
                return BadRequest("ID Mismatch");

            await _repository.UpdateAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}