using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IQPDispatchRepository:IRepository<QPDispatch>
    {
        QPDispatch GetById(int id);
        QPDispatch GetByPacketId(string pktid);
        IEnumerable<QPDispatch> GetByDispatchDate(DateTime ddate);
        IEnumerable<QPDispatch> GetBySchoolCode(string scode);
        IEnumerable<QPDispatch> GetByEventCode(string ecode);
        IEnumerable<QPDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode);
        QPDispatch GetByConsignmentNumber(string cnumber);
        IEnumerable<QPDispatch> GetByWeight(decimal wt);
        IEnumerable<QPDispatch> GetbyDeliveryStatus(string dstatus);
        IEnumerable<QPDispatch> GetByCourierName(string cname);
    }
}
