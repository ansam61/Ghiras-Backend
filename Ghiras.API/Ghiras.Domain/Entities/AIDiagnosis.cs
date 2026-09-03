using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ghiras.Domain.Entities
{
    public class AIDiagnosis
    {
        [Key]
        public int DiagnosisId { get; set; }

        public int? PlantId { get; set; }

        public string SampleImageUrl { get; set; } = string.Empty;

        public string DiseaseName { get; set; } = string.Empty;

        public double ConfidenceRate { get; set; }

        public string? OrganicTreatment { get; set; }

        public DateTime DiagnosisDate { get; set; } = DateTime.Now;

        [ForeignKey("PlantId")]
        public Plant? Plant { get; set; }
    }
}
