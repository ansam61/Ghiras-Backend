namespace Ghiras.Application.DTOs
{
    public class PlantCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PlantsCount { get; set; }
    }

    public class CreatePlantCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
