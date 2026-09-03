using System.ComponentModel.DataAnnotations;

namespace Ghiras.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "الدور / الصلاحية")]
        public string Role { get; set; } = "User";

        [Display(Name = "تاريخ الانضمام")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string? Password { get; set; }
    }
}
