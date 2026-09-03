using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.Web.Controllers
{
    public class PlantCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "http://localhost:5250/api/PlantCategory";

        public PlantCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            List<PlantCategoryViewModel> categories = new();

            try
            {
                var result = await client.GetFromJsonAsync<List<PlantCategoryViewModel>>(ApiUrl);
                if (result != null) categories = result;
            }
            catch
            {
                categories = new List<PlantCategoryViewModel>
                {
                    new PlantCategoryViewModel { CategoryId = 1, CategoryName = "نباتات داخلية", Description = "مناسبة للغرف والمكاتب", PlantsCount = 8, CreatedAt = DateTime.Now.AddDays(-30) },
                    new PlantCategoryViewModel { CategoryId = 2, CategoryName = "نباتات خارجية", Description = "للحدائق والبلكونات", PlantsCount = 15, CreatedAt = DateTime.Now.AddDays(-20) },
                    new PlantCategoryViewModel { CategoryId = 3, CategoryName = "عصاريات وصبار", Description = "تحتمل الجفاف", PlantsCount = 6, CreatedAt = DateTime.Now.AddDays(-10) },
                    new PlantCategoryViewModel { CategoryId = 4, CategoryName = "طبية وعطرية", Description = "نعناع ولافندر ورائحة زكية", PlantsCount = 4, CreatedAt = DateTime.Now.AddDays(-5) }
                };
            }

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantCategoryViewModel category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PostAsJsonAsync(ApiUrl, category);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "فشلت عملية إضافة التصنيف.");
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var category = await client.GetFromJsonAsync<PlantCategoryViewModel>($"{ApiUrl}/{id}");
                if (category == null) return NotFound();
                return View(category);
            }
            catch
            {
                return View(new PlantCategoryViewModel { CategoryId = id, CategoryName = "تصنيف تجريبي", Description = "وصف التصنيف التجريبي" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantCategoryViewModel category)
        {
            if (id != category.CategoryId) return BadRequest();

            if (!ModelState.IsValid) return View(category);

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PutAsJsonAsync($"{ApiUrl}/{id}", category);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            return View(category);
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
    }
}
