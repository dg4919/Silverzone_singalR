using SilverzoneERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class eventController : ApiController
    {
        IEventRepository _eventRepository;
        IClassRepository _classRepository;
        public eventController(IEventRepository _eventRepository, IClassRepository _classRepository)
        {
            this._eventRepository = _eventRepository;
            this._classRepository = _classRepository;
        }

        [HttpGet]
        public IHttpActionResult Get(Nullable<bool> IsClass)
        {
            try
            {
                return Ok(new
                {
                    result = new
                    {
                        Event = _eventRepository.GetAll_School(),
                        Class = IsClass==true?_classRepository.Get():null
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IHttpActionResult Create_Update(Event model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _eventRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {
                            if(_eventRepository.Exists(model.EventName,model.EventCode,model.SubjectName,out msg))
                                return Ok(new { result = "Error", message = msg });

                            //var _event = _eventRepository.FindBy(x => x.EventName.ToLower() == model.EventName.ToLower() && x.EventCode.ToLower() == model.EventCode.ToLower()).FirstOrDefault();
                            //if (_event != null)
                            //    return Ok(new { result = "Success", message = "Event already exists!" });
                            _eventRepository.Create(new Event
                            {
                                EventName = model.EventName,
                                EventCode = model.EventCode,
                                SubjectName = model.SubjectName,
                                Status = true
                            });
                            msg = "Successfully event created!";                            
                        }
                        else
                        {
                            var _event = _eventRepository.FindBy(x => x.Id == model.Id).FirstOrDefault();
                            if (_event != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_event.RowVersion, 0))
                            {
                                if (_eventRepository.Exists(model.Id,model.EventName,model.EventCode,model.SubjectName,out msg))
                                    return Ok(new { result = "Error", message = msg });

                                _event.EventName = model.EventName;
                                _event.EventCode = model.EventCode;
                                _event.SubjectName = model.SubjectName;
                                _eventRepository.Update(_event);

                                msg = "Successfully event updated!";                                
                            }
                            else                            
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !" });                            
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });            
        }

        [HttpPost]
        public IHttpActionResult Active_Deactive(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _event = _eventRepository.FindBy(x => x.Id == item.Id).FirstOrDefault();
                    if (_event != null)
                    {
                        _event.Status = !_event.Status;
                        _eventRepository.Update(_event);
                    }
                }
                return Ok(new { result = "Success", message = "Successfully save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
