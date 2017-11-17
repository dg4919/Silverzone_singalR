using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Drawing;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IDispatch_otherItem_MasterRepository : IRepository<Dispatch_otherItem_Master>
    { }

    public interface IDispatch_otherItem_addressRepository : IRepository<Dispatch_otherItem_address>
    { }

    public interface IDispatch_MasterRepository : IRepository<Dispatch_Master>
    {
        Dispatch_Master GetBychallanId(long challanId);
        Dispatch_Master GetByorderId(long orderid);
        IQueryable<Dispatch_Master> FilterbyTxt(string srchTxt);
    }

    public interface IDispatchRepository : IRepository<Dispatch>
    {
        string get_Packet_Number(long identityValue, orderSourceType srcType);
        string get_Invoice_Number(long identityValue);
        bool is_orderExist(int orderId);
        Dispatch GetByorderId(long orderid);
        Dispatch GetBy_otherItemId(long itemId);
        Dispatch GetBychallanId(long challanId);
        Dispatch GetBypacketId(string packetNumber);
        bool GetByConsignment(string consignmentNo, int dispatchId);
        string getbase64(Image image);
        IQueryable<Dispatch> FilterByDate(DateTime fromDate, DateTime toDate);
        IQueryable<Dispatch> FilterByInvoice(IQueryable<Dispatch> result, string invoiceNo);
        IQueryable<Dispatch> FilterByInvoice(IQueryable<Dispatch> result, IQueryable<string> invoiceNos);
    }

    public interface IDispatchLogsRepository : IRepository<DispatchLogs>
    { }
}
