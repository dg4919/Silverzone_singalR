using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class QuizQuestion : AuditableEntity<long>
    {
        public string Question { get; set; }

        public long? AnswerId { get; set; }

        public string ImageUrl { get; set; }

        public int Points { get; set; }

        [DataType(DataType.Date)]
        public DateTime Active_Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? End_Date { get; set; }

        public quizType QuizType { get; set; }

        public string Prize { get; set; }
        public string PrizeImage { get; set; }

        

        public virtual ICollection<QuizQuestionOptions> QuizOptions { get; set; }
    }
}
