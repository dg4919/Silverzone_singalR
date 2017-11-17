using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]

    public class schoolGroupController : ApiController
    {
        ISchoolGroupRepository _schoolGroupRepository;

        public schoolGroupController(ISchoolGroupRepository _schoolGroupRepository)
        {
            this._schoolGroupRepository = _schoolGroupRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new { result = _schoolGroupRepository.GetAll()});
        }

        [HttpPost]
        public IHttpActionResult Create_Update(SchoolGroup model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _schoolGroupRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if(model.Id==0)//Insert
                        {
                            if(_schoolGroupRepository.Exists(model.SchoolGroupName))
                                return Ok(new { result = "error", message = "School Group already exists !" });
                            else
                            {
                                _schoolGroupRepository.Create(new SchoolGroup
                                {
                                    SchoolGroupName = model.SchoolGroupName,
                                    Status = true
                                });
                                msg = "Successfully school group created!";                                
                            }
                        }
                        else//Update
                        {
                            var _schoolGroup = _schoolGroupRepository.Get(model.Id);
                            if (_schoolGroup != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_schoolGroup.RowVersion, 0))
                            {
                                if (_schoolGroupRepository.Exists(model.Id, model.SchoolGroupName))
                                    return Ok(new { result = "error", message = "School Group already exists !" });
                                else
                                {                                  
                                    if (_schoolGroup != null)
                                    {
                                        _schoolGroup.SchoolGroupName = model.SchoolGroupName;                                        
                                        _schoolGroupRepository.Update(_schoolGroup);
                                    }
                                    msg = "Successfully school group updated!";                                    
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
                    var _schoolGroup = _schoolGroupRepository.Get(item.Id);
                    if (_schoolGroup != null)
                    {
                        _schoolGroup.Status = !_schoolGroup.Status;
                        _schoolGroupRepository.Update(_schoolGroup);
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
