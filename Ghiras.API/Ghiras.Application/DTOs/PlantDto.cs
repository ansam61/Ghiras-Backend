namespace Ghiras.Application.DTOs
{
    public class PlantDto
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; } = string.Empty;
        public string? ScientificName { get; set; }
        public string? ImageUrl { get; set; }
        public string? CareInstructions { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreatePlantDto
    {
        public string PlantName { get; set; } = string.Empty;
        public string? ScientificName { get; set; }
        public string? ImageUrl { get; set; }
        public string? CareInstructions { get; set; }
        public int CategoryId { get; set; }
    }
}
