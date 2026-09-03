using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ghiras.Web.Models
{
    public class PlantViewModel
    {
        public int PlantId { get; set; }

        [Required(ErrorMessage = "اسم النبتة مطلوب")]
        [Display(Name = "اسم النبتة")]
        public string PlantName { get; set; } = string.Empty;

        [Display(Name = "الاسم العلمي")]
        public string? ScientificName { get; set; }

        [Display(Name = "رابط الصورة الرئيسية")]
        public string? ImageUrl { get; set; }

        [JsonIgnore]
        [Display(Name = "اختر صورة النبتة")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "إرشادات العناية")]
        public string? CareInstructions { get; set; }

        [Required(ErrorMessage = "يرجى اختيار تصنيف النبتة")]
        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }

        [Display(Name = "اسم التصنيف")]
        public string? CategoryName { get; set; }

        public List<PlantImageViewModel>? Images { get; set; } = new();

        [JsonIgnore]
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }

    public class PlantImageViewModel
    {
        public int ImageId { get; set; }
        public int PlantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}