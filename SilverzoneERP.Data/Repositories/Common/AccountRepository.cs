using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class AccountRepository : BaseRepository<User>, IAccountRepository
    {
        public AccountRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public bool sms_verification(string mobileNo, int verfiy_code, verficationType Type)
        {
            string msg = string.Empty;
            string _smsCode = verfiy_code.ToString();

            switch (Type)
            {
                case verficationType.register:
                    msg = string.Format(smsTemplates.new_registration, _smsCode);
                    break;
                case verficationType.forget:
                    msg = string.Format(smsTemplates.foget_password, _smsCode);
                    break;
                case verficationType.change:
                    msg = string.Format(smsTemplates.change_mobile, _smsCode);
                    break;
            }

            return ClassUtility.send_message(mobileNo, msg);
        }

        public int get_smsCode()
        {
            return ClassUtility.get_smsCode();
        }

        public User GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public User user_login(User model)
        {
            return FindBy(x => x.MobileNumber == model.MobileNumber && x.Password == model.Password).FirstOrDefault();
        }

        //public User GetByEmail(string Email)
        //{
        //    return FindBy(x => x.MobileNumber == model.MobileNumber && x.Password == model.Password).FirstOrDefault();
        //}

        public User check_User(string userName, verificationMode inputType)
        {
            var user = new User();

            if (inputType == verificationMode.email)
                user = findByEmailId(userName).FirstOrDefault();

            else if (inputType == verificationMode.mobile)
                user = findByMobile(userName).FirstOrDefault();

            return user;
        }

        public bool check_User(string email, int roleId)
        {
            return _dbset.Any(x => x.EmailID.Equals(email) && x.RoleId == roleId);
        }

        public IEnumerable<User> findByMobile(string mobileNo)
        {
            return FindBy(x => x.MobileNumber == mobileNo);
        }

        public IEnumerable<User> findByEmailId(string emailId)
        {
            return FindBy(x => x.EmailID == emailId);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public List<string> upload_profile_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_Images_toTemp(tempPath);
        }

        public void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath)
        {
            // image name contain full relative path like > '/Image/User/Profile/abc.jpg', So we just want image name
            var images = imageName.Select(x => x.Substring(x.LastIndexOf('/') + 1));
            ClassUtility.save_Images_toPhysical(images, tempPath, finalPath, true, "profilePic.jpg");
        }

    }
}
