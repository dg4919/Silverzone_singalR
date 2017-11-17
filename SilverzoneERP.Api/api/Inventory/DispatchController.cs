using SilverzoneERP.Data;
using SilverzoneERP.Entities.ViewModel.Inventory;
using System;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Inventory
{
    public class DispatchController : ApiController
    {
        IDispatch_otherItem_MasterRepository dispatch_otherItem_MasterRepository { get; set; }
        IDispatch_otherItem_addressRepository dispatch_otherItem_addressRepository { get; set; }
        ISchoolRepository schoolRepository { get; set; }
        IErrorLogsRepository errorLogsRepository { get; set; }
        ICityRepository cityRepository { get; set; }

        [HttpPost]
        public IHttpActionResult create_dispatch_otherItem(DispatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = dispatch_otherItem_MasterRepository.BeginTransaction())
                {
                    try
                    {
                        long dispatchId = dispatch_otherItem_MasterRepository.Create(DispatchViewModel.parse(model)).Id;

                        var _adresmodel = DispatchViewModel.parseA(model);
                        _adresmodel.Id = dispatchId;
                        dispatch_otherItem_addressRepository.Create(_adresmodel);

                        transaction.Commit();
                        return Ok(new { result = "ok" }); ;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        errorLogsRepository.logError(ex);
                        return Ok(new { result = "error" });
                    }
                }
                    
            }
            return Ok(new { result = "error" }); ;
        }

        [HttpGet]
        public IHttpActionResult get_schoolBy_SchCode(long schCode)
        {
            var model = schoolRepository.findBySchCode(schCode);
            if (model == null)
                return Ok(new { result = "notfound" });

            return Ok(new
            {
                result = DispatchViewModel.parse(model)
            });
        }

        [HttpGet]
        public IHttpActionResult get_otherDispatch_Items()
        {
            var dispatchList = dispatch_otherItem_addressRepository.GetAll();
            var citys = cityRepository.GetAll();

            return Ok(new
            {
                result = DispatchViewModel.parse(dispatchList, citys)
            });
        }


        public DispatchController(IDispatch_otherItem_MasterRepository _dispatch_otherItem_MasterRepository,
                                  IDispatch_otherItem_addressRepository _dispatch_otherItem_addressRepository,
                                  ISchoolRepository _schoolRepository,
                                  IErrorLogsRepository _errorLogsRepository,
                                  ICityRepository _cityRepository)
        {
            dispatch_otherItem_MasterRepository = _dispatch_otherItem_MasterRepository;
            dispatch_otherItem_addressRepository = _dispatch_otherItem_addressRepository;
            schoolRepository = _schoolRepository;
            errorLogsRepository = _errorLogsRepository;
            cityRepository = _cityRepository;
        }


    }
}
