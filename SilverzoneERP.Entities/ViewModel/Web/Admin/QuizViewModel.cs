using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SilverzoneERP.Entities.ViewModel.Admin
{
    public class QuizViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        public string ImageUrl { get; set; }

        //[Required]
        //public int Points { get; set; }

        [Required]
        public int AnswerId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Active_Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }

        [Required]
        public quizType QuizType { get; set; }

        public string Prize { get; set; }
        public string PrizeImage { get; set; }

        [Required]
        public QuizOptionsViewModel optionModel { get; set; }

        public static dynamic Parse(IQueryable<QuizQuestion> modelList)
        {
            return modelList.Select(quiz => new
            {
                quiz.Id,
                quiz.Question,
                quiz.Active_Date,
                quiz.Status,
                quiz.CreatedBy,
                quiz.CreationDate,
                quiz.UpdatedBy,
                quiz.UpdationDate
            });
        }

        public static dynamic Parse(QuizQuestion quiz)
        {
            return new
            {
                Id = quiz.Id,
                Question = quiz.Question,
                AnswerId = quiz.AnswerId,
                Active_Date = quiz.Active_Date,
                ImageUrl = quiz.ImageUrl,
                Points = quiz.Points,
                quiz.QuizType,
                optionModel = quiz.QuizOptions.Select(x => new
                {
                    Id = x.Id,
                    options = x.Option,
                    options_ImageUrl = x.ImageUrl
                }),
            };
        }

        public class QuizOptionsViewModel
        {
            public IList<string> options { get; set; }

            public IList<string> options_ImageUrl { get; set; }
        }

    }
}