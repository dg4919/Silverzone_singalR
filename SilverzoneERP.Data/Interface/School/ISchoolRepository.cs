using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface ISchoolRepository:IRepository<School>
    {
        string LastGenCode();
        School findBySchCode(long schCode);
        dynamic GetHistory(long SchoolId, int StartIndex, int Limit);
        dynamic Search(string Anything, string SchoolCode, string SchoolName, Nullable<long> CountryId, Nullable<long> ZoneId, Nullable<long> StateId, Nullable<long> DistrictId, Nullable<long> CityId, Nullable<long> PinCode ,Nullable<bool> BlackListed, Nullable<int> Event_Type,Nullable<long> EventId, int StartIndex,int Limit,ref int count);

        dynamic GetCourier();
        dynamic GetExamDate();
        dynamic GetSchool(long EventManagementId);
    }
}
