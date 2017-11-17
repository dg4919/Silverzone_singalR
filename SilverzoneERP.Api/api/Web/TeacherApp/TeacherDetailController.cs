using SilverzoneERP.Entities.ViewModel.TeacherApp;
using SilverzoneERP.Data;
using System;
using System.Web.Http;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.TeacherApp
{
    [Authorize]
    public class TeacherDetailController : ApiController
    {
        private ITeacherDetailRepository teacherDetailRepository { get; set; }
        private ITeacherEventRepository teacherEventRepository { get; set; }
        private IProfileRepository profileRepository { get; set; }
        private IEventRepository eventRepository { get; set; }
        private IAccountRepository accountRepository;

        [HttpPost]
        public IHttpActionResult create_Event(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (eventRepository.is_eventExist(model.EventName, model.EventCode))
                    return Ok(new { result = "exist" });

                eventRepository.Create(new Event()
                {
                    EventName = model.EventName,
                    EventCode = model.EventCode,
                    Status = true
                });
                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult create_Profile(string profileName)
        {
            if (profileRepository.isExist(profileName))
                return Ok(new { result = "exist" });

            profileRepository.Create(new Profile()
            {
                ProfileName = profileName,
                Status = true
            });
            return Ok(new { result = "success" });
        }

        [HttpGet]
        public IHttpActionResult get_Profiles()
        {
            var result = profileRepository.GetByStatus(true);
            return Ok(new { result = result });
        }

        [HttpGet]
        public IHttpActionResult get_Events()
        {
            var result = eventRepository.GetByStatus(true);
            return Ok(new { result = result });
        }

        [HttpPost]
        public IHttpActionResult save_userDetail(UserDetailViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(User.Identity.Name);

                var userInfo = accountRepository.FindById(userId);

                userInfo.GenderType = userModel.gender;
                userInfo.DOB = userModel.Age;
                userInfo.UserName = userModel.UserName;

                accountRepository.Update(userInfo);

                teacherDetailRepository.Create(new TeacherDetail()
                {
                    Id = userId,
                    City = userModel.City,
                    Country = userModel.Country,
                    ProfileId = userModel.ProfileId,
                    SchoolAddress = userModel.SchoolAddress,
                    SchoolName = userModel.SchoolName,
                    State = userModel.State,
                    PinCode = userModel.PinCode,
                    Status = false
                }, false);

                foreach (var eventId in userModel.Events)
                {
                    teacherEventRepository.Create(new TeacherEvent()
                    {
                        EventId = eventId,
                        UserId = userId
                    }, false);      // bulk insert only .. not saved data Yet !
                }

                teacherDetailRepository.Save(); // finally save changes :)

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult update_userDetail(UserDetailViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(User.Identity.Name);

                var userInfo = accountRepository.FindById(userId);

                userInfo.GenderType = userModel.gender;
                userInfo.DOB = userModel.Age;
                userInfo.UserName = userModel.UserName;

                accountRepository.Update(userInfo);

                var entity = teacherDetailRepository.FindById(userId);
                entity.City = userModel.City;
                entity.Country = userModel.Country;
                entity.ProfileId = userModel.ProfileId;
                entity.SchoolAddress = userModel.SchoolAddress;
                entity.SchoolName = userModel.SchoolName;
                entity.State = userModel.State;
                entity.PinCode = userModel.PinCode;

                teacherDetailRepository.Update(entity);

                teacherEventRepository.DeleteWhere(
                teacherEventRepository.FindBy(x => x.UserId == userId));

                foreach (var eventId in userModel.Events)
                {
                    teacherEventRepository.Create(new TeacherEvent()
                    {
                        EventId = eventId,
                        UserId = userId
                    }, false);      // bulk insert only .. not saved data Yet !
                }

                teacherDetailRepository.Save(); // finally save changes :)

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }


        public TeacherDetailController(
            ITeacherDetailRepository _teacherDetailRepository,
            ITeacherEventRepository _teacherEventRepository,
            IProfileRepository _profileRepository,
            IEventRepository _eventRepository,
            IAccountRepository _accountRepository)
        {
            teacherDetailRepository = _teacherDetailRepository;
            teacherEventRepository = _teacherEventRepository;
            profileRepository = _profileRepository;
            eventRepository = _eventRepository;
            accountRepository = _accountRepository;
        }

    }
}
