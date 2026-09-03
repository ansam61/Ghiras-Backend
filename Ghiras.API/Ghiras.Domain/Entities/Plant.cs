using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ghiras.Domain.Entities
{
    public class Plant
    {
        [Key]
        public int PlantId { get; set; }

        [Required]
        [MaxLength(150)]
        public string PlantName { get; set; } = string.Empty;

        public string? ScientificName { get; set; }

        public string? ImageUrl { get; set; }

        public string? CareInstructions { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public PlantCategory? Category { get; set; }

        public ICollection<PlantImage>? Images { get; set; }

        public ICollection<AIDiagnosis>? Diagnoses { get; set; }
    }
}
