using System.Linq;
using System.IO;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using SilverzoneERP.Entities;

namespace SilverzoneERP.Data
{
    public class ForgetPasswordRepository : BaseRepository<ForgetPassword>, IForgetPasswordRepository
    {
        public ForgetPasswordRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public bool sendEmail_forgetPassword(string TemplatePath, int verfiy_code, string emailId)
        {         
            string html = string.Empty;
            using (StreamReader reader = new StreamReader(TemplatePath))
            {
                html = reader.ReadToEnd();
            }

            html = html.Replace("(otpCode)", verfiy_code.ToString());

            ClassUtility.sendMail(emailId, "Password Recovery - Silverzone", html, emailSender.emailNoreply);
            return true;
        }

        public ForgetPassword getRecords (long userId, verificationMode type)
        {
            return FindBy(x => x.UserId == userId && x.verificationMode == type).SingleOrDefault();     // we will get either 1 or 0 record
        }

    }
}
