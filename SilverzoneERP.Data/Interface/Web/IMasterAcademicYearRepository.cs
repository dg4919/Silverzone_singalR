using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IMasterAcademicYearRepository:IRepository<MasterAcademicYear>
    {
        MasterAcademicYear GetById(int id);
        MasterAcademicYear GetByYear(string year);
    }
}
