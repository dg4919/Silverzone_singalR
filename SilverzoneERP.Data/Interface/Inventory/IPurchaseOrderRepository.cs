using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        bool isRecordExist(long BookId, long PO_Id);
        bool isRecordExist(long Id, long BookId, long PO_Id);
        bool sendEmail_PO_Confirmation(string emailTemplate, string emailId);
        PurchaseOrder getRecord(long BookId, long PO_Id);
        PurchaseOrder getRecord(long Id, long BookId, long PO_Id);
        IQueryable<PurchaseOrder> get_pendingPO(IQueryable<long> Ids, IQueryable<long> booksId, int? poType);
        IQueryable<PurchaseOrder> get_byBookId(IQueryable<long> booksId);

    }
}
