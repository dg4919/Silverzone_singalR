using SilverzoneERP.Context;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        // ICollection have Count property > but IEnumerable has not
        public Coupon GetByName(string Name)
        {
            // trim use to remove extra space + match with lower case
            return FindBy(x => x.Coupon_name.ToLower().Trim() == Name.ToLower().Trim()).SingleOrDefault();    // use as like by in SQL
        }

        public bool Iscoupon_Exist(long Id, string Name, CouponType type)
        {
            return _dbset.Any(
                x => x.Coupon_name.ToLower().Trim() == Name.ToLower().Trim()
                && x.DiscountType == type
                && x.Id != Id);
        }

        public bool check_Coupon(string Name, CouponType type)
        {
            return _dbset.Any(
                            x => x.Coupon_name.ToLower().Trim() == Name.ToLower().Trim()
                            && x.DiscountType == type);
        }



    }
}
