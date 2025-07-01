namespace MultipleChoicExam.Models
{
    public class newUser
    {
        public UserAccount User { get; set; } = new UserAccount();
        public List<UserAccount> userList { get; set; } = new List<UserAccount>();
        public string SeachInforUser { get; set; }
    }
}
