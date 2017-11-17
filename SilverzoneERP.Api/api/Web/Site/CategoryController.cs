using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class CategoryController : ApiController
    {
        IBookCategoryRepository bookCategoryRepository;
        IBookRepository bookRepository;
        IitemTitle_MasterRepository itemTitle_MasterRepository;

        [HttpGet]
        public IHttpActionResult category_List()
        {
            // where category contains a list<a> > anonymous type 
            var category = (from data in bookCategoryRepository.GetByStatus(true)
                            select new
                            {
                                id = data.Id,
                                name = data.Name
                            });

            return Ok(new { result = category });
        }

        [HttpGet]
        public IHttpActionResult get_category_byClass(long subjectId, long classId)
        {
            // where category contains a list<a> > anonymous type 
            var category = bookRepository.getbookCategory(subjectId, classId)
                .Select(x => new
                {
                    Id = x.Id,      // book id
                    x.ItemTitle_Master.CategoryId,
                    categoryName = x.ItemTitle_Master.BookCategory.Name
                });

            return Ok(new { result = category });
        }

        [HttpGet]
        public IHttpActionResult get_bookTitle()
        {
            // where category contains a list<a> > anonymous type 
            var category = itemTitle_MasterRepository
                           .GetAll()
                           .GroupBy(x => x.SubjectId)
                           .Select(a => new
                           {
                               subject = new
                               {
                                   Id = a.Key,
                                   Name = a.FirstOrDefault().Subject.SubjectName,
                                   Class = a.GroupBy(x => x.ClassId)
                                            .Select(b => new
                                            {
                                                Id = b.Key,
                                                Name = b.FirstOrDefault().Class.className,
                                                category = b.GroupBy(x => x.CategoryId)
                                                            .Select(c => new
                                                            {
                                                                Id = c.Key,
                                                                title_mId = c.FirstOrDefault().Id,
                                                                Name = c.FirstOrDefault().BookCategory.Name,
                                                            })
                                            })
                               }
                           });

            return Ok(new { result = category });
        }

        [HttpGet]
        public IHttpActionResult get_existedBook_Title()
        {
            var books = bookRepository.GetAll().Select(x => new
            {
                bookId = x.Id,
                x.Title_Mid
            });

            var books_titleId = bookRepository.GetAll().Select(x => x.Title_Mid);

            // where category contains a list<a> > anonymous type 
            var category = itemTitle_MasterRepository
                           .FindBy(title => books_titleId.Contains(title.Id))
                           .GroupBy(x => x.SubjectId)
                           .Select(a => new
                           {
                               subject = new
                               {
                                   Id = a.Key,
                                   Name = a.FirstOrDefault().Subject.SubjectName,
                                   Class = a.GroupBy(x => x.ClassId)
                                            .Select(b => new
                                            {
                                                Id = b.Key,
                                                Name = b.FirstOrDefault().Class.className,
                                                category = b.Where(z => z.SubjectId == a.Key && z.ClassId == b.Key)
                                                            .GroupBy(x => x.CategoryId)
                                                            .Select(c => new
                                                            {
                                                                Id = c.Key,
                                                                BookId = books.FirstOrDefault(book => book.Title_Mid == c.FirstOrDefault().Id).bookId,
                                                                Name = c.FirstOrDefault().BookCategory.Name,
                                                            })
                                            })
                               }
                           });
            return Ok(new { result = category });
        }


        // *****************  Constructor  ********************************

        public CategoryController(IBookCategoryRepository _bookCategoryRepository,
                                  IBookRepository _bookRepository,
                                  IitemTitle_MasterRepository _itemTitle_MasterRepository)
        {
            bookCategoryRepository = _bookCategoryRepository;
            bookRepository = _bookRepository;
            itemTitle_MasterRepository = _itemTitle_MasterRepository;
        }

    }
}
