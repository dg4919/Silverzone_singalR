using SilverzoneERP.Entities.ViewModel.Admin;
using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities;
using System;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.Constant;

namespace SilverzoneERP.Api.api.Admin
{
    public class QuizController : ApiController
    {
        IQuizQuestionRepository quizQuestionRepository { get; set; }
        IQuizQuestionOptionsRepository quizQuestionOptionsRepository { get; set; }
        IErrorLogsRepository errorLogsRepository { get; set; }

        [HttpPost]
        public IHttpActionResult upload_quizImage()
        {
            var url = image_urlResolver.quizImage_main;
            var save_Imagespath = quizQuestionRepository.upload_quiz_Image_toTemp(url);

            return Ok(new { result = save_Imagespath });
        }

        [HttpPost]
        public IHttpActionResult create_quiz(QuizViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { result = "error" });

            // to check if any record is exist
            if (model.QuizType == quizType.Normal_quiz)
                if (quizQuestionRepository.Is_recordExist(model.Active_Date, 0))
                    return Ok(new { result = "exist" });

            if (model.QuizType == quizType.Mega_quiz)
                if (quizQuestionRepository.Is_recordExist(model.Active_Date, model.End_Date, 0))
                    return Ok(new { result = "exist" });

            // multiple operations r perform at a time so using transaction :)
            // when 1st trans completed then 2nd will execute
            using (var transaction = quizQuestionRepository.BeginTransaction())
            {
                var quiz = new QuizQuestion();      // new blank model
                try
                {
                    quiz.Question = model.Question;     // attach new rcord in it without id value :)
                    quiz.Active_Date = model.Active_Date;
                    quiz.ImageUrl = model.ImageUrl;
                    quiz.Points = 10;
                    quiz.QuizType = model.QuizType;
                    quiz.Status = true;

                    if (model.QuizType == quizType.Mega_quiz)
                    {
                        quiz.End_Date = model.End_Date;
                        quiz.Prize = model.Prize;
                        quiz.PrizeImage = model.PrizeImage;
                    }

                    quiz = quizQuestionRepository.Create(quiz);

                    for (int i = 0; i < model.optionModel.options.Count; i++)
                    {
                        quizQuestionOptionsRepository.Create(new QuizQuestionOptions()
                        {
                            QuizId = quiz.Id,
                            Option = model.optionModel.options[i] ?? null,
                            ImageUrl = model.optionModel.options_ImageUrl[i] ?? null,
                            isAnswer = model.AnswerId == i ? true : false
                        }, false);
                    } // after bulk insert save record
                    quizQuestionOptionsRepository.Save();

                    // set answer id 
                    quiz.AnswerId = quizQuestionOptionsRepository
                                    .FindBy(x => x.QuizId == quiz.Id && x.isAnswer == true)
                                    .SingleOrDefault()
                                    .Id;

                    // update record :)
                    quizQuestionRepository.Update(quiz);

                    transaction.Commit();
                    return Ok(new { result = "success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }

        }

        [HttpPost]
        public IHttpActionResult update_quiz(QuizViewModel model)
        {
            var quiz = quizQuestionRepository.FindById(model.Id);

            if (quiz == null)
                return Ok(new { result = "notfound" });

            // to check if any record is exist
            if (model.QuizType == quizType.Normal_quiz)
                if (quizQuestionRepository.Is_recordExist(model.Active_Date, 0))
                    return Ok(new { result = "exist" });

            if (model.QuizType == quizType.Mega_quiz)
                if (quizQuestionRepository.Is_recordExist(model.Active_Date, model.End_Date, 0))
                    return Ok(new { result = "exist" });

            using (var transaction = quizQuestionRepository.BeginTransaction())
            {
                try
                {
                    quiz.Question = model.Question;
                    quiz.Active_Date = model.Active_Date;
                    quiz.ImageUrl = model.ImageUrl;
                    quiz.Points = 10;
                    quiz.QuizType = model.QuizType;
                    quiz.Status = true;

                    if (model.QuizType == quizType.Mega_quiz)
                    {
                        quiz.End_Date = model.End_Date;
                        quiz.Prize = model.Prize;
                        quiz.PrizeImage = model.PrizeImage;
                    }

                    // delete previous question options
                    var options = quizQuestionOptionsRepository.FindBy(x => x.QuizId == model.Id);
                    quizQuestionOptionsRepository.DeleteWhere(options);

                    for (int i = 0; i < model.optionModel.options.Count; i++)
                    {
                        quizQuestionOptionsRepository.Create(new QuizQuestionOptions()
                        {
                            QuizId = quiz.Id,
                            Option = model.optionModel.options[i],
                            ImageUrl = model.optionModel.options_ImageUrl[i],
                            isAnswer = model.AnswerId == i ? true : false
                        }, false);
                    } // after bulk insert save record
                    quizQuestionOptionsRepository.Save();

                    // set answer id 
                    quiz.AnswerId = quizQuestionOptionsRepository
                                    .FindBy(x => x.QuizId == quiz.Id && x.isAnswer == true)
                                    .SingleOrDefault()
                                    .Id;

                    // update record :)
                    quizQuestionRepository.Update(quiz);

                    transaction.Commit();
                    return Ok(new { result = "success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();     // first rollback changes then save logs in DB, otherwise log record will also rollback
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }

        }

        [HttpGet]
        public IHttpActionResult getAll_quiz()
        {
            var quizList = QuizViewModel.Parse(quizQuestionRepository.GetAll());

            return Ok(new { result = quizList });
        }

        [HttpGet]
        public IHttpActionResult get_quizbyId(int quizId)
        {
            var quiz = QuizViewModel.Parse(quizQuestionRepository.FindById(quizId));

            return Ok(new { result = quiz });
        }

        public QuizController(
            IQuizQuestionRepository _quizQuestionRepository,
            IQuizQuestionOptionsRepository _quizQuestionOptionsRepository,
            IErrorLogsRepository _errorLogsRepository
            )
        {
            quizQuestionRepository = _quizQuestionRepository;
            quizQuestionOptionsRepository = _quizQuestionOptionsRepository;
            errorLogsRepository = _errorLogsRepository;
        }
    }
}
