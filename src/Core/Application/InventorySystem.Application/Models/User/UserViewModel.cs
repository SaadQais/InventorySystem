using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.Models.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "الأسم")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "البريد الألكتروني")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "المسؤليه")]
        public string UserRole { get; set; }

        public bool Enabled { get; set; }        
    }
}
