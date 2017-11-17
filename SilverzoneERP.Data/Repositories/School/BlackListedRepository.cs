using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class BlackListedRepository : BaseRepository<BlackListedSchool>, IBlackListedRepository
    {
        public BlackListedRepository(SilverzoneERPContext context) : base(context) { }

        public List<BlackListedSchool> GetBySchoolId(long Id)
        {
            try
            {
                return _dbContext.BlackListedSchools.Where(x=>x.SchId==Id).OrderByDescending(x=>x.UpdationDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }                       
        }

        public bool IsChange(long SchoolId,bool IsBlocked)
        {
            try
            {
                var data = _dbContext.BlackListedSchools
                    .OrderByDescending(x => x.UpdationDate)
                    .FirstOrDefault();

                if (data == null)
                    return true;
                else
                return data.IsBlocked == IsBlocked ? false : true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic Get_Latest_BlackListd(long SchoolId)
        {
            try
            {
                return _dbContext.BlackListedSchools.Where(x => x.SchId == SchoolId).OrderByDescending(b => b.UpdationDate)
                    .ToList()
                    .Select(x => new
                    {
                        x.IsBlocked,
                        x.BlackListedRemarks,
                        BlackListedBy = _dbContext.ERPusers.Where(u => u.Id == x.UpdatedBy).FirstOrDefault().UserName,
                        BlackListedOn = x.UpdationDate
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
