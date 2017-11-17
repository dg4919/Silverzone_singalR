using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IOrderDetailRepository:IRepository<OrderDetail>
    {
        OrderDetail GetById(int id);
        IEnumerable<OrderDetail> GetByOrderId(int id);
    }
}
