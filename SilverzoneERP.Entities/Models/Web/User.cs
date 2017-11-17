using System;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class User : UserCommon
    {
        public Nullable<long> ClassId { get; set; }
        public virtual Class Class { get; set; }

        public int TotalPoint { get; set; }
        public virtual ICollection<UserQuizPoints> QuizPoints { get; set; }
    }
}
