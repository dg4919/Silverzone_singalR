using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IStock_MasterRepository : IRepository<Stock_Master>
    {
        IQueryable<Stock_Master> getRecord(inventoryType type);
        Stock_Master getBy_challanNo(string challanNo);
    }

    public interface IStockRepository : IRepository<Stock>
    {
        IQueryable<Stock> getRecord(IQueryable<long> stockIds);
        bool isRecordExist(long PO_Id, long stockId, long bookId);
        bool isRecordExist(long Id, long PO_Id, long stockId, long bookId);
        Stock getRecord(long PO_Id, long stockId, long bookId);
        Stock getRecord(long Id, long PO_Id, long stockId, long bookId);
        Stock getRecordN(long Id, long stockId, long bookId);
        Stock getRecord(long stockId, long bookId);
        IQueryable<Stock> get_pendingPO(IQueryable<long> Ids, IQueryable<long> booksId);
        bool sendEmail_Stock_Confirmation(string emailTemplate, string emailId);
        IQueryable<Stock> filerBy_stockMid(IQueryable<long> stock_mId);
        IQueryable<Stock> filerBy_bookId(long bookId, inventoryType type);
    }

    public interface ICounterCustomerRepository : IRepository<CounterCustomer>
    {
        
    }
}
