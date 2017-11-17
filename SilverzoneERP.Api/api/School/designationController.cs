using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class designationController : ApiController
    {
        IDesignationRepository _designationRepository;
        public designationController(IDesignationRepository _designationRepository)
        {
            this._designationRepository = _designationRepository;
        }
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {               
                return Ok(new { result = _designationRepository.GetAll() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult Create_Update(Designation model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _designationRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {                            
                            if (_designationRepository.Exists(model.DesgName))
                                return Ok(new { result = "Success", message = "Designation already exists!" });
                            _designationRepository.Create(new Designation
                            {
                                DesgName = model.DesgName,
                                DesgDescription = model.DesgDescription,
                                Status = true
                            });
                            msg = "Successfully Designation created!";                            
                        }
                        else
                        {
                            var _desg = _designationRepository.Get(model.Id);
                            if (_desg != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_desg.RowVersion, 0))
                            {
                                if (_designationRepository.Exists(model.Id,model.DesgName))
                                    return Ok(new { result = "Success", message = "Designation already exists!" });                               

                                _desg.DesgName = model.DesgName;
                                _desg.DesgDescription = model.DesgDescription;
                                _designationRepository.Update(_desg);

                                msg = "Successfully designation updated!";                                
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
                    var _desg = _designationRepository.Get(item.Id);
                    if(_desg!=null)
                    {
                        _desg.Status = !_desg.Status;
                        _designationRepository.Update(_desg);
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
