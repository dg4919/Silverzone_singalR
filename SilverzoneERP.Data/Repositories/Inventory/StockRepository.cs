using SilverzoneERP.Context;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    class Stock_MasterRepository : BaseRepository<Stock_Master>, IStock_MasterRepository
    {
        public Stock_MasterRepository(SilverzoneERPContext context) : base(context) { }

        public override Stock_Master FindById(long id)
        {
            return _dbset.SingleOrDefault(x => x.Id == id
                                            && x.Status == true);
        }

        public IQueryable<Stock_Master> getRecord(inventoryType type)
        {
            return FindBy(x => x.InventoryType == type
                            && x.Status == true);
        }

        public Stock_Master getBy_challanNo(string challanNo)
        {
            return _dbset.SingleOrDefault(x => x.ChallanNo.Equals(challanNo)
                                            && x.Status == true);
        }

    }

    class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(SilverzoneERPContext context) : base(context) { }

        public override IQueryable<Stock> GetAll()
        {
            return _dbset.Where(x => x.Status == true);
        }

        public IQueryable<Stock> getRecord(IQueryable<long> stockIds)
        {
            return FindBy(x => stockIds.Contains(x.Stock_mId)
                                              && x.Status == true);
        }

        public bool isRecordExist(long PO_Id, long stockId, long bookId)
        {
            return _dbset.Any(x => x.PO_Id == PO_Id
                                && x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Status == true);
        }
       
        public bool isRecordExist(long Id, long PO_Id, long stockId, long bookId)
        {
            return _dbset.Any(x => x.PO_Id == PO_Id
                                && x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Id != Id
                                && x.Status == true);
        }

        public Stock getRecord(long PO_Id, long stockId, long bookId)
        {
            return _dbset.SingleOrDefault(x => x.PO_Id == PO_Id
                                && x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Status == true);
        }

        public Stock getRecord(long stockId, long bookId)
        {
            return _dbset.SingleOrDefault(x => x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Status == true);
        }

        public Stock getRecord(long Id, long PO_Id, long stockId, long bookId)
        {
            return _dbset.SingleOrDefault(x => x.PO_Id == PO_Id
                                && x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Id != Id
                                && x.Status == true);
        }

        public Stock getRecordN(long Id, long stockId, long bookId)
        {
            return _dbset.SingleOrDefault(x => x.Stock_mId == stockId
                                && x.BookId == bookId
                                && x.Id != Id
                                && x.Status == true);
        }

        public IQueryable<Stock> get_pendingPO(IQueryable<long> Ids, IQueryable<long> booksId)
        {
            // 1st filtered record then next filter
            return _dbset.Where(x => Ids.Contains((long)x.PO_Id)        // to parse into long > convert.toInt64 will throw error 
                                              && booksId.Contains(x.BookId)
                                              && x.Status == true);
        }

        public bool sendEmail_Stock_Confirmation(string emailTemplate, string emailId)
        {
            ClassUtility.sendMail(emailId, "Challan List - Silverzone", emailTemplate, emailSender.emailInfo);
            return true;
        }

        public IQueryable<Stock> filerBy_stockMid(IQueryable<long> stock_mId)
        {
            return GetAll().Where(x => stock_mId.Contains(x.Stock_mId));
        }

        public IQueryable<Stock> filerBy_bookId(long bookId, inventoryType type)
        {
            return GetAll()
                   .Where(x => x.BookId == bookId
                            && x.Stock_Master.InventoryType == type);
        }

    }

    class CounterCustomerRepository : BaseRepository<CounterCustomer>, ICounterCustomerRepository
    {
        public CounterCustomerRepository(SilverzoneERPContext context) : base(context) { }
    }

    }
