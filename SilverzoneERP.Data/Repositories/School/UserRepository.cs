using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class UserRepository: BaseRepository<ERPuser>, IUserRepository
    {
        public UserRepository(SilverzoneERPContext context) : base(context) { }

        public ERPuser GetById(long id)
        {
            try
            {
                return _dbContext.ERPusers.FirstOrDefault(x => x.Id == id && x.Status == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public ERPuser GetByEmail(string EmailID)
        {
            try
            {
                return _dbContext.ERPusers.FirstOrDefault(x => x.EmailID == EmailID);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ERPuser SignIn(string EmailID, string Password)
        {
            try
            {
                return _dbContext.ERPusers.FirstOrDefault(x => x.EmailID == EmailID && x.Password == Password && x.Status == true);                    
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        public dynamic Get(int StartIndex,int Limit,out long Count)
        {
            try
            {
                var data = _dbContext.ERPusers.OrderByDescending(x=>x.UpdationDate).Skip(StartIndex).Take(Limit).Select(x => new
                {
                    x.Id,
                    x.UserName,
                    icon = "<img src = " + "\"" + "http://localhost:57469/" + (x.ProfilePic == null ? "ProfilePic/placeholderImage.png" : x.ProfilePic) + "\"" + "class=" + "\"" + "img-circle" + "\"" + "style=" + "\"" + "width: 23px;height: 23px;" + "\"" + "/>",
                    x.ProfilePic,
                    x.UserAddress,
                    x.EmailID,
                    UserID = "(" + x.EmailID + ")",
                    x.MobileNumber,
                    x.Password,
                    x.GenderType,
                    x.RoleId,
                    x.DOB,
                    x.Status
                });
                Count = _dbContext.Users.Count();
                return data;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic Get()
        {
            try
            {
                var data = _dbContext.ERPusers.Where(x=>x.Status==true).OrderByDescending(x => x.UpdationDate).Select(x => new
                {
                    x.Id,
                    x.UserName,
                    icon = "<img src = " + "\"" + "http://localhost:55615/" + (x.ProfilePic == null ? "ProfilePic/placeholderImage.png" : x.ProfilePic) + "\"" + "class=" + "\"" + "img-circle" + "\"" + "style=" + "\"" + "width: 23px;height: 23px;" + "\"" + "/>",
                    x.ProfilePic,
                    x.UserAddress,
                    x.EmailID,
                    UserID = "(" + x.EmailID + ")",
                    x.MobileNumber,
                    x.Password,
                    x.GenderType,
                    x.RoleId,
                    x.Role.RoleName,
                    x.DOB,
                    x.Status
                });                
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ERPuser Get(long UserId)
        {
            return _dbContext.ERPusers.FirstOrDefault(x => x.Id == UserId);
        }
        public dynamic GetCreated_UpdatedBy(long UserId)
        {
            try
            {
                var data = _dbContext.Users.Where(x=>x.Id== UserId).Select(x => new
                {                   
                    x.UserName,                   
                    x.ProfilePic,                                                                                
                }).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string WriteImage(string Base64image)
        {
           return ClassUtility.WriteImage(Base64image);
        }

        public bool DeleteImage(string ImagePath)
        {
            return ClassUtility.DeleteImage(ImagePath);
        }

        public string GetUserName(long UserId)
        {
            return _dbset.FirstOrDefault(x => x.Id == UserId).UserName;
        }

    }
}
