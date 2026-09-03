namespace Ghiras.Application.DTOs
{
    public class PlantImageDto
    {
        public int ImageId { get; set; }
        public int PlantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class CreatePlantImageDto
    {
        public int PlantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public bool IsPrimary { get; set; } = true;
    }
}
