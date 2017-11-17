using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class courierController : ApiController
    {
        ICourierRepository _courierRepository;
        public courierController(ICourierRepository _courierRepository)
        {
            this._courierRepository = _courierRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var data = _courierRepository.GetAll();
            return Ok(new { result = data });
        }

        [HttpPost]
        public IHttpActionResult Create_Update(Courier model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _courierRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)//Insert
                        {
                            if (_courierRepository.Exists(model.Courier_Name))
                                return Ok(new { result = "error", message = "Courier already exists !" });
                            else
                            {
                                model.Status = true;
                                _courierRepository.Create(model);
                                msg = "Successfully courier created!";
                            }
                        }
                        else//Update
                        {
                            var _courier = _courierRepository.Get(model.Id);
                            if (_courier != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_courier.RowVersion, 0))
                            {
                                if (_courierRepository.Exists(model.Id, model.Courier_Name))
                                    return Ok(new { result = "error", message = "Courier already exists !" });
                                else
                                {
                                    if (_courier != null)
                                    {
                                        _courier.Courier_Name = model.Courier_Name;
                                        _courier.Courier_Link = model.Courier_Link;
                                        _courierRepository.Update(_courier);
                                    }
                                    msg = "Successfully courier updated!";
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
                    var _courier = _courierRepository.Get(item.Id);
                    if (_courier != null)
                    {
                        _courier.Status = !_courier.Status;
                        _courierRepository.Update(_courier);
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
