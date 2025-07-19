    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace MultipleChoicExam.Models
    {
        public class HomeTest
        {
            public Subject01 Subject01 { get; set; } = new Subject01();
            public Question Question { get; set; } = new Question();
            public UserAccount Account { get; set; } = new UserAccount();
        // Danh sách option hiển thị
        public List<Question> Questions { get; set; } = new List<Question>();
            public List<SelectListItem>? QuestionOptions { get; set; }
        public int SelectedIndex { get; set; } = 0; // Chỉ mục câu hỏi hiện tại
                                                    // Các ID được chọn (binding khi submit)
        public int? SelectedQuestionId { get; set; }
            public int TotalQuestion { get; set; }
            public int TestTime { get; set; }


        }
    }
