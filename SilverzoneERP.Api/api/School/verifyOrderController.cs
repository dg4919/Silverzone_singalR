using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class verifyOrderController : ApiController
    {
        IBookRepository _bookRepository;
        IPurchaseOrder_MasterRepository _purchaseOrder_MasterRepository;
        IPurchaseOrderRepository _purchaseOrderRepository;
        IitemTitle_MasterRepository _itemTitle_MasterRepository;
        IinventorySourceDetailRepository _inventorySourceDetail;
        ISchoolRepository _schoolRepository;
        public verifyOrderController(IBookRepository _bookRepository, IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseOrder_MasterRepository _purchaseOrder_MasterRepository, IitemTitle_MasterRepository _itemTitle_MasterRepository, IinventorySourceDetailRepository _inventorySourceDetail, ISchoolRepository _schoolRepository)
        {
            this._bookRepository = _bookRepository;
            this._purchaseOrder_MasterRepository = _purchaseOrder_MasterRepository;
            this. _purchaseOrderRepository = _purchaseOrderRepository;
            this._itemTitle_MasterRepository = _itemTitle_MasterRepository;
            this._inventorySourceDetail = _inventorySourceDetail;
            this._schoolRepository = _schoolRepository;
        }

        [HttpGet]
        public IHttpActionResult GetOrder()
        {
            var data = _purchaseOrder_MasterRepository.FindBy(x => x.From == orderSourceType.School && x.To == orderSourceType.Silverzone && x.PurchaseOrders.Count(p => p.Status == true) != 0 && x.isVerified==false).OrderByDescending(x => x.PO_Date).ToList().Select(x => new {
                PO_mId = x.Id,
                x.PO_Number,
                x.PO_Date,
                x.isVerified,
                School = _schoolRepository.GetSchool(x.srcFrom),
                Order = x.PurchaseOrders.Where(p => p.Status == true).Select(p => new {
                    POId = p.Id,
                    PO_masterId=p.PO_mId,
                    p.Book.ItemTitle_Master.ClassId,
                    ClassName = p.Book.ItemTitle_Master.Class.className,
                    p.BookId,
                    BookName = p.Book.ItemTitle_Master.BookCategory.Name,
                    Quantity_verify = p.Quantity,
                    Quantity = "",
                    p.Rate,
                    p.Status
                })
            });
            return Ok(new { result = data });
        }

        [HttpPost]
        public IHttpActionResult Verify(long PO_MId,List<PurchaseOrder_ViewModel> model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _purchaseOrder_MasterRepository.BeginTransaction())
                {
                    try
                    {
                       
                        var POM = _purchaseOrder_MasterRepository.FindById(PO_MId);
                        POM.isVerified = true;
                        _purchaseOrder_MasterRepository.Update(POM);
                        foreach (PurchaseOrder_ViewModel item in model)
                        {
                            if(item.POId==0)
                            {
                                _purchaseOrderRepository.Create(new PurchaseOrder {
                                    PO_mId=(long)item.PO_masterId,
                                    BookId=item.BookId,
                                    Quantity=item.Quantity,
                                    Status=true
                                });
                            }
                            else
                            {
                                var po = _purchaseOrderRepository.FindById(item.POId);
                                po.Quantity = item.Quantity;
                                _purchaseOrderRepository.Update(po);
                            }                            
                        }

                        transaction.Commit();
                        return Ok(new { result = "Success", message = "Successfully verified !" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });
                    }                    
                }                
            }
            return Ok(new { result = "error" });
        }
    }
}
