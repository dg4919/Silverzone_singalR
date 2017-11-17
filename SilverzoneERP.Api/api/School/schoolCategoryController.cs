using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]

    public class schoolCategoryController : ApiController
    {
        ISchoolCategoryRepository schoolCategoryRepository;

        public schoolCategoryController(ISchoolCategoryRepository _schoolCategoryRepository)
        {
            this.schoolCategoryRepository = _schoolCategoryRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new { result = schoolCategoryRepository.GetAll()});
        }

        [HttpPost]
        public IHttpActionResult Create_Update(SchoolCategory model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = schoolCategoryRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if(model.Id==0)//Insert
                        {
                            if(schoolCategoryRepository.Exists(model.CategoryName))
                                return Ok(new { result = "error", message = "Category already exists !" });
                            else
                            {
                                schoolCategoryRepository.Create(new SchoolCategory
                                {
                                    CategoryName = model.CategoryName,
                                    Status = true
                                });
                                msg = "Successfully category created!";                                
                            }
                        }
                        else//Update
                        {
                            var _category = schoolCategoryRepository.Get(model.Id);
                            if (_category != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_category.RowVersion, 0))
                            {
                                if (schoolCategoryRepository.Exists(model.Id, model.CategoryName))
                                    return Ok(new { result = "error", message = "Category already exists !" });
                                else
                                {                                  
                                    if (_category != null)
                                    {
                                        _category.CategoryName = model.CategoryName;                                        
                                        schoolCategoryRepository.Update(_category);
                                    }
                                    msg = "Successfully category updated!";                                    
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
                    var _category = schoolCategoryRepository.Get(item.Id);
                    if (_category != null)
                    {
                        _category.Status = !_category.Status;
                        schoolCategoryRepository.Update(_category);
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
