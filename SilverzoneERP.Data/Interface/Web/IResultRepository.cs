using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IResultRepository:IRepository<Result>
    {
        Result GetById(int id);
        IEnumerable<Result> GetResultBySchoolCodeAndEventId(string schcode, int eventid);
        Result GetResultBySchoolCodeEventIdAndEnrollNo(string scode, int eventid, string enroll);
    }
}
