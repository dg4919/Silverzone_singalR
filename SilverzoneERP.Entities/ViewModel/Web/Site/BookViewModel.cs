using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using System;
using SilverzoneERP.Entities.Constant;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class BookViewModel
    {
        #region properties
        public long BookId { get; set; }
        public string BookImage { get; set; }
        public string book_title { get; set; }
        public string Publisher { get; set; }
        public string Edition { get; set; }
        //public int Pages { get; set; }
        //public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public bool In_stock { get; set; }

        public string Class { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string BookDescription { get; set; }

        public IEnumerable<Book_contentViewModel> contents { get; set; }

        public CouponViewModel CouponInfo { get; set; }

        #endregion

        public static BookViewModel Parse(Book book)
        {
            var res = new BookViewModel()
            {
                BookId = book.Id,
                BookImage = image_urlResolver.getBookImage(book.BookImage),
                book_title = book.Title,
                Publisher = book.Publisher,
                Edition = book.Edition,
                Category = book.ItemTitle_Master.BookCategory.Name,            // using navigation property to bind value
                BookDescription = book.BookDetails.BookDescription,      // using navigation property to bind value
                Class = book.ItemTitle_Master.Class.className,
                Subject = book.ItemTitle_Master.Subject.SubjectName,
                //Pages=book.Pages,
                Price = book.Price,
                //Weight=book.Weight,
                In_stock = book.in_Stock,
                contents = book.BookContents.Select(x => Book_contentViewModel.Parse(x)),       // want a list of properties so create a view with required prop > also using navigation prop
                CouponInfo = book.ItemTitle_Master.BookCategory.CouponId.HasValue ? CouponViewModel.Parse(book.ItemTitle_Master.BookCategory.Coupons, book.Price) : null
            };

            return res;
        }

        public static IEnumerable<BookViewModel> Parse(IEnumerable<Book> books)       // list is Base class
        {
            return books
                    .Select(book => new BookViewModel()
                    {
                        BookId = book.Id,
                        BookImage = image_urlResolver.getBookImage(book.BookImage),
                        book_title = book.Title,
                        Category = book.ItemTitle_Master.BookCategory.Name,            // using navigation property to bind value
                        Class = book.ItemTitle_Master.Class.className,
                        Subject = book.ItemTitle_Master.Subject.SubjectName,
                        Price = book.Price,
                        Publisher = book.Publisher,
                        In_stock = book.in_Stock,
                        CouponInfo = CouponViewModel.Parse(book.ItemTitle_Master.BookCategory.Coupons, book.Price)
                    });
        }

    }

    public class BundleViewModel
    {
        // dynamic type allow us to return anonymous parameter/Type
        public static dynamic Parse(IQueryable<BookBundle> entityList)
        {
            // to use string.format() with select list > 1st need to convert data into a list :)
            return entityList.ToList().Select(bundle => new
            {
                bundleInfo = new
                {
                    bundle.Id,
                    bundle.Name,
                    bundle.bundle_totalPrice,
                    bundle.books_totalPrice,
                    bundle.Class.className
                },
                bookInfo = bundle.bundle_details.Select(bundleDetail => new
                {
                    BookId = bundleDetail.book.Id,
                    BookImage = image_urlResolver.getBookImage(bundleDetail.book.BookImage),
                    title = bundleDetail.book.Title,
                    price = bundleDetail.book.Price
                })
            });
        }
    }

    public class Book_contentViewModel
    {
        public string content_name { get; set; }
        public string content_description { get; set; }

        internal static Book_contentViewModel Parse(BookContent content)
        {
            return new Book_contentViewModel()
            {
                content_name = content.Name,
                content_description = content.Description
            };
        }

    }

    public class CouponViewModel
    {
        public CouponType couponType { get; set; }
        public int couponPrice { get; set; }
        public decimal book_newPrice { get; set; }

        public static CouponViewModel Parse(Coupon coupon, decimal bookPrice)
        {
            if (coupon == null)         // handle null
                return null;

            var sys_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Date;

            if (!(coupon.Status &&
                sys_date >= coupon.Start_time &&
                sys_date <= coupon.End_time))
                return null;         // if coupon is not valid

            return new CouponViewModel()            // return new object of same type
            {
                couponType = coupon.DiscountType,
                couponPrice = coupon.Coupon_amount,
                book_newPrice = coupon.DiscountType == CouponType.FlatDiscount        // calculating BookPrice
                                      ? bookPrice - coupon.Coupon_amount
                                      : bookPrice - (bookPrice * coupon.Coupon_amount) / 100
            };

        }

    }

    //public class SugestionViewModel
    //{
    //    public int classId { get; set; }
    //    public int searchId { get; set; }
    //    public int bookId { get; set; }
    //}

}