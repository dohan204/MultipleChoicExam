namespace MultipleChoicExam.Models
{
    public class SubmittedAnswer
    {
        public int QuestionId { get; set; } // ID của câu hỏi

        public string Answer { get; set; } = string.Empty; // Lựa chọn của người dùng

    }
}
