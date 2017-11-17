using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class titleController : ApiController
    {
        ITitleRepository _titleRepository;
        public titleController(ITitleRepository _titleRepository)
        {
            this._titleRepository = _titleRepository;
        }
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {                
                return Ok(new { result = _titleRepository.GetAll() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IHttpActionResult GetActive()
        {
            try
            {
                return Ok(new { result = _titleRepository.Get(true) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult Create_Update(Title model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _titleRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {                            
                            if (_titleRepository.Exists(model.TitleName))
                                return Ok(new { result = "Success", message = "Title already exists!" });

                            _titleRepository.Create(new Title
                            {
                                TitleName = model.TitleName,
                                Status = true
                            });
                            msg = "Successfully title created!";
                            
                        }
                        else
                        {
                            var _title = _titleRepository.Get(model.Id);
                            if (_title != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_title.RowVersion, 0))
                            {
                                if (_titleRepository.Exists(model.Id,model.TitleName))
                                    return Ok(new { result = "Success", message = "Title already exists!" });
                                
                                _title.TitleName = model.TitleName;
                                _titleRepository.Update(_title);

                                msg = "Successfully city updated!";                                
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
                    var _title = _titleRepository.Get(item.Id);
                    if(_title != null)
                    {
                        _title.Status = !_title.Status;
                        _titleRepository.Update(_title);
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
