using System.ComponentModel.DataAnnotations;

namespace Ghiras.Domain.Entities
{
    public class PlantCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Plant>? Plants { get; set; }
    }
}
