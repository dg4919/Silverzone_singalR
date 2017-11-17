using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    //[Authorize]
    public class feePaymentController : ApiController
    {
        IFeePaymentRepository _feePaymentRepository;
        IDrownOnBankRepository _drownOnBankRepository;
        public feePaymentController(IFeePaymentRepository _feePaymentRepository, IDrownOnBankRepository _drownOnBankRepository)
        {
            this._feePaymentRepository = _feePaymentRepository;
            this._drownOnBankRepository = _drownOnBankRepository;
        }
        [HttpGet]
        public IHttpActionResult MiniStatement(long SchoolId)
        {
            return Ok(new
            {
                Registration = _feePaymentRepository.MiniStatement(SchoolId, PaymentAgainst.Registration),
                Book = _feePaymentRepository.MiniStatement(SchoolId, PaymentAgainst.Book),
                Both =_feePaymentRepository.MiniStatement(SchoolId, PaymentAgainst.Both)
            });
        }
        [HttpGet]
        public IHttpActionResult History(long SchoolId)
        {
            return Ok(new { result = _feePaymentRepository.History(SchoolId) });
        }
        [HttpPost]
        public IHttpActionResult Fee(FeePayment model)
        {
          
            if(ModelState.IsValid)
            {
                try
                {
                    if(model.DrawnOnBankId==-1)
                    {
                        var _drawnOnBank = _drownOnBankRepository.FindBy(x=>x.BankName.Trim().ToLower().Equals(model.OtherBank.Trim().ToLower())).FirstOrDefault();
                        if (_drawnOnBank == null)
                        {
                            model.DrawnOnBankId = _drownOnBankRepository.Create(new DrownOnBank
                            {
                                BankName = model.OtherBank,
                                Status = true
                            }).Id;
                        }
                        else
                            model.DrawnOnBankId = _drawnOnBank.Id;
                    }
                    model.Status = true;
                    model= _feePaymentRepository.Create(model);
                    model.ReceiptNo = 999 + model.Id;
                    _feePaymentRepository.Update(model);
                    return Ok(new { result = "Success", message = "Payment is Successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new { result = "Error",message = ex.Message });
                }               
            }
            return Ok(new { result = "Error", message="Error" });
        }
    }
}
