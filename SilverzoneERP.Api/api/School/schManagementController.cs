using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using Newtonsoft.Json.Linq;

namespace SilverzoneERP.Api.api.School
{
    //[Authorize]
    public class schManagementController : ApiController
    {
        IDesignationRepository _designationRepository;
        ITitleRepository _titleRepository;
        IEventYearRepository _eventYearRepository;
        ICityRepository _cityRepository;
        ISchoolRepository _schoolRepository;
        IContactRepository _contactRepository;
        IEventManagementRepository _eventManagementRepository;
        ICoOrdinatorRepository _coOrdinatorRepository;
        IStateRepository _stateRepository;
        IBlackListedRepository _blackListedRepository;
        ISchoolCategoryRepository _schoolCategoryRepository;
        ISchoolGroupRepository _schoolGroupRepository;
        IUserRepository _userRepository;

        public schManagementController(IDesignationRepository _designationRepository, ITitleRepository _titleRepository, IEventYearRepository _eventYearRepository, ICityRepository _cityRepository, ISchoolRepository _schoolRepository, IContactRepository _contactRepository, IEventManagementRepository _eventManagementRepository, ICoOrdinatorRepository _coOrdinatorRepository, IStateRepository _stateRepository, IBlackListedRepository _blackListedRepository, ISchoolCategoryRepository _schoolCategoryRepository, ISchoolGroupRepository _schoolGroupRepository, IUserRepository _userRepository)
        {
            this._designationRepository = _designationRepository;
            this._titleRepository = _titleRepository;
            this._eventYearRepository = _eventYearRepository;
            this._cityRepository = _cityRepository;
            this._schoolRepository = _schoolRepository;
            this._contactRepository = _contactRepository;
            this._eventManagementRepository = _eventManagementRepository;
            this._coOrdinatorRepository = _coOrdinatorRepository;
            this._stateRepository = _stateRepository;
            this._blackListedRepository = _blackListedRepository;
            this._schoolCategoryRepository = _schoolCategoryRepository;
            this._schoolGroupRepository = _schoolGroupRepository;
            this._userRepository = _userRepository;
        }
        [HttpGet]
        public string GetLastGenSchoolCode()
        {
            try
            {
                return _schoolRepository.LastGenCode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult Get_Related_Object()
        {
            try
            {
                return Ok(new
                {
                    result = new
                    {
                        Designation = _designationRepository.Get(true),
                        Title = _titleRepository.Get(true),
                        Event = _eventYearRepository.Get(DateTime.Now.Year, true),
                        SchoolCategory = _schoolCategoryRepository.Get(true),
                        SchoolGroup = _schoolGroupRepository.Get(true)
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult GetCity()
        {
            try
            {
                var data = from c in _cityRepository.FindBy(x => x.Status == true)
                           select new
                           {
                               CityId = c.Id,
                               c.CityName,
                               c.DistrictId,
                               c.District.DistrictName,
                               c.StateId,
                               c.State.StateName,
                               c.ZoneId,
                               c.Zone.ZoneName,
                               c.CountryId,
                               c.Country.CountryName
                           };
                return Ok(new { result = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult CityValidate(string CityName, long DistrictId)
        {
            try
            {
                var data = _cityRepository.FindBy(x => x.CityName == CityName && x.DistrictId == DistrictId).ToList();
                return Ok(new { result = data.Count == 0 ? true : false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult GetLog(long SchId, int StartIndex, int Limit)
        {
            try
            {
                var data = _schoolRepository.GetHistory(SchId, StartIndex, Limit);
                return Ok(new { result = data });
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        [HttpGet]
        public IHttpActionResult GetSchool(long SchCode)
        {
            try
            {
                // var asa = _blackListedRepository.Get_Latest_BlackListd(2);

                var data = from s in _schoolRepository.FindBy(x => x.SchCode == SchCode && x.Status == true)
                           .ToList()
                           select new
                           {
                               SchId = s.Id,
                               s.SchCode,
                               s.SchName,
                               s.SchEmail,
                               s.SchAddress,
                               s.SchAltAddress,
                               s.CityId,
                               s.City.CityName,
                               s.DistrictId,
                               s.District.DistrictName,
                               s.StateId,
                               s.State.StateName,
                               s.ZoneId,
                               s.Zone.ZoneName,
                               s.CountryId,
                               s.Country.CountryName,
                               SchPinCode = "" + s.SchPinCode,
                               SchPhoneNo = "" + s.SchPhoneNo,
                               SchFaxNo = "" + s.SchFaxNo,
                               s.SchWebSite,
                               s.SchCategoryId,
                               s.SchGroupId,
                               s.SchBoard,
                               s.SchAffiliationNo,
                               BlackListed = _blackListedRepository.Get_Latest_BlackListd(s.Id),

                               Contact_List = s.Contact.Where(c => c.ContactType == 0 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {
                                   AddressGuid = Guid.NewGuid(),
                                   c.DesgId,
                                   c.Designation.DesgName,
                                   c.TitleId,
                                   Title = c.Title.TitleName,
                                   c.ContactName,
                                   c.ContactMobile,
                                   c.ContactAltMobile1,
                                   c.ContactAltMobile2,
                                   c.ContactEmail,
                                   c.ContactAltEmail1,
                                   c.ContactAltEmail2,
                                   c.ContactType,
                                   AddressTo = ("" + c.AddressTo).ToLower()
                               }),
                               IsOtherContact = s.Contact.Count(c => c.ContactType == 1 && c.Status == true && c.ContactYear == DateTime.Now.Year) != 0 ? true : false,
                               OtherContact = s.Contact.Where(c => c.ContactType == 1 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {
                                   AddressGuid = Guid.NewGuid(),
                                   c.DesgId,
                                   c.Designation.DesgName,
                                   c.TitleId,
                                   Title = c.Title.TitleName,
                                   c.ContactName,
                                   c.ContactMobile,
                                   c.ContactAltMobile1,
                                   c.ContactAltMobile2,
                                   c.ContactEmail,
                                   c.ContactAltEmail1,
                                   c.ContactAltEmail2,
                                   c.ContactType,
                                   c.AddressTo
                               }).FirstOrDefault(),
                               Events = s.EventManagement.Where(e => e.Status == true && e.EventManagementYear == DateTime.Now.Year).Select(ec => new
                               {
                                   EventGuid = Guid.NewGuid(),
                                   ec.EventId,
                                   ec.Event.EventCode,
                                   CoOrdinators = ec.CoOrdinator.Where(e => e.Status == true ).Select(c => new
                                   {
                                       CoOrdinatorGuid = Guid.NewGuid(),
                                       c.TitleId,
                                       CoOrdTitle = c.Title.TitleName,
                                       c.CoOrdName,
                                       c.CoOrdMobile,
                                       c.CoOrdAltMobile1,
                                       c.CoOrdAltMobile2,
                                       c.CoOrdEmail,
                                       c.CoOrdAltEmail1,
                                       c.CoOrdAltEmail2
                                   })
                               })

                           };
                return Ok(new { result = data.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult GetSchool(long SchCode, long EventId)
        {
            try
            {
                var data = from s in _schoolRepository.FindBy(x => x.SchCode == SchCode && x.Status == true)
                           .ToList()
                           select new
                           {
                               SchId = s.Id,
                               s.SchCode,
                               s.SchName,
                               s.SchEmail,
                               s.SchAddress,
                               s.SchAltAddress,
                               s.CityId,
                               s.City.CityName,
                               s.DistrictId,
                               s.District.DistrictName,
                               s.StateId,
                               s.State.StateName,
                               s.ZoneId,
                               s.Zone.ZoneName,
                               s.CountryId,
                               s.Country.CountryName,
                               SchPinCode = "" + s.SchPinCode,
                               SchPhoneNo = "" + s.SchPhoneNo,
                               SchFaxNo = "" + s.SchFaxNo,
                               s.SchWebSite,
                               s.SchCategoryId,
                               s.SchGroupId,
                               s.SchBoard,
                               s.SchAffiliationNo,                               
                               BlackListed = _blackListedRepository.Get_Latest_BlackListd(s.Id),

                               Contact_List = s.Contact.Where(c => c.ContactType == 0 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {                                   
                                   c.DesgId,
                                   c.Designation.DesgName,
                                   c.TitleId,
                                   Title = c.Title.TitleName,
                                   c.ContactName,
                                   c.ContactMobile,
                                   c.ContactAltMobile1,
                                   c.ContactAltMobile2,
                                   c.ContactEmail,
                                   c.ContactAltEmail1,
                                   c.ContactAltEmail2,
                                   c.ContactType,
                                   AddressTo = ("" + c.AddressTo).ToLower()
                               }),
                               HeadMaster = (s.Contact.Where(c => c.Designation.DesgName.ToLower() == "head master" && c.ContactType == 0 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {
                                   Name = c.Title.TitleName + " " + c.ContactName,
                               })).FirstOrDefault(),

                               Principal = (s.Contact.Where(c => c.Designation.DesgName.ToLower() == "principal" && c.ContactType == 0 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {
                                   Name = c.Title.TitleName + " " + c.ContactName,
                               })).FirstOrDefault(),

                               ExternalIncharge = (s.Contact.Where(c => c.Designation.DesgName.ToLower() == "external incharge" && c.ContactType == 0 && c.Status == true && c.ContactYear == DateTime.Now.Year).Select(c => new
                               {
                                   Name = c.Title.TitleName + " " + c.ContactName,
                                   c.ContactMobile,
                                   c.ContactAltMobile1,
                                   c.ContactAltMobile2
                               })).FirstOrDefault(),
                               EventManagement = s.EventManagement.Where(e => e.Event.Id == EventId && e.Status == true && e.EventManagementYear == DateTime.Now.Year)
                               .ToList().Select(ec => new
                               {
                                   ec.Id,
                                   ec.RegNo,
                                   ec.EventId,
                                   ec.Event.EventCode,
                                   ec.EventManagementYear,
                                   ec.TotalEnrollmentSummary,
                                   ec.IsParticipate,
                                   EnrollmentOrderSummary = JArray.Parse(ec.EnrollmentOrderSummary == null ? "[]" : ec.EnrollmentOrderSummary),
                                   CoOrdinator = ec.CoOrdinator.Where(e => e.Status == true).Select(c => new
                                   {
                                       c.Id,                                
                                       c.EventManagementId,
                                       c.TitleId,
                                       CoOrdTitle = c.Title.TitleName,
                                       c.CoOrdName,
                                       c.CoOrdMobile,
                                       c.CoOrdAltMobile1,
                                       c.CoOrdAltMobile2,
                                       c.CoOrdEmail,
                                       c.CoOrdAltEmail1,
                                       c.CoOrdAltEmail2

                                   }),
                                   CoOrdinatingTeacher = ec.CoOrdinatingTeacher.Where(ct => ct.Status == true)
                                   .Select(x => new
                                    {
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
                                    }),                                   
                                   EnrollmentOrder=ec.EnrollmentOrder.Select(x=>new {
                                       x.Id,
                                       x.EventManagementId,
                                       x.OrderNo,
                                       x.OrderDate,
                                       x.TotlaEnrollment,
                                       EnrollmentOrderDetail= JArray.Parse(x.EnrollmentOrderDetail == null ? "[]" : x.EnrollmentOrderDetail),
                                       x.ExaminationDateId,
                                       ExamDate= x.ExaminationDate!=null?( x.ExaminationDate.ExamDate==null?null:x.ExaminationDate.ExamDate.ToString("dd-MMM-yyyy")):null,                                       
                                       x.ChangeExamDate
                                   }),                                  
                                   
                                   PostalCommunication=""+ec.PostalCommunication,
                                   CourierId=""+(ec.CourierId==null?-1:ec.CourierId),
                                    ec.OtherCourier
                               }).FirstOrDefault(),

                           };
                return Ok(new
                {
                    result = data.FirstOrDefault(),
                    CourierList = _schoolRepository.GetCourier(),
                    ExamDateList = _schoolRepository.GetExamDate()
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string Get_ExaminationDateId(Nullable<long> ExaminationDateId,Nullable<DateTime> ChangeExamDate)
        {
            if (ExaminationDateId == null && ChangeExamDate==null)
                return "-2";
            else if (ExaminationDateId == null && ChangeExamDate != null)
                return "-1";
            return null;
        }

        [HttpPost]
        public IHttpActionResult Create_Update(SchMngtViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _schoolRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        
                        if (model.SchId == 0)
                        {
                            dynamic ExistingSchool = GetSchoolList(model, false);
                            if (ExistingSchool.Count != 0)
                                return Ok(new { result = "list", data = ExistingSchool });
                            Entities.Models.School _school = new Entities.Models.School();
                            _school.SchCode = 9999;
                            _school.Status = true;
                            Set_School_Value(model, ref _school);

                            _school = _schoolRepository.Create(_school);

                            model.SchId = _school.Id;
                            _school.SchCode += _school.Id;
                            _schoolRepository.Update(_school);

                            if (model.BlackListed != null && model.BlackListed.IsBlocked)
                            {
                                Add_BlackListed(model);
                            }
                            msg = "Successfully school created!";
                        }
                        else
                        {
                            var ExistingSchool = GetSchoolList(model, true);
                            if (ExistingSchool.Count != 0)
                                return Ok(new { result = "list", data = ExistingSchool });

                            var _school = _schoolRepository.FindBy(x => x.Id == model.SchId).FirstOrDefault();
                            if (_school != null)
                            {
                                Set_School_Value(model, ref _school);

                                _schoolRepository.Update(_school);
                            }

                            if (model.BlackListed != null && _blackListedRepository.IsChange(model.SchId, model.BlackListed.IsBlocked))
                            {
                                Add_BlackListed(model);
                            }

                            //Delete  Contact

                            var _contactList = _contactRepository.GetBySchoolId(model.SchId);
                            if (_contactList.Count != 0)
                            {
                                _contactRepository.DeleteWhere(_contactList);
                                _contactRepository.Save();
                            }
                           
                            msg = "Successfully school updated!";
                        }
                        if (model.SchId != 0)
                        {
                            //Add Contact 
                            foreach (var _contact in model.Contact_List)
                            {
                                _contact.ContactType = 0;
                                Add_Contact(model, _contact);
                            }
                            //Add Other Contact
                            if (model.IsOtherContact)
                            {
                                model.OtherContact.ContactType = 1;
                                Add_Contact(model, model.OtherContact);
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

        [HttpGet]
        public IHttpActionResult Search(
            string Anything,
            string SchoolName,
            string SchoolCode,
            Nullable<long> CountryId,
            Nullable<long> ZoneId,
            Nullable<long> StateId,
            Nullable<long> DistrictId,
            Nullable<long> CityId,
            Nullable<long> PinCode,
            Nullable<bool> BlackListed,
            Nullable<int> EventType,
            Nullable<long> EventId,
            int StartIndex,
            int Limit
            )
        {
            try
            {
                int count = 0;
                var data = _schoolRepository.Search(Anything, SchoolCode, SchoolName, CountryId, ZoneId, StateId, DistrictId, CityId, PinCode, BlackListed, EventType, EventId,StartIndex,Limit,ref count);
                return Ok(new { result = data,Count= count });
            }
            catch (Exception ex)
            {
                return Ok(new { result = "error", message = ex.Message });
            }
        }

        private void Add_BlackListed(SchMngtViewModel model)
        {
            try
            {
                _blackListedRepository.Create(new BlackListedSchool
                {
                    SchId = model.SchId,
                    IsBlocked = model.BlackListed.IsBlocked,
                    BlackListedRemarks = model.BlackListed.BlackListedRemarks
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Add_Contact(SchMngtViewModel model, ContactViewModel _contact)
        {
            try
            {
                _contactRepository.Create(new Contact
                {
                    SchId = model.SchId,
                    DesgId = _contact.DesgId,
                    TitleId = _contact.TitleId,
                    ContactName = _contact.ContactName,
                    ContactMobile = _contact.ContactMobile,
                    ContactAltMobile1 = _contact.ContactAltMobile1,
                    ContactAltMobile2 = _contact.ContactAltMobile2,
                    ContactEmail = _contact.ContactEmail,
                    ContactAltEmail1 = _contact.ContactAltEmail1,
                    ContactAltEmail2 = _contact.ContactAltEmail2,
                    AddressTo = _contact.AddressTo,
                    ContactType = _contact.ContactType,
                    Status = true,
                    ContactYear = DateTime.Now.Year
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private dynamic GetSchoolList(SchMngtViewModel model, bool IsEdit = false)
        {
            try
            {
                if (IsEdit)
                    return (from s in _schoolRepository.FindBy(x => x.Id != model.SchId
                            && x.SchName.ToLower() == model.SchName.ToLower()
                            && x.CountryId == model.CountryId
                            && x.StateId == model.StateId
                            && x.DistrictId == model.DistrictId
                            && x.CityId == model.CityId
                            && x.SchPinCode == model.SchPinCode)
                            select new
                            {
                                s.SchCode,
                                s.SchName,
                                s.SchEmail,
                                s.SchAddress,
                                s.Country.CountryName,
                                s.District.DistrictName,
                                s.State.StateName,
                                s.City.CityName
                            }).ToList();
                else
                    return (from s in _schoolRepository.FindBy(x => x.SchName.ToLower() == model.SchName.ToLower()
                        && x.CountryId == model.CountryId
                        && x.StateId == model.StateId
                        && x.DistrictId == model.DistrictId
                        && x.CityId == model.CityId
                        && x.SchPinCode == model.SchPinCode)
                            select new
                            {
                                s.SchCode,
                                s.SchName,
                                s.SchEmail,
                                s.SchAddress,
                                s.Country.CountryName,
                                s.District.DistrictName,
                                s.State.StateName,
                                s.City.CityName
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult GetSchoolList(long PinCode)
        {
            try
            {
                 var data=from s in _schoolRepository.FindBy(x => x.SchPinCode == PinCode)
                        select new
                        {
                            s.SchCode,
                            s.SchName,
                            s.SchEmail,
                            s.SchAddress,
                            s.Country.CountryName,
                            s.District.DistrictName,
                            s.State.StateName,
                            s.City.CityName
                        };
                return Ok(new { result = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Set_School_Value(SchMngtViewModel model, ref Entities.Models.School _school)
        {
            _school.SchName = model.SchName;
            _school.SchEmail = model.SchEmail;
            _school.SchAddress = model.SchAddress;
            _school.SchAltAddress = model.SchAltAddress;
            _school.CityId = model.CityId;
            _school.DistrictId = model.DistrictId;
            _school.StateId = model.StateId;
            _school.ZoneId = model.ZoneId;
            _school.CountryId = model.CountryId;
            _school.SchPinCode = model.SchPinCode;
            _school.SchPhoneNo = model.SchPhoneNo;
            _school.SchFaxNo = model.SchFaxNo;
            _school.SchWebSite = model.SchWebSite;
            _school.SchBoard = model.SchBoard;
            _school.SchAffiliationNo = model.SchAffiliationNo;
            _school.SchCategoryId = model.SchCategoryId;
            _school.SchGroupId = model.SchGroupId;
        }
    }
}
