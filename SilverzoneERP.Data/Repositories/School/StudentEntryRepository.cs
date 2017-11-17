using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
using System.Data.Entity;
using System;

namespace SilverzoneERP.Data
{
    public class StudentEntryRepository : BaseRepository<StudentEntry>, IStudentEntryRepository
    {
        public StudentEntryRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(long EventmanagementId, long ClassId, string Section, long RollNo, string StudentName)
        {
            return _dbset.Count(x=>x.EventmanagementId==EventmanagementId && x.ClassId== ClassId && x.Section.Trim().ToLower()==Section.Trim().ToLower() && x.RollNo==RollNo && x.StudentName.Trim().ToLower()==StudentName.Trim().ToLower())==0?false:true;
        }

        public bool Exists(long StudentEntryId,long EventmanagementId, long ClassId, string Section, long RollNo, string StudentName)
        {
            return _dbset.Count(x => x.Id!=StudentEntryId && x.EventmanagementId == EventmanagementId && x.ClassId == ClassId && x.Section.Trim().ToLower() == Section.Trim().ToLower() && x.RollNo == RollNo && x.StudentName.Trim().ToLower() == StudentName.Trim().ToLower()) == 0 ? false : true;
        }
        
        public StudentEntry Get(long StudentEntryId)
        {
            return _dbset.FirstOrDefault(x => x.Id == StudentEntryId);
        }

        public dynamic Get(long EventManagementId, Nullable<long> ClassId, string Section , int StartIndex, int Limit, out long Count)
        {
            try
            {
                Count = 0;
                if (ClassId == null && string.IsNullOrWhiteSpace(Section))
                {
                    var data = _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.OrderByDescending(s => s.UpdationDate).Skip(StartIndex).Take(Limit).Select(s => new {
                            s.Id,
                            s.EnrollmentNo,
                            s.EventmanagementId,
                            s.ClassId,
                            ClassName = s.Class.className,
                            s.Section,
                            s.RollNo,
                            s.StudentName,
                            s.RowVersion,
                            s.UpdationDate
                        })
                    }).FirstOrDefault().StudentEntry;
                    Count = _dbset.Count(x => x.EventmanagementId == EventManagementId);
                    return data;
                }

                else if (ClassId != null && !string.IsNullOrWhiteSpace(Section))
                {
                    var data = _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.Where(s=>s.ClassId==ClassId && s.Section==Section).OrderByDescending(s => s.UpdationDate).Skip(StartIndex).Take(Limit).Select(s => new {
                            s.Id,
                            s.EnrollmentNo,
                            s.EventmanagementId,
                            s.ClassId,
                            ClassName=s.Class.className,
                            s.Section,
                            s.RollNo,
                            s.StudentName,
                            s.RowVersion,
                            s.UpdationDate
                        })
                    }).FirstOrDefault().StudentEntry;
                    Count = _dbset.Count(x => x.EventmanagementId == EventManagementId && x.ClassId == ClassId && x.Section==Section);
                    return data;
                }

                else if (ClassId != null && string.IsNullOrWhiteSpace(Section))
                {
                    var data = _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.Where(s => s.ClassId == ClassId ).OrderByDescending(s => s.UpdationDate).Skip(StartIndex).Take(Limit).Select(s => new {
                            s.Id,
                            s.EnrollmentNo,
                            s.EventmanagementId,
                            s.ClassId,
                            ClassName = s.Class.className,
                            s.Section,
                            s.RollNo,
                            s.StudentName,
                            s.RowVersion,
                            s.UpdationDate
                        })
                    }).FirstOrDefault().StudentEntry;
                    Count = _dbset.Count(x => x.EventmanagementId == EventManagementId && x.ClassId == ClassId );
                    return data;
                }
                else if (ClassId == null && !string.IsNullOrWhiteSpace(Section))
                {
                    var data = _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.Where(s => s.Section == Section).OrderByDescending(s => s.UpdationDate).Skip(StartIndex).Take(Limit).Select(s => new {
                            s.Id,
                            s.EnrollmentNo,
                            s.EventmanagementId,
                            s.ClassId,
                            ClassName = s.Class.className,
                            s.Section,
                            s.RollNo,
                            s.StudentName,
                            s.RowVersion,
                            s.UpdationDate
                        })
                    }).FirstOrDefault().StudentEntry;
                    Count = _dbset.Count(x => x.EventmanagementId == EventManagementId  && x.Section == Section);
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public dynamic Get(long EventManagementId, int GroupNo)
        {
            try
            {
                if(GroupNo==1)
                {
                    return _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.OrderByDescending(s => s.UpdationDate)
                        .GroupBy(g => g.Class.className)
                        .Select(s => new {
                            ClassName = s.Key,
                            Sections = s.GroupBy(g=>g.Section).Select(l => new {
                                Section=l.Key,
                                Count=l.Count()
                            })

                        })
                    }).FirstOrDefault().StudentEntry;
                }

                if (GroupNo == 2)
                {
                    return _dbContext.EventManagements.Where(x => x.Id == EventManagementId)
                    .Select(x => new {
                        StudentEntry = x.StudentEntry.OrderByDescending(s => s.UpdationDate)
                        .GroupBy(g => g.Section)
                        .Select(s => new {
                            Section = s.Key,
                            Students = s.Select(l => new {
                                l.Id,
                                l.EventmanagementId,
                                l.ClassId,
                                ClassName=l.Class.className,
                                l.Section,
                                l.RollNo,
                                l.StudentName,
                                l.RowVersion,
                                l.UpdationDate
                            })

                        })
                    }).FirstOrDefault().StudentEntry;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateOTP(int length)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;
           
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
    }
}
