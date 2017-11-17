using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SilverzoneERP.Api.api.School
{
    // [Authorize]
    public class EventManagementController : ApiController
    {
        IEventManagementRepository _eventManagementRepository;
        ICoOrdinatorRepository _coOrdinatorRepository;
        ICoOrdinatingTeacherRepository _coOrdinatorTeacherRepository;
        IEnrollmentOrderRepository _enrollmentOrderRepository;
        IBookRepository _bookRepository;
        IPurchaseOrder_MasterRepository _purchaseOrder_MasterRepository;
        IitemTitle_MasterRepository _itemTitle_MasterRepository;
        IinventorySourceDetailRepository _inventorySourceDetail;
        
        public EventManagementController(IEventManagementRepository _eventManagementRepository, ICoOrdinatorRepository _coOrdinatorRepository, ICoOrdinatingTeacherRepository _coOrdinatingTeacherRepository, IEnrollmentOrderRepository _enrollmentOrderRepository, IBookRepository _bookRepository, IPurchaseOrder_MasterRepository _purchaseOrder_MasterRepository, IitemTitle_MasterRepository _itemTitle_MasterRepository, IinventorySourceDetailRepository _inventorySourceDetail)
        {
            this._eventManagementRepository = _eventManagementRepository;
            this._coOrdinatorRepository = _coOrdinatorRepository;
            this._coOrdinatorTeacherRepository = _coOrdinatingTeacherRepository;
            this._enrollmentOrderRepository = _enrollmentOrderRepository;
            this._bookRepository = _bookRepository;
            this._purchaseOrder_MasterRepository = _purchaseOrder_MasterRepository;
            this._itemTitle_MasterRepository = _itemTitle_MasterRepository;
            this._inventorySourceDetail = _inventorySourceDetail;      
        }

        [HttpPost]
        public IHttpActionResult Create_Update(EventManagement model)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _eventManagementRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.CourierId == -1)
                            model.CourierId = null;
                        
                        if (model.Id==0 && model.SchId!=0)
                        {                                                        
                            var new_modal=_eventManagementRepository.Create(new EventManagement {
                                SchId=model.SchId,
                                EventManagementYear = DateTime.Now.Year,
                                EventId=model.EventId,
                                CourierId=model.CourierId,
                                PostalCommunication=model.PostalCommunication,
                                OtherCourier=model.OtherCourier,
                                IsParticipate = model.IsParticipate,
                                EnrollmentOrderSummary="[]",
                                Status = true
                            });

                            new_modal.RegNo = 1000 + new_modal.Id;
                            _eventManagementRepository.Update(new_modal);
                            msg = "Successfully School Participated !";
                        }
                        else
                        {                            
                            var _eventManagement=_eventManagementRepository.Get(model.Id);
                            
                            _eventManagement.PostalCommunication = model.PostalCommunication;
                            _eventManagement.CourierId = model.CourierId;
                            _eventManagement.OtherCourier = model.OtherCourier;
                            _eventManagement.IsParticipate = model.IsParticipate;

                            _eventManagementRepository.Update(_eventManagement);
                            
                            msg = "Successfully School Participated!";
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg, EventManagementId=model.Id });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "error", message = "Error" });
        }

        #region ==========================CoOrdinator Section=======================================
        [HttpPost]
        public IHttpActionResult CoOrdinator(long EventId,long SchoolId,CoOrdinator model)
        {
            if(ModelState.IsValid && EventId!=0 && SchoolId!=0)
            {
                using (var transaction = _eventManagementRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.EventManagementId == 0)
                        {
                            var _eventManagement = _eventManagementRepository.Create(new EventManagement
                            {
                                SchId = SchoolId,
                                EventId = EventId,
                                EnrollmentOrderSummary = "[]",
                                Status = true
                            });

                            model.EventManagementId = _eventManagement.Id;

                            _eventManagement.RegNo = 1000 + _eventManagement.Id;
                            _eventManagementRepository.Update(_eventManagement);
                        }
                        if (model.Id == 0)
                        {
                            model.Status = true;
                            _coOrdinatorRepository.Create(model);
                            msg = "Successfully coOrdinator added!";
                        }
                        else
                        {

                            var _coOrdinator = _coOrdinatorRepository.FindById(model.Id);
                            _coOrdinator.TitleId = model.TitleId;
                            _coOrdinator.CoOrdName = model.CoOrdName;
                            _coOrdinator.CoOrdMobile = model.CoOrdMobile;
                            _coOrdinator.CoOrdAltMobile1 = model.CoOrdAltMobile1;
                            _coOrdinator.CoOrdAltMobile2 = model.CoOrdAltMobile2;
                            _coOrdinator.CoOrdEmail = model.CoOrdEmail;
                            _coOrdinator.CoOrdAltEmail1 = model.CoOrdAltEmail1;
                            _coOrdinator.CoOrdAltEmail2 = model.CoOrdAltEmail2;

                            _coOrdinatorRepository.Update(_coOrdinator);
                            msg = "Successfully coOrdinator updated!";
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
            return Ok(new { result = "error", message = "Error" });
        }

        [HttpGet]
        public IHttpActionResult Delete_CoOrdinator(long CoOrdinatorId)
        {
            var _coOrdinator = _coOrdinatorRepository.FindById(CoOrdinatorId);
            if(_coOrdinator!=null)
            {
                _coOrdinator.Status = false;
                _coOrdinatorRepository.Update(_coOrdinator);
                return Ok(new { result = "Success", message = "Successfully CoOrdinator deleted !" });
            }
            else
            return Ok(new { result = "error", message = "CoOrdinator does not exists !" });            
        }
        #endregion

        #region ==========================CoOrdinating Teacher Section=======================================
        [HttpPost]
        public IHttpActionResult CoOrdinatingTeacher(long EventId, long SchoolId, CoOrdinatingTeacher model)
        {
            if (ModelState.IsValid && EventId != 0 && SchoolId != 0)
            {
                using (var transaction = _eventManagementRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.EventManagementId == 0)
                        {
                            var _eventManagement = _eventManagementRepository.Create(new EventManagement
                            {
                                SchId = SchoolId,
                                EventId = EventId,
                                EnrollmentOrderSummary = "[]",
                                Status = true
                            });

                            model.EventManagementId = _eventManagement.Id;

                            _eventManagement.RegNo = 1000 + _eventManagement.Id;
                            _eventManagementRepository.Update(_eventManagement);
                        }
                        if (model.Id == 0)
                        {
                            model.Status = true;
                            _coOrdinatorTeacherRepository.Create(model);
                            msg = "Successfully coOrdinator teacher added!";
                        }
                        else
                        {

                            var _coOrdinatorTeacher = _coOrdinatorTeacherRepository.FindById(model.Id);
                            _coOrdinatorTeacher.TitleId = model.TitleId;
                            _coOrdinatorTeacher.Name = model.Name;
                            _coOrdinatorTeacher.MobileNo = model.MobileNo;
                            _coOrdinatorTeacher.AltMobileNo1 = model.AltMobileNo1;
                            _coOrdinatorTeacher.AltMobileNo2 = model.AltMobileNo2;
                            _coOrdinatorTeacher.EmailID = model.EmailID;
                            _coOrdinatorTeacher.AltEmailID1 = model.AltEmailID1;
                            _coOrdinatorTeacher.AltEmailID2 = model.AltEmailID2;
                            _coOrdinatorTeacher.No_Of_Selected_Ques = model.No_Of_Selected_Ques;


                            _coOrdinatorTeacherRepository.Update(_coOrdinatorTeacher);
                            msg = "Successfully coOrdinator teacher updated!";
                        }
                        transaction.Commit();

                       
                        return Ok(new { result = "Success", message = msg,data= GetCoOrdinatorTeacher(model.EventManagementId) });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "error", message = "Error" });
        }

        [HttpGet]
        public IHttpActionResult Delete_CoOrdinatorTeacher(long CoOrdinatorTeacherId,bool IsLoad)
        {            
            var _coOrdinatorTeacher = _coOrdinatorTeacherRepository.FindById(CoOrdinatorTeacherId);

            if (_coOrdinatorTeacher != null)
            {
                _coOrdinatorTeacher.Status = false;
                _coOrdinatorTeacherRepository.Update(_coOrdinatorTeacher);
                return Ok(new {
                    result = "Success",
                    message = "Successfully CoOrdinator Teacher deleted !",
                    data= IsLoad?GetCoOrdinatorTeacher(_coOrdinatorTeacher.EventManagementId):null
                });
            }
            else
                return Ok(new { result = "error", message = "CoOrdinator does not exists !" });
        }

        private dynamic GetCoOrdinatorTeacher(long EventManagementId)
        {
            var data= _coOrdinatorTeacherRepository.FindBy(x => x.Status == true && x.EventManagementId == EventManagementId).ToList().Select(x => new {
                x.Id,
                x.EventManagementId,
                x.TitleId,
                x.Title.TitleName,
                x.Name,
                x.MobileNo,
                x.AltMobileNo1,
                x.AltMobileNo2,
                x.EmailID,
                x.AltEmailID1,
                x.AltEmailID2,
                x.No_Of_Selected_Ques,
                x.RowVersion
            });
            return data;
        }
        #endregion

        #region ==========================CoOrdinator Section=======================================
        [HttpPost]
        public IHttpActionResult EnrollmentOrder(long EventId, long SchoolId, EnrollmentOrderViewModel model)
        {            
            if (ModelState.IsValid && EventId != 0 && SchoolId != 0)
            {
                using (var transaction = _enrollmentOrderRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.EventManagementId == 0)
                        {
                            var _eventManagement = _eventManagementRepository.Create(new EventManagement
                            {
                                SchId = SchoolId,
                                EventId = EventId,
                                EnrollmentOrderSummary = "[]",
                                Status = true
                            });

                            model.EventManagementId = _eventManagement.Id;

                            _eventManagement.RegNo = 1000 + _eventManagement.Id;
                            _eventManagementRepository.Update(_eventManagement);
                        }
                        

                        if (model.Id == 0)
                        {
                            EnrollmentOrder _enrollmentOrder = new Entities.Models.EnrollmentOrder();
                            _enrollmentOrder.EventManagementId = model.EventManagementId;
                            _enrollmentOrder.TotlaEnrollment = model.TotlaEnrollment;
                            _enrollmentOrder.EnrollmentOrderDetail = new JavaScriptSerializer().Serialize(model.EnrollmentOrderDetail);
                            _enrollmentOrder.ExaminationDateId = model.ExaminationDateId;
                            _enrollmentOrder.ChangeExamDate = model.ChangeExamDate;


                            _enrollmentOrder.Status = true;
                            _enrollmentOrder.OrderNo = 1000 + _enrollmentOrderRepository.FindBy(x => x.EventManagementId == model.EventManagementId).Count();
                                                       
                            _enrollmentOrderRepository.Create(_enrollmentOrder);
                            msg = "Successfully enrollment order added!";
                        }
                        else
                        {

                            var _enrollmentOrder = _enrollmentOrderRepository.FindById(model.Id);
                            _enrollmentOrder.TotlaEnrollment = model.TotlaEnrollment;
                            _enrollmentOrder.EnrollmentOrderDetail = new JavaScriptSerializer().Serialize(model.EnrollmentOrderDetail);
                            _enrollmentOrder.ExaminationDateId = model.ExaminationDateId;
                            _enrollmentOrder.ChangeExamDate = model.ChangeExamDate;

                            _enrollmentOrderRepository.Update(_enrollmentOrder);
                            msg = "Successfully enrollment order updated!";
                        }
                        var _eventMngt = _eventManagementRepository.FindById(model.EventManagementId);

                        JavaScriptSerializer jss = new JavaScriptSerializer();

                        List<EnrollmentOrderSummary> EnrollmentOrderSummary = new List<EnrollmentOrderSummary>();

                        var  _enrollmentOrderSummary = _enrollmentOrderRepository.FindBy(x => x.EventManagementId == _eventMngt.Id).ToList();
                        foreach (var item in _enrollmentOrderSummary)
                        {
                           var _enrollmentOrderDetail = jss.Deserialize<List<EnrollmentOrderSummary>>(item.EnrollmentOrderDetail);
                            foreach (var data in _enrollmentOrderDetail)
                            {
                                var _class = EnrollmentOrderSummary.Find(x => x.ClassId == data.ClassId);
                                if (_class != null)
                                {
                                    _class.No_Of_Student += data.No_Of_Student;
                                }
                                else
                                {
                                    EnrollmentOrderSummary.Add(new EnrollmentOrderSummary
                                    {
                                        ClassId = data.ClassId,
                                        ClassName = data.ClassName,
                                        No_Of_Student = data.No_Of_Student
                                    });
                                }
                            }
                        }
                        _eventMngt.EnrollmentOrderSummary = new JavaScriptSerializer().Serialize(EnrollmentOrderSummary);
                        _eventMngt.TotalEnrollmentSummary = EnrollmentOrderSummary.Sum(x => x.No_Of_Student);

                        var ExamDateList = _enrollmentOrderRepository.FindBy(x => x.EventManagementId == _eventMngt.Id & x.ExaminationDateId != -2).Select(x => new
                        {
                            ExamDate = x.ExaminationDateId == -1 ? x.ChangeExamDate : x.ExaminationDate.ExamDate
                        }).Distinct().ToArray();

                        if (ExamDateList.Count() == 1)
                            _eventMngt.ExamDate = ExamDateList[0].ExamDate;
                        else
                            _eventMngt.ExamDate = null;


                        _eventManagementRepository.Update(_eventMngt);
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
            return Ok(new { result = "error", message = "Error" });
        }

        [HttpGet]
        public IHttpActionResult Delete_EnrollmentOrder(long CoOrdinatorId)
        {
            var _coOrdinator = _coOrdinatorRepository.FindById(CoOrdinatorId);
            if (_coOrdinator != null)
            {
                _coOrdinator.Status = false;
                _coOrdinatorRepository.Update(_coOrdinator);
                return Ok(new { result = "Success", message = "Successfully CoOrdinator deleted !" });
            }
            else
                return Ok(new { result = "error", message = "CoOrdinator does not exists !" });
        }
        #endregion


        [HttpGet]
        public IHttpActionResult Get_FavourOf()
        {
            return Ok(new {
                FavourOf = _eventManagementRepository.Get_FavourOf(),
                DrawnOnBank = _eventManagementRepository.Get_DrawnOnBank()
            });
        }

        [HttpGet]
        public IHttpActionResult AllOrderBySchool(long EventManagementId)
        {
            var data = _purchaseOrder_MasterRepository.FindBy(x => x.From == orderSourceType.School && x.srcFrom == EventManagementId && x.To == orderSourceType.Silverzone && x.PurchaseOrders.Count(p=>p.Status==true)!=0).OrderByDescending(x=>x.PO_Date).Select(x => new {
                x.Id,
                x.PO_Number,
                x.PO_Date,
                x.isVerified,
                Order=x.PurchaseOrders.Where(p=>p.Status==true).Select(p=>new {
                    p.Id,
                    p.PO_mId,
                    p.Book.ItemTitle_Master.ClassId,
                    ClassName = p.Book.ItemTitle_Master.Class.className,
                    p.BookId,
                    BookName = p.Book.ItemTitle_Master.BookCategory.Name,
                    p.Quantity,
                    p.Rate,
                    p.Status
                })
            });
            return Ok(new { result = data });
        }

        [HttpGet]
        public IHttpActionResult GetAllBook(long SubjectId)
        {
            var data = _bookRepository.FindBy(x => x.Status == true && x.ItemTitle_Master.SubjectId==SubjectId).Select(x => new {
                BookId= x.Id,
                x.ItemTitle_Master.ClassId,
                x.ItemTitle_Master.SubjectId,
                x.ItemTitle_Master.CategoryId,
                CategoryName = x.ItemTitle_Master.BookCategory.Name,
            });

            var CompanyId = _inventorySourceDetail.FindBy(x => x.SourceName.Trim().ToLower().Equals("silverzone foundation") && x.SourceId==7).FirstOrDefault().Id;
            return Ok(new { result = data, CompanyId= CompanyId });
        }

        [HttpGet]
        public IHttpActionResult GetPurchaseOrder(long Id)
        {
            var data = _purchaseOrder_MasterRepository.FindById(Id)
                .PurchaseOrders.Select(x => new
                {
                    x.Id,
                    x.PO_mId,
                    x.Book.ItemTitle_Master.ClassId,
                    ClassName = x.Book.ItemTitle_Master.Class.className,
                    x.BookId,
                    BookName = x.Book.ItemTitle_Master.BookCategory.Name,
                    x.Quantity,
                    x.Rate
                });

            return Ok(new { result = data });
        }
    }
}
