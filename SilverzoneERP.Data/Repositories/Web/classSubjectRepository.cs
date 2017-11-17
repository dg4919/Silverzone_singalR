using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
namespace SilverzoneERP.Data
{
    public class classSubjectRepository : BaseRepository<classSubject>, IclassSubjectRepository
    {
        public classSubjectRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public IEnumerable<classSubject> Get_subjects_ByClassId(int classId)
        {
            return FindBy(x => x.ClassId == classId);
        }
       

    }
}
