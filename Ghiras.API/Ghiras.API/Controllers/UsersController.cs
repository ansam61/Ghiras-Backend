using Microsoft.AspNetCore.Mvc;

namespace Ghiras.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // قائمة تجريبية مؤقتة للمستخدمين لحين ربطها بقاعدة البيانات
        private static List<object> _users = new List<object>
        {
            new { Id = 1, Name = "أحمد علي", Username = "ahmed", Email = "ahmed@example.com", Role = "Admin" },
            new { Id = 2, Name = "سارة محمد", Username = "sara", Email = "sara@example.com", Role = "User" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_users);
        }

        [HttpPost]
        public IActionResult Create([FromBody] object user)
        {
            _users.Add(user);
            return Ok(new { message = "تمت إضافة المستخدم بنجاح" });
        }
    }
}