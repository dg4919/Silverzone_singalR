using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IExaminationDateRepository : IRepository<ExaminationDate>
    {
        dynamic Get_Active();
        ExaminationDate Get(long ExaminationDateId);      
        bool Exists(DateTime ExamDate);
        bool Exists(long ExaminationDateId, DateTime ExamDate);
        string ordinal(int num);
    }    
}
