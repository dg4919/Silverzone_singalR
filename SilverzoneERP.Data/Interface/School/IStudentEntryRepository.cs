using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IStudentEntryRepository : IRepository<StudentEntry>
    {
        bool Exists(long EventmanagementId, long ClassId, string Section,long RollNo,string StudentName);
        bool Exists(long StudentEntryId, long EventmanagementId, long ClassId, string Section, long RollNo, string StudentName);
        StudentEntry Get(long StudentEntryId);
        dynamic Get(long EventManagementId, Nullable<long> ClassId, string Section, int StartIndex, int Limit, out long Count);
        dynamic Get(long EventManagementId, int GroupNo);

        string GenerateOTP(int length);
    }
}
