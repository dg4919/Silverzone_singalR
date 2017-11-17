using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class eventYearController : ApiController
    {
        IEventYearRepository _eventYearRepository;
        IEventYearClassRepository _eventYearClassRepository;
        public eventYearController(IEventYearRepository _eventyearRepository, IEventYearClassRepository _eventYearClassRepository)
        {
            this._eventYearRepository = _eventyearRepository;
            this._eventYearClassRepository = _eventYearClassRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new {
                result = _eventYearRepository.Get(),
                Event = _eventYearRepository.Get(DateTime.Now.Year, true)
            });
        }
       

        [HttpPost]
        public IHttpActionResult Create_Update(EventYear model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _eventYearRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)//Insert
                        {
                            if (_eventYearRepository.Exists(model.EventId,model.Event_Year))
                                return Ok(new { result = "error", message = "Event already exists in year " + DateTime.Now.Year + " !" });

                            EventYear _eventyear = new EventYear();
                            _eventyear.Status = true;
                            SetValue(model, ref _eventyear);

                            model.Id= _eventYearRepository.Create(_eventyear).Id;

                            msg = "Successfully Event created in year " + model.Event_Year + " !";
                        }
                        else//Update
                        {
                            var _eventyear = _eventYearRepository.Get(model.Id);
                            if (_eventyear != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_eventyear.RowVersion, 0))
                            {
                                if (_eventYearRepository.Exists(model.Id, model.EventId,model.Event_Year))
                                    return Ok(new { result = "error", message = "Event already exists in year " + model.Event_Year + " !" });
                                else
                                {
                                    if (_eventyear != null)
                                    {
                                        SetValue(model, ref _eventyear);

                                        _eventYearRepository.Update(_eventyear);

                                        _eventYearClassRepository.DeleteWhere(_eventYearClassRepository.Get(model.Id));
                                    }
                                    msg = "Successfully Event updated in year " + DateTime.Now.Year + " !";
                                }
                            }
                        }
                        foreach (var item in model.EventYearClass)
                        {
                            _eventYearClassRepository.Create(new EventYearClass
                            {
                                EventYearId = model.Id,
                                ClassId = item.ClassId,
                                Status = true
                            });
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
                    var _eventyear = _eventYearRepository.Get(item.Id);
                    if (_eventyear != null)
                    {
                        _eventyear.Status = !_eventyear.Status;
                        _eventYearRepository.Update(_eventyear);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetValue(EventYear model,ref EventYear _eventyear)
        {
            _eventyear.EventId = model.EventId;
            _eventyear.Event_Year = model.Event_Year;            
            _eventyear.EventFee = model.EventFee;
            _eventyear.RetainFee = model.RetainFee;            
        }
    }
}
