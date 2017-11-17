using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        //***********************  Book Filter  *************************

        public Book GetById(long id)
        {
            return FindBy(x => x.Id == id).SingleOrDefault();
        }

        public IQueryable<long> GetBooksId(long subjectId, long classId, long categoryId)
        {
            var booklist = GetAll();

            if (subjectId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.SubjectId == subjectId);

            if (classId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.ClassId == classId);

            if (categoryId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.CategoryId == categoryId);

            return booklist.Select(x => x.Id);
        }

        public Book GetByTitleId(long titleId)
        {
            return _dbset.SingleOrDefault(x => x.Title_Mid == titleId 
                                            && x.Status == true);
        }

        public Book GetByTitle(string title)
        {
            return FindBy(x => x.Title == title).FirstOrDefault();
        }
        public Book GetByISBN(string isbn)
        {
            return FindBy(x => x.ISBN == isbn).FirstOrDefault();
        }

        public IEnumerable<Book> GetByAuthor(string authorName)
        {
            return _dbset.Where(x => x.Publisher == authorName).AsEnumerable();
        }

        public Book GetByEdition(string edition)
        {
            return FindBy(x => x.Edition == edition).FirstOrDefault();
        }
        public IEnumerable<Book> GetByWeight(decimal weight)
        {
            return _dbset.Where(x => x.Weight == weight).AsEnumerable();
        }
        public IEnumerable<Book> GetByPrice(decimal price)
        {
            return _dbset.Where(x => x.Price == price).AsEnumerable();
        }
        public IEnumerable<Book> GetByPriceRange(decimal startprice, decimal endprice)
        {
            return _dbset.Where(x => x.Price >= startprice && x.Price <= endprice).AsEnumerable();
        }
        public IEnumerable<Book> GetByCategory(long categoryId)
        {
            return _dbset.Where(x => x.ItemTitle_Master.CategoryId == categoryId).AsEnumerable();
        }

        public Book get_books_byId(long bookId)
        {
            // find use to pass only PK value
            var book = _dbset.Find(bookId);

            return book;
        }

        // ***************  Used for filteration Records  **********************

        //public IQueryable<Book> GetAllBooks()
        //{
        //    return _dbset.Include(x => x.Category);
        //}

        // bcoz where return IQueryable type > then after we can convert it into IEnumerable
        // books is the filtered records 
        public IEnumerable<Book> FilterByClassAndSubject(long classId, long subjectid, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId && x.Id == subjectid);
        }

        public IEnumerable<Book> FilterByClassSubjectAndCategory(long classId, long subjectid, IEnumerable<long> categoriesId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId && x.Id == subjectid && categoriesId.Contains(x.ItemTitle_Master.CategoryId));
        }

        public IQueryable<Book> FilterByClassId(long? classId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId.Value);
        }

        public IQueryable<Book> FilterBySubjectId(long? subjectId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.SubjectId == subjectId.Value);
        }

        public IQueryable<Book> FilterBycategoriesId(IEnumerable<long> categoriesId, IQueryable<Book> books)
        {
            return books.Where(x => categoriesId.Contains(x.ItemTitle_Master.CategoryId));
        }

        //public IQueryable<Book> parseBookImage(IQueryable<Book> books)
        //{
        //    return books.Select(x =>
        //    new Book
        //    {
        //        Id = x.Id,
        //        BookImage = string.Format("{0}{1}",
        //                      image_urlResolver.bookImage_main,
        //                      x.BookImage),
        //        Title = x.Title,
        //        Category = x.Category,
        //        Class = x.Class,
        //        Subject = x.Subject,
        //        Price = x.Price,
        //        Publisher = x.Publisher,
        //        in_Stock = x.in_Stock
        //    });
        //}

        public List<string> upload_book_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_Images_toTemp(tempPath);
        }

        public bool check_book(long classId, long subjectId, long bookCategoryId)
        {
            return _dbset.Any(x => x.ItemTitle_Master.ClassId == classId
                                && x.ItemTitle_Master.SubjectId == subjectId
                                && x.ItemTitle_Master.CategoryId == bookCategoryId
                              );
        }

        public IQueryable<Book> GetByStatus(bool status)
        {
            return FindBy(x => x.Status == status
                            && x.ItemTitle_Master.Subject.Status == true         // only fetch active records
                            && x.ItemTitle_Master.BookCategory.Status == true);
        }

        public IQueryable<Book> getbookCategory(long subjectId, long classId)
        {
            return FindBy(x => x.ItemTitle_Master.SubjectId == subjectId
                            && x.ItemTitle_Master.ClassId == classId
                            && x.Status == true);
        }

    }
}
