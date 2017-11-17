using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IFeePaymentRepository : IRepository<FeePayment>
    {
        dynamic MiniStatement(long SchoolId, PaymentAgainst _paymentAgainst);        
        dynamic History(long SchoolId);        
    }
}
