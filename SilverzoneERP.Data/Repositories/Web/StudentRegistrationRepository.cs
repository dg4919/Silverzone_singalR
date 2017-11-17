using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class StudentRegistrationRepository:BaseRepository<StudentRegistration>,IStudentRegistrationRepository
    {
        public StudentRegistrationRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
        public StudentRegistration GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
       public  IEnumerable<StudentRegistration> GetByEventCode(string ecode)
        {
            return _dbset.Where(x => x.EventCode == ecode).AsEnumerable();
        }
        
        public IEnumerable<StudentRegistration> GetBySchoolCode(string schcode)
        {
            return FindBy(x => x.SchCode == schcode).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByRegSrlNo(int srno)
        {
            return _dbset.Where(x => x.RegSrlNo == srno).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByEventCodeAndSchCode(string ecode, string schcode)
        {
            return _dbset.Where(x => x.EventCode == ecode && x.SchCode == schcode).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByExamDateOpted(int edate)
        {
            return _dbset.Where(x => x.ExamDateOpted == edate).AsEnumerable();
        }
    }
}
