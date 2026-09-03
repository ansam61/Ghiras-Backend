using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ghiras.Web.Models
{
    public class AIDiagnosisViewModel
    {
        public int DiagnosisId { get; set; }

        [Display(Name = "النبتة المرتبطة")]
        public int? PlantId { get; set; }

        [Display(Name = "اسم النبتة")]
        public string? PlantName { get; set; }

        [Display(Name = "صورة العينة")]
        public string SampleImageUrl { get; set; } = string.Empty;

        [Display(Name = "اسم المرض")]
        public string DiseaseName { get; set; } = string.Empty;

        [Display(Name = "نسبة الثقة")]
        public double ConfidenceRate { get; set; }

        [Display(Name = "العلاج العضوي المقترح")]
        public string? OrganicTreatment { get; set; }

        [Display(Name = "تاريخ التشخيص")]
        public DateTime DiagnosisDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        [Display(Name = "رفع صورة الفحص")]
        public IFormFile? UploadImage { get; set; }

        [JsonIgnore]
        public IEnumerable<SelectListItem>? Plants { get; set; }
    }
}
