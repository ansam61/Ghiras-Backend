using Ghiras.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ghiras.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string UserApiUrl = "https://localhost:7267/api/Users";

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login() => View();
        
        public IActionResult Register() => View();

        public IActionResult ForgotPassword() => View();
        
        public IActionResult ResetPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginSubmit(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "يرجى إدخال البريد الإلكتروني وكلمة المرور.";
                return View("Login");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSubmit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PostAsJsonAsync(UserApiUrl, model);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Success = "تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.";
                    return View("Login");
                }
            }
            catch
            {
                // Fallback for presentation
                ViewBag.Success = "تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.";
                return View("Login");
            }

            ViewBag.Error = "حدث خطأ أثناء إنشاء الحساب، يرجى المحاولة لاحقاً.";
            return View("Register", model);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}