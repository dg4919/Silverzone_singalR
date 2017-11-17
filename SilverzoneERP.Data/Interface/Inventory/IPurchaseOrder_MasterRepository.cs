using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IPurchaseOrder_MasterRepository : IRepository<PurchaseOrder_Master>
    {
        bool isRecordExist(long PO_number);

        PurchaseOrder_Master getRecordFrom(long PO_number, long srcId);
        PurchaseOrder_Master getRecordFrom(long PO_number, IQueryable<long> src_InfoIds);

        PurchaseOrder_Master getRecordTo(long PO_number, long srcId);
        PurchaseOrder_Master getBy_Ponumber(long PO_number);

        IQueryable<PurchaseOrder_Master> getRecordTo(long src_InfoId);
        IQueryable<PurchaseOrder_Master> getRecordFrom(long src_InfoId);
        IQueryable<long> getRecordFrom(orderSourceType type);

        IQueryable<PurchaseOrder_Master> findRecordFrom(orderSourceType srcId, IQueryable<long> src_InfoIds);
        IQueryable<PurchaseOrder_Master> findBypoNo(orderSourceType srcId, long poNo);
        IQueryable<PurchaseOrder_Master> findRecordTo(orderSourceType srcId, long src_InfoId);
        IQueryable<PurchaseOrder_Master> findRecordFrom(orderSourceType srcId, long src_InfoId);

        //PurchaseOrder_Master getRecord(long PO_number, long srcInfoId);

        IQueryable<PurchaseOrder_Master> FilterBypoNo(IQueryable<PurchaseOrder_Master> model, long poNo);

        IQueryable<PurchaseOrder_Master> FilterBySource(IQueryable<PurchaseOrder_Master> model, IQueryable<long> sourceIds);
        IQueryable<PurchaseOrder_Master> FilterBySource(IQueryable<PurchaseOrder_Master> model, long sourceId);

        IQueryable<PurchaseOrder_Master> FilterByfromDate(IQueryable<PurchaseOrder_Master> model, DateTime fromDate);
        IQueryable<PurchaseOrder_Master> FilterBytoDate(IQueryable<PurchaseOrder_Master> model, DateTime toDate);
    }
}
