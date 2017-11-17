using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class ResultRepository:BaseRepository<Result>,IResultRepository
    {
        public ResultRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
        public Result GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<Result> GetResultBySchoolCodeAndEventId(string schcode, int eventid)
        {
            return _dbset.Where(x => x.SchCode == schcode && x.EventId == eventid).AsEnumerable();
        }
        public Result GetResultBySchoolCodeEventIdAndEnrollNo(string scode, int eventid, string enroll)
        {
            return _dbset.Where(x => x.SchCode == scode && x.EventId == eventid && x.NIORollNo == enroll).FirstOrDefault();
        }
    }
}
