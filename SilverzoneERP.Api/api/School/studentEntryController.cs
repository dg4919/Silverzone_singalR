using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]

    public class studentEntryController : ApiController
    {
        IStudentEntryRepository _studentEntryRepository;
        IEventManagementRepository _eventManagementRepository;
        public studentEntryController(IStudentEntryRepository _studentEntryRepository, IEventManagementRepository _eventManagementRepository)
        {
            this._studentEntryRepository = _studentEntryRepository;
            this._eventManagementRepository = _eventManagementRepository;
        }

        [HttpPost]
        public IHttpActionResult Create_Update(StudentEntry model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _studentEntryRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        model.Section = model.Section.ToUpper();
                        if (model.Id == 0)
                        {
                            
                            if (_studentEntryRepository.Exists(model.EventmanagementId,model.ClassId,model.Section,model.RollNo,model.StudentName))
                                return Ok(new { result = "Success", message = "Already exists!" });
                            model.Status = true;

                            
                            model.AuthenticationCode = _studentEntryRepository.GenerateOTP(5);
                            model.Id=_studentEntryRepository.Create(model).Id;

                            var _eventManagement = _eventManagementRepository.Get(model.EventmanagementId);
                            model.EnrollmentNo = "SZ/"+ _eventManagement.School.SchCode+"/"+DateTime.Now.Year+"/"+model.Id;

                            _studentEntryRepository.Update(model);

                            msg = "Successfully created!";
                        }
                        else
                        {
                            var _studentEntry = _studentEntryRepository.Get(model.Id);
                            if (_studentEntry != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_studentEntry.RowVersion, 0))
                            {
                                if (_studentEntryRepository.Exists(model.Id, model.EventmanagementId, model.ClassId, model.Section, model.RollNo, model.StudentName))
                                    return Ok(new { result = "Success", message = "Already exists!" });
                                _studentEntry.ClassId = model.ClassId;
                                _studentEntry.Section = model.Section;
                                _studentEntry.RollNo = model.RollNo;
                                _studentEntry.StudentName = model.StudentName;
                               
                                _studentEntryRepository.Update(_studentEntry);

                                msg = "Successfully updated!";
                            }
                            else
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !" });
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
        public IHttpActionResult GetStudent(long EventManagementId,Nullable<int> ClassId,string Section,int StartIndex, int Limit)
        {
            try
            {
                long Count;
                return Ok(new { result = _studentEntryRepository.Get(EventManagementId, ClassId, Section, StartIndex, Limit, out Count), Count = Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IHttpActionResult GetStudentGroup(long EventManagementId,int GroupNo)
        {
            try
            {                
                return Ok(new { result = _studentEntryRepository.Get(EventManagementId, GroupNo)});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
