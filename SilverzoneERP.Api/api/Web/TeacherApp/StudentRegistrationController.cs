using SilverzoneERP.Entities.ViewModel.TeacherApp;
using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.TeacherApp
{
    [Authorize]
    public class StudentRegistrationController : ApiController
    {
        private IMasterAcademicYearRepository masterAcademicYearRepository;
        private IStudentRegistrationRepository studentRegistrationRepository;
        private IAccountRepository accountRepository;
        private ITeacherDetailRepository teacherDetailRepository;
        private IEventRepository eventRepository;
        private ISchoolEventRepository schoolEventRepository;
        private IQueryRepository queryRepository;
        private IBookDispatchRepository bookDispatchRepository;
        private IQPDispatchRepository qpDispatchRepository;
        private IEventCodeImagePathRepository eventCodeImagePathRepository;
        private IResultRepository resultRepository;
        private IOlympiadListRepository olympiadListRepository;
        private ITeacherEventRepository teacherEventRepository;
        private IProfileRepository profileRepository;
        
        public StudentRegistrationController(IMasterAcademicYearRepository _masterAcademicYearRepository,IStudentRegistrationRepository _studentRegistrationRepository,IAccountRepository _accountRepository,ITeacherDetailRepository  _teacherDetailRepository,IEventRepository _eventRepository,ISchoolEventRepository _schoolEventRepository,IQueryRepository _queryRepository,IBookDispatchRepository _bookDispatchRepository,IQPDispatchRepository _qpDispatchRepository,IEventCodeImagePathRepository _eventCodeImagePathRepository,IResultRepository _resultRepository,IOlympiadListRepository _olympiadListRepository,ITeacherEventRepository _teacherEventRepository,IProfileRepository _profileRepository)
        {
            masterAcademicYearRepository = _masterAcademicYearRepository;
            studentRegistrationRepository = _studentRegistrationRepository;
            accountRepository = _accountRepository;
            teacherDetailRepository = _teacherDetailRepository;
            eventRepository = _eventRepository;
            schoolEventRepository = _schoolEventRepository;
            queryRepository = _queryRepository;
           bookDispatchRepository = _bookDispatchRepository;
            qpDispatchRepository = _qpDispatchRepository;
            eventCodeImagePathRepository = _eventCodeImagePathRepository;
            resultRepository = _resultRepository;
            olympiadListRepository = _olympiadListRepository;
            teacherEventRepository = _teacherEventRepository;
            profileRepository = _profileRepository;
        }

      
       //public IHttpActionResult GetSchoolEvent()
       // {
       //     Boolean flag;
       //     var userId =Convert.ToInt32(User.Identity.Name);

       //     // var aCode =teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();
       //     // var tcode = aCode.SchoolCode;
       //     var data = (from tbl1 in schoolEventRepository.GetAll()
       //                 join tbl2 in eventRepository.GetAll() on new { EventId = tbl1.EventId } equals new { EventId = tbl2.Id }
       //                 join tbl3 in eventCodeImagePathRepository.GetAll() on new { EventId = tbl2.Id } equals new { EventId =tbl3.EventId }
       //                 join tbl4 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl4.SchoolCode }
       //                 where
       //                   tbl4.Id == userId
       //                 orderby
       //                   tbl1.EventYear descending
       //                 select new
       //                 {
       //                     tbl1.SchCode,
       //                     tbl1.EventId,
       //                     tbl2.EventCode,
       //                     tbl1.EventYear,
       //                     tbl2.EventName,
       //                     tbl3.EventImagePath
       //                 });
       //    //  var data =(from s in schoolEventRepository.FindBy(x => x.SchCode ==tcode) select s).Take(2);
       //     var count = data.Count();
       //     if (count == 0)
       //         return Ok(new { result = "error" });
       //     if(count>1)
       //     {
       //         flag = true;
       //     }
       //     else
       //     {
       //         flag = false;
       //     }
           
       //     return Ok(new {result=data,flag});
       // }

        [HttpGet]
       public IHttpActionResult GetStudentDetail(string ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();

            if(sCode == null)
                return Ok(new { result = "error" });

            var tcode = sCode.SchoolCode;
            var studentList = studentRegistrationRepository.GetAll().Where(x => x.SchCode == tcode && x.EventCode==ecode);
            return Ok(new { result = studentList });
        }

        [HttpGet]
        public IHttpActionResult GetSchoolStudentDetailBySchoolCodeAndEventCode(string scode,string ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            //var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();
            // var tcode = sCode.SchoolCode;
            var studentList = (from tbl1 in studentRegistrationRepository.FindBy(x=>x.SchCode==scode && x.EventCode==ecode)
                               join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                               where
                                 tbl2.Id == userId &&
                                 tbl1.EventCode == ecode
                               select new
                               {
                                   tbl1.Id,
                                   tbl1.EventCode,
                                   tbl1.SchCode,
                                   tbl1.RegSrlNo,
                                   tbl1.ExamDateOpted,
                                   tbl1.Class1E,
                                   tbl1.Class2E,
                                   tbl1.Class3E,
                                   tbl1.Class4E,
                                   tbl1.Class5E,
                                   tbl1.Class6E,
                                   tbl1.Class7E,
                                   tbl1.Class8E,
                                   tbl1.Class9E,
                                   tbl1.Class10E,
                                   tbl1.Class11EN,
                                   tbl1.Class12EN,
                                   tbl1.TotalE,
                                   tbl1.Book1,
                                   tbl1.Book2,
                                   tbl1.Book3,
                                   tbl1.Book4,
                                   tbl1.Book5,
                                   tbl1.Book6,
                                   tbl1.Book7,
                                   tbl1.Book8,
                                   tbl1.Book9,
                                   tbl1.Book10,
                                   tbl1.Book11,
                                   tbl1.Book12,
                                   tbl1.BookTot,
                                   tbl1.PreYearQP1,
                                   tbl1.PreYearQP2,
                                   tbl1.PreYearQP3,
                                   tbl1.PreYearQP4,
                                   tbl1.PreYearQP5,
                                   tbl1.PreYearQP6,
                                   tbl1.PreYearQP7,
                                   tbl1.PreYearQP8,
                                   tbl1.PreYearQP9,
                                   tbl1.PreYearQP10,
                                   tbl1.PreYearQP11,
                                   tbl1.PreYearQP12,
                                   tbl1.PreYearQPTot,
                                   tbl1.GenXBook1,
                                   tbl1.GenXBook2,
                                   tbl1.GenXBook3,
                                   tbl1.GenXBook4,
                                   tbl1.GenXBook5,
                                   tbl1.GenXBook6,
                                   tbl1.GenXBook7,
                                   tbl1.GenXBook8,
                                   tbl1.GenXBook9,
                                   tbl1.GenXBook10,
                                   tbl1.GenXBook11,
                                   tbl1.GenXBook12,
                                   tbl1.GenXBookTot
                               });
         
                return Ok(studentList);
        }

     [HttpGet]
     public IHttpActionResult  GetDipatchDetail(string scode, string  ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            //var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();
            // var tcode = sCode.SchoolCode;
            var orderList = (from tbl1 in bookDispatchRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode)
                               join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                               where
                                 tbl2.Id == userId &&
                                 tbl1.EventCode == ecode
                               select new
                               {
                                   tbl1.Id,
                                   tbl1.EventCode,
                                   tbl1.SchCode,
                                   tbl1.PacketID,
                                   tbl1.ItemDescription,
                                   tbl1.ReceivedBy,
                                   tbl1.Remarks,
                                   tbl1.Weight,
                                   tbl1.StatusDate,
                                   tbl1.DeliveryStatus,
                                   tbl1.DispatchDate,
                                   tbl1.DispatchMode,
                                   tbl1.ConsignmentNumber,
                                   tbl1.CourierName
                                   
                               });

            return Ok(orderList);
        }

        [HttpGet]
        public IHttpActionResult GetQPDipatchDetail(string scode, string ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            //var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();
            // var tcode = sCode.SchoolCode;
            var dispatchList = (from tbl1 in qpDispatchRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode)
                             join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                             where
                               tbl2.Id == userId &&
                               tbl1.EventCode == ecode
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.EventCode,
                                 tbl1.SchCode,
                                 tbl1.PacketID,
                                 tbl1.ItemDescription,
                                 tbl1.ReceivedBy,
                                 tbl1.Remarks,
                                 tbl1.Weight,
                                 tbl1.StatusDate,
                                 tbl1.DeliveryStatus,
                                 tbl1.DispatchDate,
                                 tbl1.DispatchMode,
                                 tbl1.ConsignmentNumber,
                                 tbl1.CourierName

                             });

            return Ok(dispatchList);
        }

        [HttpGet]
        public IHttpActionResult GetQueryList()
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var queryList = (from tbl1 in queryRepository.FindBy(x => x.UserId == userid)
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.QueryDate,
                                 tbl1.OldRef,
                                 tbl1.QueryDetail,
                                 tbl1.QueryStatus,
                                 tbl1.Subject,
                                 tbl1.UserId,
                                 tbl1.CloseDate
                             });

            return Ok(queryList);
        }


        #region Query
        [HttpGet]
        public IHttpActionResult GetQueryById(int id)
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var dataList = queryRepository.GetAll().Where(x => x.Id == id).ToList();

            return Ok(new { result = dataList });
        }

        [HttpPost]
        public IHttpActionResult CreateQuery(QueryViewModel model)
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            if (model != null)
            {
                queryRepository.Create(new Query()
                {
                    Subject = model.Subject,
                    QueryDetail = model.QueryDetail,
                    OldRef = model.OldRef,
                    QueryDate =DateTime.Today,
                    QueryStatus = "Open",
                    UserId = userid
                    });

                    return Ok(new { result = "Success" });
                
            }
            return Ok(new { result = "error" });
        }
        [HttpPost]  // automatically assign value if send from ajax in a single model
        public IHttpActionResult Edit(QueryViewModel model)
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            if (model != null)
            {
               
                    var query = queryRepository.GetById(model.id);
                    query.Subject = model.Subject;
                    query.QueryDetail = model.QueryDetail;
                    query.OldRef = query.OldRef;
                    query.QueryDate = model.QueryDate;
                    query.QueryStatus = model.QueryStatus;
                    query.UserId = userid;
                    // update records
                    queryRepository.Update(query);
                    return Ok(new { result = "Success" });
                
            }

            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult Delete(int Id)
        {
            if (Id != 0)
            {
                queryRepository.Delete(queryRepository.
                    GetById(Id));

                return Ok(new { result = "Query deleted sucessfully" });
            }
            return Ok(new { result = "Query is not found" });
        }

        #endregion Query


        #region Result
        [HttpGet]
        public  IHttpActionResult GetResultBySchoolCodeAndEventId(string scode,int eid)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            //var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();
            // var tcode = sCode.SchoolCode;
            var orderList = (from tbl1 in resultRepository.FindBy(x=>x.SchCode==scode && x.EventId==eid)
                             join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                             where
                               tbl2.Id == userId
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.Class,
                                 tbl1.SchCode,
                                 tbl1.RollNo,
                                 tbl1.TotMarks,
                                 tbl1.ClassRank,
                                 tbl1.StateRank,
                                 tbl1.AllIndiaRank,
                                 tbl1.NIORollNo,
                                 tbl1.StudName,
                                 tbl1.RawScore,
                                 tbl1.SecondLevelEligible,
                                 tbl1.Medal,
                                 tbl1.EventId,
                                 tbl1.EventYear,
                                 tbl1.Level
                             });
            
            return Ok(orderList);
        }

        [HttpGet]
        public IHttpActionResult GetResultBySchoolCodeEventIdAndRoll(string scode, int eid,string rollno)
        {
            var data = string.Empty; 
            var userId = Convert.ToInt32(User.Identity.Name);
            
            var orderList = (from tbl1 in resultRepository.FindBy(x => x.SchCode == scode && x.EventId == eid && x.NIORollNo == rollno)
                             join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                             where
                               tbl2.Id == userId &&
                               tbl1.NIORollNo == rollno
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.Class,
                                 tbl1.SchCode,
                                 tbl1.RollNo,
                                 tbl1.TotMarks,
                                 tbl1.ClassRank,
                                 tbl1.StateRank,
                                 tbl1.AllIndiaRank,
                                 tbl1.NIORollNo,
                                 tbl1.StudName,
                                 tbl1.RawScore,
                                 tbl1.SecondLevelEligible,
                                 tbl1.Medal,
                                 tbl1.EventId,
                                 tbl1.EventYear,
                                 tbl1.Level
                             });
           
               return Ok(orderList) ;
        }
        #endregion Result

        #region OlympiadList
        [HttpGet]
        public IHttpActionResult GetOlympiadList(bool status)
        {
            var olyList = (from s in olympiadListRepository.FindBy(x => x.Status == status)
                         select new {
                             s.OlympiadName,
                             s.FirstDate,
                             s.SecondDate,
                             s.LastDateOfRegistration
                            
                             
                         });

            return Ok(olyList);
        }
        #endregion OlympiadList

        #region TeacherProfile
        [HttpGet]
        public IHttpActionResult GetSchoolCode()
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var data = teacherDetailRepository.FindById(userid);

            if (data != null)
                return Ok(new { result = data.SchoolCode });
            else
                return Ok(new { result = data });
        }

        //[HttpGet]
        //public IHttpActionResult GetTeacherProfile()
        //{
        //    var userid =Convert.ToInt32(User.Identity.Name);
        //    var allEvents = eventRepository.GetAll();
        //    var allprofile = profileRepository.GetAll();
        //    var events = (from tbl1 in eventRepository.GetAll()
        //                  join tbl2 in teacherEventRepository.GetAll() on new { Id = tbl1.Id } equals new { Id = tbl2.EventId }
        //                  where
        //                    tbl2.UserId == userid
        //                  select new
        //                  {
        //                      tbl1.Id,
        //                      tbl1.EventCode,
                              

        //                  });
        //    var data = (from tbl1 in teacherDetailRepository.GetAll()
        //                join tbl2 in profileRepository.GetAll() on new { Id = tbl1.ProfileId } equals new { Id = tbl2.Id }
        //                where
        //                  tbl1.User.Id == userid
        //                select new
        //                {
        //                    tbl1.SchoolName,
        //                    tbl1.SchoolAddress,
        //                    tbl1.PinCode,
        //                    tbl1.City,
        //                    tbl1.State,
        //                    tbl1.Country,
        //                    tbl1.ProfileId,
        //                    tbl1.SchoolCode,
        //                    tbl1.User.UserName,
                          
        //                    tbl1.User.ProfilePic,
        //                    GenderType = (System.Int32?)tbl1.User.GenderType,
        //                    DOB = (System.DateTime?)tbl1.User.DOB,
        //                    tbl2.ProfileName,
                           
        //                    events,
        //                    allEvents,
        //                    allprofile
        //                });

        //    return Ok(data);
        //}


        [HttpGet]
        public IHttpActionResult GetUserMaster()
        {
            var allEvents = eventRepository.GetAll();
            var allprofile = profileRepository.GetAll();
            return Ok(new {allEvents, allprofile });
        }
        #endregion TeacherStatus
    }
}
