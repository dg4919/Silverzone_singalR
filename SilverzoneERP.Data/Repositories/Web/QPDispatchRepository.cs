using System.Linq;
using System.Collections.Generic;
using System;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class QPDispatchRepository:BaseRepository<QPDispatch>,IQPDispatchRepository
    {
        public QPDispatchRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public QPDispatch GetByConsignmentNumber(string cnumber)
        {
            return FindBy(x => x.ConsignmentNumber == cnumber).FirstOrDefault();
        }

        public IEnumerable<QPDispatch> GetByCourierName(string cname)
        {
            return _dbset.Where(x => x.CourierName == cname).AsEnumerable();
        }

        public IEnumerable<QPDispatch> GetbyDeliveryStatus(string dstatus)
        {
            return _dbset.Where(x => x.DeliveryStatus == dstatus).AsEnumerable();
        }

        public IEnumerable<QPDispatch> GetByDispatchDate(DateTime ddate)
        {
            return _dbset.Where(x => x.DispatchDate == ddate).AsEnumerable();
        }

        public IEnumerable<QPDispatch> GetByEventCode(string ecode)
        {
            return _dbset.Where(x => x.EventCode == ecode).AsEnumerable();
        }

        public QPDispatch GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }

        public QPDispatch GetByPacketId(string pktid)
        {
            return FindBy(x => x.PacketID == pktid).FirstOrDefault();
        }

        public IEnumerable<QPDispatch> GetBySchoolCode(string scode)
        {
            return _dbset.Where(x => x.SchCode == scode).AsEnumerable();
        }

        public IEnumerable<QPDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode)
        {
            return _dbset.Where(x => x.SchCode == scode && x.EventCode == ecode).AsEnumerable();
        }

        public IEnumerable<QPDispatch> GetByWeight(decimal wt)
        {
            return _dbset.Where(x => x.Weight == wt).AsEnumerable();
        }
    }
}
