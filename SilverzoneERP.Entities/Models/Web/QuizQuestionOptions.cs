using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class QuizQuestionOptions : Entity<long>
    {
        public string Option { get; set; }

        public long QuizId { get; set; }

        public string ImageUrl { get; set; }

        public bool isAnswer { get; set; }

        [ForeignKey("QuizId")]      // virtual must use to initialize this navigation prop values :)
        public virtual QuizQuestion QuizQuestion { get; set; }
    }
}
