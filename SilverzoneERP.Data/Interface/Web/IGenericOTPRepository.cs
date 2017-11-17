using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IGenericOTPRepository : IRepository<GenericOTP>
    {
        GenericOTP GetByMobile(string mobileNo);
    }
}
