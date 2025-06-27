using System.ComponentModel.DataAnnotations;

namespace MultipleChoicExam.Models
{
    public class ChangePassword
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        public string ConfirmNewPassword { get; set; }
    }

}
