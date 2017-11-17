using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class QuizQuestionOptionsRepository : BaseRepository<QuizQuestionOptions>, IQuizQuestionOptionsRepository
    {
        public QuizQuestionOptionsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
      
    }
}
