using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IForgetPasswordRepository : IRepository<ForgetPassword>
    {
        bool sendEmail_forgetPassword(string TemplatePath, int verfiy_code, string emailId);
        ForgetPassword getRecords(long userId, verificationMode type);

    }
}
