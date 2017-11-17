using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Coupon GetByName(string Name);
        bool Iscoupon_Exist(long Id, string Name, CouponType type);
        bool check_Coupon(string Name, CouponType type);

    }
}
