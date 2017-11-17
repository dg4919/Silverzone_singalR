using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class UserQuizPoints : Entity<long>
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User UserInfo { get; set; }

        public long QuizId { get; set; }

        [ForeignKey("QuizId")]
        public virtual QuizQuestion quiz { get; set; }

        public int Answerid { get; set; }

        public DateTime Submit_Date{ get; set; }

    }
}
