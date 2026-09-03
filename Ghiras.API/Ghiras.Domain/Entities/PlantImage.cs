using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ghiras.Domain.Entities
{
    public class PlantImage
    {
        [Key]
        public int ImageId { get; set; }

        public int PlantId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? FileName { get; set; }

        public bool IsPrimary { get; set; } = true;

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [ForeignKey("PlantId")]
        public Plant? Plant { get; set; }
    }
}
