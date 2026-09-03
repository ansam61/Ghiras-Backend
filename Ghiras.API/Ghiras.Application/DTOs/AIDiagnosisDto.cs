namespace Ghiras.Application.DTOs
{
    public class AIDiagnosisDto
    {
        public int DiagnosisId { get; set; }
        public int? PlantId { get; set; }
        public string? PlantName { get; set; }
        public string SampleImageUrl { get; set; } = string.Empty;
        public string DiseaseName { get; set; } = string.Empty;
        public double ConfidenceRate { get; set; }
        public string? OrganicTreatment { get; set; }
        public DateTime DiagnosisDate { get; set; }
    }

    public class CreateAIDiagnosisDto
    {
        public int? PlantId { get; set; }
        public string SampleImageUrl { get; set; } = string.Empty;
        public string DiseaseName { get; set; } = string.Empty;
        public double ConfidenceRate { get; set; }
        public string? OrganicTreatment { get; set; }
    }
}
