using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class ExaminationDateRepository : BaseRepository<ExaminationDate>, IExaminationDateRepository
    {
        public ExaminationDateRepository(SilverzoneERPContext context) : base(context) { }

        public ExaminationDate Get(long ExaminationDateId)
        {
            return _dbset.FirstOrDefault(x => x.Id == ExaminationDateId);
        }
        public bool Exists(DateTime ExamDate)
        {
            return _dbset.FirstOrDefault(x => x.ExamDate == ExamDate) == null ? false : true;
        }
        public bool Exists(long ExaminationDateId, DateTime ExaminationDate)
        {
            return _dbset.FirstOrDefault(x => x.Id!= ExaminationDateId && x.ExamDate == ExaminationDate) == null ? false : true;
        }
        public dynamic Get_Active()
        {
            return _dbset.Where(x => x.Status == true).ToList()
                .Select(x => new {
                    ExaminationDateId = x.Id,
                    x.ExamDate,
                    ExamDateFormated = ordinal(x.ExamDate.Day) + " " + (x.ExamDate.ToString("MMMM")) + ", " + x.ExamDate.Year,
                    x.Status,
                    x.RowVersion
                });

        }
        public string ordinal(int num)
        {
            return ClassUtility.ordinal(num);
        }
    }
}
