﻿using System.ComponentModel.DataAnnotations;

namespace MultipleChoicExam.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string SubjectId { get; set; }
        public string QContent { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Answer { get; set; }
        //public DateTime? CreateAt { get; set; }
    }
}
