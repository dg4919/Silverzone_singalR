using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class examDateController : ApiController
    {
        IExaminationDateRepository _examinationDateRepository;
        public examDateController(IExaminationDateRepository _examinationDateRepository)
        {
            this._examinationDateRepository = _examinationDateRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            
            var data = from e in _examinationDateRepository.GetAll()
                       .ToList()
                       select new
                       {
                           ExaminationDateId=e.Id,
                           e.ExamDate,
                           ExamDateFormated= _examinationDateRepository.ordinal(e.ExamDate.Day)+" "+(e.ExamDate.ToString("MMMM"))+", "+e.ExamDate.Year,
                           IsEdit=DateTime.Now<=e.ExamDate,
                           e.Status,
                           e.RowVersion
                       };
            return Ok(new
            {
                result = data
            });
        }
        [HttpGet]
        public IHttpActionResult Get_Active()
        {

            var data = _examinationDateRepository.Get_Active();
            return Ok(new { result = data });
        }

        [HttpPost]
        public IHttpActionResult Create_Update(ExaminationDate model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _examinationDateRepository.BeginTransaction())
                {
                    try
                    {
                        //TimeZone tz = TimeZone.CurrentTimeZone;
                        //model.ExamDate = tz.ToUniversalTime(model.ExamDate);
                        //return Ok(new { result = "Success", message = "" });
                        string msg = "";
                        if(model.Id==0)//Insert
                        {
                            if(_examinationDateRepository.Exists(model.ExamDate))
                                return Ok(new { result = "error", message = "Exam Date already exists !" });
                            else
                            {
                                _examinationDateRepository.Create(new ExaminationDate {
                                    ExamDate = model.ExamDate,
                                    Status = true
                                });
                                msg = "Successfully Exam Date created!";                                
                            }
                        }
                        else//Update
                        {
                            var _examDate = _examinationDateRepository.Get(model.Id);
                            if (_examDate != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_examDate.RowVersion, 0))
                            {
                                if (_examinationDateRepository.Exists(model.Id, model.ExamDate))
                                    return Ok(new { result = "error", message = "Class already exists !" });
                                else
                                {                                  
                                    if (_examDate != null)
                                    {
                                        _examDate.ExamDate = model.ExamDate;                                        
                                        _examinationDateRepository.Update(_examDate);
                                    }
                                    msg = "Successfully Exam Date updated!";                                    
                                }
                            }                                
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });
                    }
                }                
            }
            return Ok(new { result = "error", message = "error" });            
        }

        [HttpPost]
        public IHttpActionResult Active_Deactive(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _examDate = _examinationDateRepository.Get(item.Id);
                    if (_examDate != null)
                    {
                        _examDate.Status = !_examDate.Status;
                        _examinationDateRepository.Update(_examDate);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
