using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class StudentAttendanceRepository : BaseRepository<StudentAttendance>, IStudentAttendanceRepository
    {
        public StudentAttendanceRepository(SilverzoneERPContext context) : base(context) { }

      
    }
}
