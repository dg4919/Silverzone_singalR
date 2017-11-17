using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class QuizQuestionRepository : BaseRepository<QuizQuestion>, IQuizQuestionRepository
    {
        public QuizQuestionRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public List<string> upload_quiz_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_Images_toTemp(tempPath);
        }

        public bool Is_recordExist (DateTime _date, int quizId)
        {
            return _dbset.Any(x => x.Active_Date == _date && x.Id != quizId && x.Status == true);
        }

        public bool Is_recordExist(DateTime start_date, DateTime end_date, int quizId)
        {
            return _dbset.Any(x => x.Active_Date == start_date
                                && x.End_Date == end_date
                                && x.Id != quizId
                                && x.Status == true);
        }
    }
}
