using SilverzoneERP.Context;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace SilverzoneERP.Data
{
    class PurchaseOrder_MasterRepository : BaseRepository<PurchaseOrder_Master>, IPurchaseOrder_MasterRepository
    {
        public PurchaseOrder_MasterRepository(SilverzoneERPContext context) : base(context) { }

        public bool isRecordExist(long PO_number)
        {
            return _dbset.Any(x => x.PO_Number == PO_number && x.Status == true);
        }

        public PurchaseOrder_Master getRecordFrom(long PO_number, long srcId)
        {
            return _dbset.SingleOrDefault(x => x.PO_Number == PO_number
                                            && x.srcFrom == srcId
                                            && x.Status == true);
        }

        public IQueryable<long> getRecordFrom(orderSourceType type)
        {
            return FindBy(x => x.From == type
                            && x.Status == true)
                   .GroupBy(y => y.srcFrom)      // only single time srcFrom id would be pass
                   .Select(y => y.FirstOrDefault().srcFrom);
        }

    public PurchaseOrder_Master getRecordFrom(long PO_number, IQueryable<long> src_InfoIds)
        {
            return _dbset.SingleOrDefault(x => x.PO_Number == PO_number
                           && src_InfoIds.Contains(x.srcFrom)
                           && x.Status == true);
        }

        public PurchaseOrder_Master getRecordTo(long PO_number, long srcId)
        {
            return _dbset.SingleOrDefault(x => x.PO_Number == PO_number
                                            && x.srcTo == srcId
                                            && x.Status == true);
        }

        public PurchaseOrder_Master getBy_Ponumber(long PO_number)
        {
            return _dbset.SingleOrDefault(x => x.PO_Number == PO_number
                                                                && x.Status == true);
        }

        public IQueryable<PurchaseOrder_Master> getRecordFrom(long src_InfoId)
        {
            return _dbset.Where(x => x.srcFrom == src_InfoId
                                            && x.Status == true);
        }

        public IQueryable<PurchaseOrder_Master> getRecordTo(long src_InfoId)
        {
            return FindBy(x => x.srcTo == src_InfoId
                            && x.Status == true);
        }      

        public IQueryable<PurchaseOrder_Master> findRecordFrom(orderSourceType srcId, IQueryable<long> src_InfoIds)
        {
            return FindBy(x => x.From == srcId
                           && src_InfoIds.Contains(x.srcFrom)
                           && x.Status == true);
        }
        public IQueryable<PurchaseOrder_Master> findBypoNo(orderSourceType srcId, long poNo)
        {
            return FindBy(x => x.From == srcId
                           && x.PO_Number == poNo
                           && x.Status == true);
        }

        public IQueryable<PurchaseOrder_Master> findRecordFrom(orderSourceType srcId, long src_InfoId)
        {
            return FindBy(x => x.From == srcId
                           && x.srcFrom == src_InfoId
                           && x.Status == true);
        }

        public IQueryable<PurchaseOrder_Master> findRecordTo(orderSourceType srcId, long src_InfoId)
        {
            return FindBy(x => x.To == srcId
                            && x.srcTo == src_InfoId
                            && x.Status == true);
        }

        //public PurchaseOrder_Master getRecord(long PO_number, long srcInfoId)
        //{
        //    return _dbset.SingleOrDefault(x => x.PO_Number == PO_number
        //                                    && x.SourceInfo_Id == srcInfoId
        //                                    && x.Status == true);
        //}

        public override IQueryable<PurchaseOrder_Master> GetAll()
        {
            return _dbset.Where(x => x.Status == true);
        }

        public IQueryable<PurchaseOrder_Master> FilterBypoNo(IQueryable<PurchaseOrder_Master> model, long poNo)
        {
            return model.Where(x => x.PO_Number == poNo);
        }

        public IQueryable<PurchaseOrder_Master> FilterBySource(IQueryable<PurchaseOrder_Master> model, IQueryable<long> sourceIds)
        {
            return model.Where(x => sourceIds.Contains(x.srcTo));
        }

        public IQueryable<PurchaseOrder_Master> FilterBySource(IQueryable<PurchaseOrder_Master> model, long sourceId)
        {
            return model.Where(x => x.srcTo == sourceId);
        }


        public IQueryable<PurchaseOrder_Master> FilterByfromDate(IQueryable<PurchaseOrder_Master> model, DateTime fromDate)
        {
            return model.Where(x => DbFunctions.TruncateTime(x.PO_Date) >= fromDate);
        }

        public IQueryable<PurchaseOrder_Master> FilterBytoDate(IQueryable<PurchaseOrder_Master> model, DateTime toDate)
        {
            return model.Where(x => DbFunctions.TruncateTime(x.PO_Date) <= toDate);
        }

    }
}
