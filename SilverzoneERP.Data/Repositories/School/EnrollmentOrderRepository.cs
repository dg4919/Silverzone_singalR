using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class EnrollmentOrderRepository : BaseRepository<EnrollmentOrder>, IEnrollmentOrderRepository
    {
        public EnrollmentOrderRepository(SilverzoneERPContext context) : base(context) { }
        
    }
}
