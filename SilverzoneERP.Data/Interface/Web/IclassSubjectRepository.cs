using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
namespace SilverzoneERP.Data
{
    public interface IclassSubjectRepository : IRepository<classSubject>
    {
       IEnumerable<classSubject> Get_subjects_ByClassId(int classId);
    }
}
