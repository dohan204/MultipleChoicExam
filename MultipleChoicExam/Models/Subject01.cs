using System.ComponentModel.DataAnnotations;

namespace MultipleChoicExam.Models
{
    public class Subject01
    {
        [Key]
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
    }
}
