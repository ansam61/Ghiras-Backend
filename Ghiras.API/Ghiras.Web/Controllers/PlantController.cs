using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ghiras.Web.Controllers
{
    public class PlantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private const string ApiUrl = "https://localhost:7267/api/Plants";
        private const string CategoryApiUrl = "https://localhost:7267/api/PlantCategory";
        private const string ImageApiUrl = "https://localhost:7267/api/PlantImage";

        public PlantController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        public async Task<IActionResult> Index(string? searchString, int? categoryId)
        {
            var client = _httpClientFactory.CreateClient();
            List<PlantViewModel> plants = new();

            try
            {
                var result = await client.GetFromJsonAsync<List<PlantViewModel>>(ApiUrl);
                if (result != null) plants = result;
            }
            catch
            {
                plants = new List<PlantViewModel>
                {
                    new PlantViewModel { PlantId = 1, PlantName = "نبتة البوتس (اللبلاب)", ScientificName = "Epipremnum aureum", CategoryName = "نباتات زينة داخلية", ImageUrl = "/images/pothos.jpg", CareInstructions = "الري مرة أسبوعياً، وإبعادها عن الشمس المباشرة." },
                    new PlantViewModel { PlantId = 2, PlantName = "شجرة الزيتون", ScientificName = "Olea europaea", CategoryName = "أشجار خضراء", ImageUrl = "/images/olive.jpg", CareInstructions = "تحتاج شمس مباشرة وري معتدل." },
                    new PlantViewModel { PlantId = 3, PlantName = "نبتة الألوفيرا", ScientificName = "Aloe vera", CategoryName = "صبارات وعصاريات", ImageUrl = "/images/aloe.jpg", CareInstructions = "ري خفيف جداً كل أسبوعين." }
                };
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                plants = plants.Where(p => p.PlantName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                          (p.ScientificName != null && p.ScientificName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                plants = plants.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            ViewData["SearchString"] = searchString;
            return View(plants);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var plant = await client.GetFromJsonAsync<PlantViewModel>($"{ApiUrl}/{id}");
                if (plant == null) return NotFound();
                return View(plant);
            }
            catch
            {
                var mock = new PlantViewModel
                {
                    PlantId = id,
                    PlantName = "نبتة البوتس (اللبلاب)",
                    ScientificName = "Epipremnum aureum",
                    CategoryName = "نباتات زينة داخلية",
                    ImageUrl = "/images/pothos.jpg",
                    CareInstructions = "الري عند جفاف التربة السطحية، تقليم الأطراف الذابلة، وتغذيتها بالمغذي الخضري شهرياً."
                };
                return View(mock);
            }
        }

        public async Task<IActionResult> Create()
        {
            var model = new PlantViewModel();
            model.Categories = await GetCategorySelectListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantViewModel plant)
        {
            if (!ModelState.IsValid)
            {
                plant.Categories = await GetCategorySelectListAsync();
                return View(plant);
            }

            if (plant.ImageFile != null && plant.ImageFile.Length > 0)
            {
                plant.ImageUrl = await SaveImageFileAsync(plant.ImageFile);
            }

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PostAsJsonAsync(ApiUrl, plant);
                if (response.IsSuccessStatusCode)
                {
                    var createdPlant = await response.Content.ReadFromJsonAsync<PlantViewModel>();
                    if (createdPlant != null && !string.IsNullOrEmpty(plant.ImageUrl))
                    {
                        // Save image record in PlantImage DB table
                        var imageModel = new
                        {
                            PlantId = createdPlant.PlantId,
                            ImageUrl = plant.ImageUrl,
                            FileName = plant.ImageFile?.FileName,
                            IsPrimary = true
                        };
                        await client.PostAsJsonAsync(ImageApiUrl, imageModel);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "فشلت عملية حفظ النبتة.");
            plant.Categories = await GetCategorySelectListAsync();
            return View(plant);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            PlantViewModel? plant = null;
            try
            {
                plant = await client.GetFromJsonAsync<PlantViewModel>($"{ApiUrl}/{id}");
            }
            catch
            {
                plant = new PlantViewModel
                {
                    PlantId = id,
                    PlantName = "نبتة البوتس (اللبلاب)",
                    ScientificName = "Epipremnum aureum",
                    CategoryId = 1,
                    CareInstructions = "الري مرة أسبوعياً."
                };
            }

            if (plant == null) return NotFound();
            plant.Categories = await GetCategorySelectListAsync();
            return View(plant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantViewModel plant)
        {
            if (id != plant.PlantId) return BadRequest();

            if (!ModelState.IsValid)
            {
                plant.Categories = await GetCategorySelectListAsync();
                return View(plant);
            }

            if (plant.ImageFile != null && plant.ImageFile.Length > 0)
            {
                plant.ImageUrl = await SaveImageFileAsync(plant.ImageFile);
            }

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PutAsJsonAsync($"{ApiUrl}/{id}", plant);
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(plant.ImageUrl))
                    {
                        var imageModel = new
                        {
                            PlantId = plant.PlantId,
                            ImageUrl = plant.ImageUrl,
                            FileName = plant.ImageFile?.FileName,
                            IsPrimary = true
                        };
                        await client.PostAsJsonAsync(ImageApiUrl, imageModel);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "فشلت عملية تحديث بيانات النبتة.");
            plant.Categories = await GetCategorySelectListAsync();
            return View(plant);
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

        private async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var categories = await client.GetFromJsonAsync<List<PlantCategoryViewModel>>(CategoryApiUrl);
                if (categories != null && categories.Any())
                {
                    return categories.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    });
                }
            }
            catch { }

            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "نباتات زينة داخلية" },
                new SelectListItem { Value = "2", Text = "أشجار ومزروعات خارجية" },
                new SelectListItem { Value = "3", Text = "صبارات وعصاريات" },
                new SelectListItem { Value = "4", Text = "أعشاب خضراء ونباتات طبية" }
            };
        }

        private async Task<string> SaveImageFileAsync(IFormFile file)
        {
            string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(webRoot, "uploads");
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