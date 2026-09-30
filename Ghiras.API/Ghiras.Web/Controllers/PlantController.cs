using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ghiras.Web.Controllers
{
    public class PlantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private const string ApiUrl = "http://localhost:5250/api/Plants";
        private const string CategoryApiUrl = "http://localhost:5250/api/PlantCategory";
        private const string ImageApiUrl = "http://localhost:5250/api/PlantImage";

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
                plants = new List<PlantViewModel>();
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
                return NotFound();
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
                plant = null;
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
                new SelectListItem { Value = "2", Text = "نباتات ظلية" },
                new SelectListItem { Value = "3", Text = "أعشاب ونباتات طبية" },
                new SelectListItem { Value = "4", Text = "خضروات وفواكه" },
                new SelectListItem { Value = "5", Text = "عصاريات وصبارات" },
                new SelectListItem { Value = "6", Text = "نباتات مائية" }
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