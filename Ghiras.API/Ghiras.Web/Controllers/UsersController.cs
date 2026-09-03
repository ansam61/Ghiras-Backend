using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "https://localhost:7267/api/Users";

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            List<UserViewModel> users = new();

            try
            {
                var result = await client.GetFromJsonAsync<List<UserViewModel>>(ApiUrl);
                if (result != null) users = result;
            }
            catch
            {
                users = new List<UserViewModel>
                {
                    new UserViewModel { UserId = 1, Username = "eng: ANSAM JAMEEL", Email = "ansam@ghiras.sa", Role = "المسؤول الرئيسي", CreatedAt = DateTime.Now.AddMonths(-6) },
                    new UserViewModel { UserId = 2, Username = "سارة الشمري", Email = "sara@ghiras.sa", Role = "مشرف زراعي", CreatedAt = DateTime.Now.AddMonths(-2) },
                    new UserViewModel { UserId = 3, Username = "محمد الغامدي", Email = "m.ghamdi@example.com", Role = "مستخدم رئيسي", CreatedAt = DateTime.Now.AddDays(-15) }
                };
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PostAsJsonAsync(ApiUrl, model);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "فشلت عملية إضافة المستخدم.");
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
    }
}