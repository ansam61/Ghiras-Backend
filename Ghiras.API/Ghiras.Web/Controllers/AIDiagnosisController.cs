using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ghiras.Web.Controllers
{
    public class AIDiagnosisController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private const string ApiUrl = "https://localhost:7267/api/AIDiagnosis";
        private const string PlantApiUrl = "https://localhost:7267/api/Plants";

        public AIDiagnosisController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            List<AIDiagnosisViewModel> list = new();

            try
            {
                var result = await client.GetFromJsonAsync<List<AIDiagnosisViewModel>>(ApiUrl);
                if (result != null) list = result;
            }
            catch
            {
                list = new List<AIDiagnosisViewModel>
                {
                    new AIDiagnosisViewModel { DiagnosisId = 1, PlantName = "نبتة البوتس", SampleImageUrl = "/images/leaf_sample1.jpg", DiseaseName = "تبقع الأوراق الفطري (Leaf Spot)", ConfidenceRate = 96.5, OrganicTreatment = "رش محلول زيت النيم والماء ورش أوراق النبات صباحاً مع تخفيف الري.", DiagnosisDate = DateTime.Now.AddDays(-2) },
                    new AIDiagnosisViewModel { DiagnosisId = 2, PlantName = "شجرة الزيتون", SampleImageUrl = "/images/leaf_sample2.jpg", DiseaseName = "عفن الجذور (Root Rot)", ConfidenceRate = 92.0, OrganicTreatment = "تهوية التربة والتوقف عن الري فوراً حتى تجف التربة تماماً، ورش مبيد فطري عضوي.", DiagnosisDate = DateTime.Now.AddDays(-5) }
                };
            }

            return View(list);
        }

        public async Task<IActionResult> Diagnose()
        {
            var model = new AIDiagnosisViewModel();
            model.Plants = await GetPlantSelectListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Diagnose(AIDiagnosisViewModel model)
        {
            if (model.UploadImage == null || model.UploadImage.Length == 0)
            {
                ModelState.AddModelError("UploadImage", "يرجى رفع صورة الورقة للنبتة المراد فحصها.");
                model.Plants = await GetPlantSelectListAsync();
                return View(model);
            }

            string imageUrl = await SaveImageFileAsync(model.UploadImage);
            model.SampleImageUrl = imageUrl;

            // Simulated AI Prediction Engine based on image/sample
            var simulatedDiseases = new[]
            {
                new { Disease = "تبقع الأوراق البكتيري (Bacterial Leaf Spot)", Treatment = "إزالة الأوراق المصابة بالكامل ورش محلول بيكربونات الصوديوم وزيت النيم." },
                new { Disease = "البياض الدقيقي (Powdery Mildew)", Treatment = "رش الأوراق بخلطة الحليب المخفف بالماء (1:9) أو محلول الكبريت المائي." },
                new { Disease = "عفن الجذور والرطوبة الزائدة", Treatment = "تقليل الكميات المروية، نقل النبتة لأصيص جيد التصريف وتغيير التربة التالفة." }
            };

            var randomChoice = simulatedDiseases[new Random().Next(simulatedDiseases.Length)];
            model.DiseaseName = randomChoice.Disease;
            model.OrganicTreatment = randomChoice.Treatment;
            model.ConfidenceRate = Math.Round(88.0 + (new Random().NextDouble() * 10), 1);
            model.DiagnosisDate = DateTime.Now;

            var client = _httpClientFactory.CreateClient();
            try
            {
                await client.PostAsJsonAsync(ApiUrl, model);
            }
            catch
            {
                // Fallback for presentation
            }

            ViewBag.IsResultReady = true;
            model.Plants = await GetPlantSelectListAsync();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                await client.DeleteAsync($"{ApiUrl}/{id}");
            }
            catch { }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetPlantSelectListAsync()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var plants = await client.GetFromJsonAsync<List<PlantViewModel>>(PlantApiUrl);
                if (plants != null && plants.Any())
                {
                    return plants.Select(p => new SelectListItem
                    {
                        Value = p.PlantId.ToString(),
                        Text = p.PlantName
                    });
                }
            }
            catch { }

            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "نبتة البوتس (اللبلاب)" },
                new SelectListItem { Value = "2", Text = "شجرة الزيتون" },
                new SelectListItem { Value = "3", Text = "نبتة الألوفيرا" }
            };
        }

        private async Task<string> SaveImageFileAsync(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/uploads/" + uniqueFileName;
        }
    }
}