using SilverzoneERP.Entities.ViewModel.Site;
using SilverzoneERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class BookController : ApiController
    {
        IBookRepository bookRepository;
        IClassRepository classRepository;
        ICouponRepository couponRepository;
        IBundleRepository bundleRepository;
        IBundleDetailRepository bundleDetailRepository;

        [HttpGet]
        public IHttpActionResult get_bookDetail_byId(int bookId)
        {
            var book = BookViewModel.Parse(bookRepository.GetById(bookId));
            var isBundle_exist = bundleDetailRepository.GetAll().Any(x => x.BookId == bookId);

            if (!isBundle_exist)    // if bundle is not avaiable then return required propertieas :)
            {
                return Ok(new
                {
                    result = new            // new properties asign to result
                    {
                        book_info = book,
                        bookCombo_status = isBundle_exist,
                    }
                });
            }

            // if bundle is avaiable then continue run this code
            var bundleId_list = bundleDetailRepository
                                .FindBy(x => x.BookId == bookId)
                                .Select(x => x.BundleId).ToList();

            var comboDetail = BundleViewModel.Parse(
                              bundleRepository.FindBy(x => bundleId_list.Contains(x.Id) && x.Status == true)
                                );

            return Ok(new
            {
                result = new
                {
                    book_info = book,
                    bookCombo_status = isBundle_exist,
                    comboInfo = comboDetail
                }
            });
        }

        [HttpGet]
        public IHttpActionResult get_bookBundleDetail_byId(int bundleId)
        {
            var bundleDetail = BundleViewModel.Parse(
                              bundleRepository.FindBy(x => x.Id == bundleId)
                                );

            return Ok(new { result = bundleDetail });
        }

        [HttpGet]
        public IHttpActionResult getbook_suggestions(int bookId)
        {
            var bookInfo = bookRepository.GetById(bookId);

            var books = BookViewModel.Parse(
            bookRepository.GetByStatus(true)
                            .Where(x => x.ItemTitle_Master.ClassId == bookInfo.ItemTitle_Master.ClassId && x.ItemTitle_Master.SubjectId == bookInfo.ItemTitle_Master.SubjectId && x.Id != bookInfo.Id)
                            .OrderBy(r => Guid.NewGuid())   // new guid use to genrate random records
                            .Take(3)                       // take 3 use to select TOP 3 records
            );

            return Ok(new { result = books });
        }

        [HttpGet]
        public IHttpActionResult getbook_recommends(int bookId)
        {
            var bookInfo = bookRepository.GetById(bookId);

            var books = BookViewModel.Parse(
            bookRepository.GetByStatus(true)
                            .Where(x => x.ItemTitle_Master.ClassId == bookInfo.ItemTitle_Master.ClassId && x.ItemTitle_Master.CategoryId == bookInfo.ItemTitle_Master.CategoryId && x.Id != bookInfo.Id)
                            .OrderBy(r => Guid.NewGuid())
                            .Take(3)
            );

            return Ok(new { result = books });
        }

        [HttpGet]
        public IHttpActionResult getbook_bundles(int classId)
        {
            var bundles = BundleViewModel.Parse(
                              bundleRepository.FindBy(x => x.ClassId == classId && x.Status == true)
                                );

            return Ok(new { result = bundles });
        }


        [HttpGet]
        public IHttpActionResult searchBooks(
            long classId,                                   // We must have a classId while srearciing a record
            [FromUri] ICollection<long> book_categoriesId,  // to support array type use > FromUri
            int? subjectId = null)                          // we need to pass explicit value to make it as > default param
        {
            var books = bookRepository.GetByStatus(true);       // only active books

            books = bookRepository.FilterByClassId(classId, books);           // we must get a clssId to filter

            // we dont need to Filter with ClassId & SubjectId > bcoz previous book record contain data with a particular classId
            // then we r again filtering from that record with subjectId
            if (subjectId.HasValue)
                books = bookRepository.FilterBySubjectId(subjectId.Value, books);

            if (book_categoriesId.Count != 0)       // count property is have icollection Type
                books = bookRepository.FilterBycategoriesId(book_categoriesId, books);


            return Ok(new { result = BookViewModel.Parse(books.ToList()) });
        }


        public IHttpActionResult getAllClass()
        {
            var _result = classRepository.GetAll().Select(x => new { x.Id, x.className });

            return Ok(new { result = _result });
        }

        // *****************  Constructor  ********************************

        public BookController(
            IBookRepository _bookRepository,
            IClassRepository _classRepository,
            ICouponRepository _couponRepository,
            IBundleRepository _bundleRepository,
            IBundleDetailRepository _bundleDetailRepository)
        {
            bookRepository = _bookRepository;
            classRepository = _classRepository;
            couponRepository = _couponRepository;
            bundleRepository = _bundleRepository;
            bundleDetailRepository = _bundleDetailRepository;

        }


    }
}
