using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class GenericOTPRepository : BaseRepository<GenericOTP>, IGenericOTPRepository
    {
        public GenericOTPRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public GenericOTP GetByMobile(string mobileNo)
        {
            return FindBy(x => x.mobileNo == mobileNo).FirstOrDefault();
        }

   
    }
}
