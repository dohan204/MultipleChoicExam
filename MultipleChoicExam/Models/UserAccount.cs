using System.ComponentModel.DataAnnotations;

namespace MultipleChoicExam.Models
{
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }
        public string RoleId { get; set; }
        [Required(ErrorMessage ="Vui lòng nập tài khoản!")]
        public string  UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!!")]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
        public string  FullName { get; set; }
        public string Email { get; set; }
        //public string  Phone { get; set; }
        public string  Address  { get; set; }
        public DateTime Birthday { get; set; }
        public string Note { get; set; }
        //public string CreateBy { get; set; }
        //public DateTime CreateAt { get; set; }
        //public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
