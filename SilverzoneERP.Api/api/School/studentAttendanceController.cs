using Newtonsoft.Json.Linq;
using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]

    public class studentAttendanceController : ApiController
    {
        IStudentEntryRepository _studentEntryRepository;
        IStudentAttendanceRepository _studentAttendanceRepository;
        IUserRepository _userRepository;
        public studentAttendanceController(IStudentEntryRepository _studentEntryRepository, IStudentAttendanceRepository _studentAttendanceRepository, IUserRepository _userRepository)
        {
            this._studentEntryRepository = _studentEntryRepository;
            this._studentAttendanceRepository = _studentAttendanceRepository;
            this._userRepository = _userRepository;
        }

        [HttpPost]
        public IHttpActionResult Create_Update(StudentAttendance model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _studentAttendanceRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";

                        var _studentAttendance = _studentAttendanceRepository.FindBy(x=>x.Id==model.Id).FirstOrDefault();
                        if(_studentAttendance==null)
                        {
                            model.Status = true;
                            _studentAttendanceRepository.Create(model);

                            msg = "Successfully answer created !";
                        }
                        else
                        {
                            _studentAttendance.AnswerJSON = model.AnswerJSON;
                            _studentAttendanceRepository.Update(_studentAttendance);
                            msg = "Successfully answer updated !";
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });
        }

        [HttpGet]
        public IHttpActionResult Get(long Id)
        {
            try
            {
                var data = _studentAttendanceRepository.FindById(Id);
                JObject obj = new JObject();
                JArray arrry = new JArray();
                string Createdby = "";
                string CreatedDate = "";
                if (data != null)
                {
                    arrry = JArray.Parse(data.AnswerJSON);
                    Createdby = _userRepository.GetUserName(data.UpdatedBy);
                    CreatedDate = data.UpdationDate.ToString();
                }

                obj.Add("AnswerJSON", arrry);
                obj.Add("Createdby", Createdby);
                obj.Add("CreatedDate", CreatedDate);

                return Ok(new { result = obj });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     
    }
}
