using System.ComponentModel.DataAnnotations;

namespace MultipleChoicExam.Models
{
    public class TestHistory
    {
        [Key]
        public int TestID { get; set; }
        public int UserId { get; set; }
        public string SubjectId { get; set; } = string.Empty;
        public DateTime testDate { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectAnswer { get; set; }
        public int Mark { get; set; }
    }
}
