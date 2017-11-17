using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class UserQuizPointsRepository : BaseRepository<UserQuizPoints>, IUserQuizPointsRepository
    {
        public UserQuizPointsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
       
    }
}
