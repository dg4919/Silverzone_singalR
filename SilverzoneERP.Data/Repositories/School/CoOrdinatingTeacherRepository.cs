using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class CoOrdinatingTeacherRepository : BaseRepository<CoOrdinatingTeacher>, ICoOrdinatingTeacherRepository
    {
        public CoOrdinatingTeacherRepository(SilverzoneERPContext context) : base(context) { }

      
    }
}
