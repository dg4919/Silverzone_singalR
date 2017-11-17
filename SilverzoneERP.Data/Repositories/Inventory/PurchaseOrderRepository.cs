using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    class PurchaseOrderRepository : BaseRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(SilverzoneERPContext context) : base(context) { }

        public override IQueryable<PurchaseOrder> GetAll()
        {
            return _dbset.Where(x => x.Status == true);
        }

        public bool isRecordExist(long BookId, long PO_Id)
        {
            return _dbset.Any(x => x.BookId == BookId
                                && x.PO_mId == PO_Id
                                && x.Status == true);
        }

        public bool isRecordExist(long Id, long BookId, long PO_Id)
        {
            return _dbset.Any(x => x.BookId == BookId
                                && x.PO_mId == PO_Id
                                && x.Id != Id
                                && x.Status == true);
        }

        public PurchaseOrder getRecord(long Id, long BookId, long PO_Id)
        {
            return _dbset.SingleOrDefault(x => x.BookId == BookId
                                && x.PO_mId == PO_Id
                                && x.Id != Id
                                && x.Status == true);
        }

        public PurchaseOrder getRecord(long BookId, long PO_Id)
        {
            return _dbset.SingleOrDefault(x => x.BookId == BookId
                                && x.PO_mId == PO_Id
                                && x.Status == true);
        }

        public bool sendEmail_PO_Confirmation(string emailTemplate, string emailId)
        {
            ClassUtility.sendMail(emailId, "Purchase Order List - Silverzone", emailTemplate, emailSender.emailInfo);
            return true;
        }

        public IQueryable<PurchaseOrder> get_byBookId(IQueryable<long> booksId)
        {
            return _dbset.Where(x => booksId.Contains(x.BookId)
                                  && x.Status == true);
        }


        public IQueryable<PurchaseOrder> get_pendingPO(IQueryable<long> Ids, IQueryable<long> booksId, int? poType)
        {
            return _dbset.Where(x => (Ids.Contains(x.PO_mId)
                                      && booksId.Contains(x.BookId)
                                      && poType == 3
                                       ? x.is_Adjusted      // for true value
                                       : x.is_Adjusted == x.is_Adjusted)
                                      && x.Status == true
                                      );
        }

    }
}
