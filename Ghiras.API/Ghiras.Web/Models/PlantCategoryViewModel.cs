using System.ComponentModel.DataAnnotations;

namespace Ghiras.Web.Models
{
    public class PlantCategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [Display(Name = "اسم التصنيف")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "عدد النباتات")]
        public int PlantsCount { get; set; }
    }
}
