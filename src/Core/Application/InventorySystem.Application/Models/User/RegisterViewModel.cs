using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Models.User
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الألكتروني")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمه السر")]
        [Compare("Password", ErrorMessage = "كلمه السر غير متطابقه")]
        public string ConfirmPassword { get; set; }
    }
}
