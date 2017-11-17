using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IQuizQuestionRepository : IRepository<QuizQuestion>
    {
        List<string> upload_quiz_Image_toTemp(string tempPath);
        bool Is_recordExist(DateTime _date, int quizId);
        bool Is_recordExist(DateTime start_date, DateTime end_date, int quizId);
    }
}
