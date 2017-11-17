using SilverzoneERP.Entities.ViewModel;
using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    public class classController : ApiController
    {
        IClassRepository _classRepository;
        public classController(IClassRepository _classRepository)
        {
            this._classRepository = _classRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new { result = _classRepository.GetAllClass()});
        }

        [HttpPost]
        public IHttpActionResult Create_Update(Class model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _classRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if(model.Id==0)//Insert
                        {
                            if(_classRepository.Exists(model.className))
                                return Ok(new { result = "error", message = "Class already exists !" });
                            else
                            {
                                _classRepository.Create(new Class {
                                    className = model.className,
                                    Status = true
                                });
                                msg = "Successfully class created!";                                
                            }
                        }
                        else//Update
                        {
                            var _class = _classRepository.Get(model.Id);
                            if (_class != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_class.RowVersion, 0))
                            {
                                if (_classRepository.Exists(model.Id, model.className))
                                    return Ok(new { result = "error", message = "Class already exists !" });
                                else
                                {                                  
                                    if (_class != null)
                                    {
                                        _class.className = model.className;                                        
                                        _classRepository.Update(_class);
                                    }
                                    msg = "Successfully class updated!";                                    
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
                    var _class = _classRepository.Get(item.Id);
                    if (_class != null)
                    {
                        _class.Status = !_class.Status;
                        _classRepository.Update(_class);
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
