using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class TeacherEventRepository : BaseRepository<TeacherEvent>, ITeacherEventRepository
    {
        public TeacherEventRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    
    }
}
