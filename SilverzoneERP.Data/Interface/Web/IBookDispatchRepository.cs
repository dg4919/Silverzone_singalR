using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IBookDispatchRepository:IRepository<BookDispatch>
    {
        BookDispatch GetById(int id);
        BookDispatch GetByPacketId(string pktid);
        IEnumerable<BookDispatch> GetByDispatchDate(DateTime ddate);
        IEnumerable<BookDispatch> GetBySchoolCode(string scode);
        IEnumerable<BookDispatch> GetByEventCode(string ecode);
        IEnumerable<BookDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode);
        BookDispatch GetByConsignmentNumber(string cnumber);
        IEnumerable<BookDispatch> GetByWeight(decimal wt);
        IEnumerable<BookDispatch> GetbyDeliveryStatus(string dstatus);
        IEnumerable<BookDispatch> GetByCourierName(string cname);

    }
}
