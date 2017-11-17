using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class BookDispatchRepository:BaseRepository<BookDispatch>,IBookDispatchRepository
    {
        public BookDispatchRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookDispatch GetByConsignmentNumber(string cnumber)
        {
            return FindBy(x => x.ConsignmentNumber == cnumber).FirstOrDefault();
        }

        public IEnumerable<BookDispatch> GetByCourierName(string cname)
        {
            return _dbset.Where(x => x.CourierName == cname).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetbyDeliveryStatus(string dstatus)
        {
            return _dbset.Where(x => x.DeliveryStatus == dstatus).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByDispatchDate(DateTime ddate)
        {
            return _dbset.Where(x => x.DispatchDate == ddate).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByEventCode(string ecode)
        {
            return _dbset.Where(x => x.EventCode == ecode).AsEnumerable();
        }

        public BookDispatch GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }

        public BookDispatch GetByPacketId(string pktid)
        {
            return FindBy(x => x.PacketID == pktid).FirstOrDefault();
        }

        public IEnumerable<BookDispatch> GetBySchoolCode(string scode)
        {
            return _dbset.Where(x => x.SchCode == scode).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode)
        {
            return _dbset.Where(x => x.SchCode == scode && x.EventCode == ecode).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByWeight(decimal wt)
        {
            return _dbset.Where(x => x.Weight == wt).AsEnumerable();
        }
    }
}
