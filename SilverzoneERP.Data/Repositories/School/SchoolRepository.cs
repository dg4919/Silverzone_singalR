using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using SilverzoneERP.Entities;

namespace SilverzoneERP.Data
{
    public class SchoolRepository:BaseRepository<School>,ISchoolRepository
    {
        public SchoolRepository(SilverzoneERPContext context) : base(context) { }

        public string LastGenCode()
        {
            return _dbset.OrderByDescending(x => x.Id).FirstOrDefault().SchCode.ToString();
        }

        public School findBySchCode(long schCode)
        {
            return _dbset.SingleOrDefault(x => x.SchCode == schCode);
        }

        public dynamic GetHistory(long SchoolId, int StartIndex, int Limit)
        {
            var data = _dbContext.SchoolLogs.Where(x=>x.SchId==SchoolId).OrderByDescending(x=>x.Id).Skip(StartIndex).Take(Limit)
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x=>new {
                    x.CreationDate,
                    x.ERPuser.UserName,
                    x.ERPuser.EmailID
                });
            var _count = _dbContext.SchoolLogs.Count(x => x.SchId == SchoolId);
            return new { Log= data , Count = _count };
        }

        public dynamic Search(string Anything, string SchoolCode, string SchoolName, Nullable<long> CountryId, Nullable<long> ZoneId, Nullable<long> StateId, Nullable<long> DistrictId, Nullable<long> CityId, Nullable<long> PinCode, Nullable<bool> BlackListed, Nullable<int> Event_Type, Nullable<long> EventId, int StartIndex, int Limit,ref int count)
        {
            var data = _dbset.Where(x => x.Status == true);
            if (!string.IsNullOrWhiteSpace(Anything))
            {
                data = _dbset.Where(x =>
                                         x.SchCode.ToString().Contains(Anything)
                                         || x.SchName.Contains(Anything)
                                         || x.SchAddress.Contains(Anything)
                                         || x.SchAltAddress.Contains(Anything)
                                         || x.SchEmail.Contains(Anything)
                                         || x.SchWebSite.Contains(Anything)
                                         || x.SchPhoneNo.ToString().Contains(Anything)
                                         || x.SchFaxNo.ToString().Contains(Anything)
                                         || x.SchPinCode.ToString().Contains(Anything)
                                         || x.SchBoard.Contains(Anything)
                                         || x.SchAffiliationNo.Contains(Anything)
                                    );                   
            }
            else
            {
                if (Event_Type != null)
                {
                    if (Event_Type == (int)EventType.CurrentEvent)
                        data = data.Where(x => x.EventManagement.Where(ec => ec.Event.Id == EventId).Count() != 0);

                    else if (Event_Type == (int)EventType.This_Year_All_Event)
                        data = data.Where(x => x.EventManagement.Where(ec => ec.EventManagementYear == DateTime.Now.Year).Count() != 0);

                    else if (Event_Type == (int)EventType.Last_Year_Current_Event)
                        data = data.Where(x => x.EventManagement
                        .Where(ec =>
                                    ec.Event.Id ==  EventId &&
                                    ec.EventManagementYear < DateTime.Now.Year
                              ).Count() != 0);

                    else if (Event_Type == (int)EventType.Last_Year_All_Event)
                    {
                        data = data.Where(x => x.EventManagement
                        .Where(ec =>
                                    ec.EventManagementYear < DateTime.Now.Year
                              ).Count() != 0);
                    }                        
                    else if (Event_Type == (int)EventType.Not_Participated_Till_Date)
                    {
                        data = data.Where(x => x.EventManagement.Count()==0);
                    }
                    else if (Event_Type == (int)EventType.Participated_Till_Date)
                    {
                        data = data.Where(x => x.EventManagement.Count() != 0);
                    }
                }
                if(BlackListed!=null)
                {
                    data = data.Where(x => x.BlackListed.OrderByDescending(b=>b.UpdationDate).FirstOrDefault().IsBlocked== BlackListed);
                }
                
                if (!string.IsNullOrWhiteSpace(SchoolCode))
                {
                    List<long> _schoolCode = SchoolCode.Split(',').Select(s => long.Parse(s)).ToList();
                    if(_schoolCode.Count!=0)
                        data = data.Where(x => _schoolCode.Contains(x.SchCode));                        
                } 
                if(!string.IsNullOrWhiteSpace(SchoolName))
                {
                    data = data.Where(x => x.SchName.Contains(SchoolName));
                }
                if (CountryId != null)                
                    data = data.Where(x => x.CountryId == CountryId);
                if (ZoneId != null)
                    data = data.Where(x => x.ZoneId == ZoneId);
                if (StateId != null)
                    data = data.Where(x => x.StateId == StateId);
                if (DistrictId != null)
                    data = data.Where(x => x.DistrictId == DistrictId);
                if (CityId != null)
                    data = data.Where(x => x.CityId == CityId);
                if (PinCode != null)
                    data = data.Where(x => x.SchPinCode == PinCode);                
            }
            count = data.Count();
            return data.ToList().Skip(StartIndex).Take(Limit).Select(x => new {
                SchId = x.Id,
                x.SchCode,
                x.SchName,
                x.SchAddress,
                x.Country.CountryName,
                x.Zone.ZoneName,
                x.State.StateName,
                x.District.DistrictName,
                x.City.CityName,
                x.SchPinCode,
                x.SchEmail,
                x.SchPhoneNo
            });            
        }

        public dynamic GetCourier()
        {
            return _dbContext.Couriers.Where(x => x.Status == true)
                .Select(x=>new { CourierId=x.Id, x.Courier_Name,x.Courier_Link});
        }
        public dynamic GetExamDate()
        {
            return _dbContext.ExaminationDates.Where(x=>x.Status==true)
                .ToList()
                .Select(e=> new
                 {
                     ExaminationDateId = e.Id,
                    ExamDate=e.ExamDate.ToString("dd-MMM-yyyy"),
                     ExamDateFormated = ClassUtility.ordinal(e.ExamDate.Day) + " " + (e.ExamDate.ToString("MMMM")) + ", " + e.ExamDate.Year,
                     e.Status,
                     e.RowVersion
                 });
        }

        public dynamic GetSchool(long EventManagementId)
        {
            return _dbContext.EventManagements.Where(x => x.Id == EventManagementId).Select(x=>new {
                School =new {
                    EventManagementId=x.Id,
                    x.EventId,
                    x.Event.EventCode,
                    x.School.SchCode,
                    x.School.SchName,
                    x.School.SchAddress,
                    x.School.Country.CountryName,
                    x.School.Zone.ZoneName,
                    x.School.State.StateName,
                    x.School.District.DistrictName,
                    x.School.City.CityName,
                    x.School.SchPinCode,
                    x.EnrollmentOrderSummary
                }
                
            }).FirstOrDefault().School;
        }
    }
}
