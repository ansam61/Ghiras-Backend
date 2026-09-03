using Ghiras.Application.Interfaces;
using Ghiras.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIDiagnosisController : ControllerBase
    {
        private readonly IAIDiagnosisRepository _repository;

        public AIDiagnosisController(IAIDiagnosisRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AIDiagnosis>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AIDiagnosis>> GetById(int id)
        {
            var diagnosis = await _repository.GetByIdAsync(id);
            if (diagnosis == null)
                return NotFound();

            return Ok(diagnosis);
        }

        [HttpGet("plant/{plantId}")]
        public async Task<ActionResult<IEnumerable<AIDiagnosis>>> GetByPlantId(int plantId)
        {
            var result = await _repository.GetByPlantIdAsync(plantId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AIDiagnosis>> Create([FromBody] AIDiagnosis diagnosis)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(diagnosis);
            return CreatedAtAction(nameof(GetById), new { id = diagnosis.DiagnosisId }, diagnosis);
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