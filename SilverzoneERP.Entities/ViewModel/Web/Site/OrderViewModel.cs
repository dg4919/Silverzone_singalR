using SilverzoneERP.Entities.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class OrderViewModel
    {
        public int Shipping_addressId { get; set; }
        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }

        public IEnumerable<bookViewModel> bookViewModel { get; set; }
    }

    public class bookViewModel
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal unitPrice { get; set; }
        public bookType bookType { get; set; }
    }

    public class orderPriceViewModel
    {
        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }
        public countryType shipping_country { get; set; }

        public IEnumerable<bookPriceViewModel> bookPrice { get; set; }


        public static orderPriceViewModel Parse(IEnumerable<OrderDetail> modelList)
        {
            var order = modelList.FirstOrDefault().Order;

            return new orderPriceViewModel()
            {
                Total_Shipping_Amount = order.Total_Shipping_Amount,
                Total_Shipping_Charges = order.Total_Shipping_Charges,
                shipping_country = order.UserShippingAddress.CountryType,        // get values by navigation proprties
                bookPrice = modelList.Select(x => new bookPriceViewModel()
                {
                    BookPrice = x.bookType == bookType.Book ? x.Book.Price : x.Bundle.bundle_totalPrice,
                    //BookPrice = x.Book.Price,
                    Quantity = x.Quantity,
                    book_newPrice = x.Book != null && x.Book.ItemTitle_Master.BookCategory.CouponId.HasValue && x.bookType == bookType.Book
                                  ? getBook_newPrice(x.Book.Price, x.Book.ItemTitle_Master.BookCategory.Coupons)
                                  : 0
                })
            };
        }

        private static decimal getBook_newPrice(decimal bookPrice, Coupon coupon)
        {
            var sys_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Date;

            if (coupon.Status &&
                sys_date >= coupon.Start_time &&
                sys_date <= coupon.End_time)
            {
                if (coupon.DiscountType == CouponType.FlatDiscount)
                    return bookPrice - coupon.Coupon_amount;
                else
                    return bookPrice - (bookPrice * coupon.Coupon_amount) / 100;
            }

            return 0;       // if coupon is not valid return > 0  
        }


        public static bool validate_orderAmount(orderPriceViewModel modelList)
        {
            var shipping_amount = modelList.bookPrice.Sum(x => (x.book_newPrice != 0 ? x.book_newPrice : x.BookPrice) * x.Quantity);  // sum of list of each(sum * quantity)
            var item_quantity = modelList.bookPrice.Sum(x => x.Quantity);         // return sum of quantity in the list

            var shipping_charge = modelList.shipping_country == countryType.India
                                ? 40 + (item_quantity - 1) * 25
                                : 1200 + (item_quantity - 1) * 200;

            if ((modelList.Total_Shipping_Amount + modelList.Total_Shipping_Charges).Equals
                (shipping_amount + shipping_charge))
                return true;
            else
                return false;
        }
    }

    public class bookPriceViewModel
    {
        public decimal BookPrice { get; set; }
        public int Quantity { get; set; }
        public decimal book_newPrice { get; set; }
    }

    public class bundleWheight_ViewModel
    {
        public long dispatch_mId { get; set; }
        public IEnumerable<bundleWheight_Model> wheightModel { get; set; }

        public static Packet_BundleInfo parse(long dispatchId, bundleWheight_Model model)
        {
            return new Packet_BundleInfo()
            {
                dispatch_mId = dispatchId,
                PM_Id = model.PM_Id,
                Netwheight = model.Netwheight
            };
        }

    }

    public class bundleWheight_Model
    {
        public long PM_Id { get; set; }
        public decimal Netwheight { get; set; }
    }

}