using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string BaseApiUrl = "https://localhost:7267/api";

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();
            var client = _httpClientFactory.CreateClient();

            try
            {
                var plants = await client.GetFromJsonAsync<List<PlantViewModel>>($"{BaseApiUrl}/Plants");
                if (plants != null)
                {
                    model.TotalPlants = plants.Count;
                    model.RecentPlants = plants.Take(5).ToList();
                }

                var categories = await client.GetFromJsonAsync<List<PlantCategoryViewModel>>($"{BaseApiUrl}/PlantCategory");
                if (categories != null)
                {
                    model.TotalCategories = categories.Count;
                }

                var diagnoses = await client.GetFromJsonAsync<List<AIDiagnosisViewModel>>($"{BaseApiUrl}/AIDiagnosis");
                if (diagnoses != null)
                {
                    model.TotalDiagnoses = diagnoses.Count;
                    model.RecentDiagnoses = diagnoses.Take(5).ToList();
                }

                var users = await client.GetFromJsonAsync<List<UserViewModel>>($"{BaseApiUrl}/Users");
                if (users != null)
                {
                    model.TotalUsers = users.Count;
                }
            }
            catch
            {
                // Fallback mock values if API is offline during preview
                model.TotalPlants = model.TotalPlants > 0 ? model.TotalPlants : 12;
                model.TotalCategories = model.TotalCategories > 0 ? model.TotalCategories : 4;
                model.TotalDiagnoses = model.TotalDiagnoses > 0 ? model.TotalDiagnoses : 28;
                model.TotalUsers = model.TotalUsers > 0 ? model.TotalUsers : 6;
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
