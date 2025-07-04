namespace MultipleChoicExam.Models
{
    public class AddSubject
    {
        public string Search { get; set; }
        public Subject01 Subject { get; set; } = new Subject01();
        public List<Subject01> ListSubject { get; set; } = new List<Subject01>();

    }
}
