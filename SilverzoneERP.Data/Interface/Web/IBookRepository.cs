using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetById(long id);
        Book GetByTitle(string title);
        Book GetByISBN(string isbn);
        IEnumerable<Book> GetByAuthor(string authorName);
        //IEnumerable<Book> GetByClassAndSubject(string stdclass,long subjectid);
        Book GetByEdition(string edition);
        IEnumerable<Book> GetByWeight(decimal weight);
        IEnumerable<Book> GetByPrice(decimal price);
        Book GetByTitleId(long titleId);
        IEnumerable<Book> GetByPriceRange(decimal startprice, decimal endprice);
        IEnumerable<Book> GetByCategory(long categoryId);
        Book get_books_byId(long bookId);

        IQueryable<Book> FilterByClassId(long? classId, IQueryable<Book> books);
        IQueryable<Book> FilterBySubjectId(long? subjectId, IQueryable<Book> books);
        IQueryable<Book> FilterBycategoriesId(IEnumerable<long> categoriesId, IQueryable<Book> books);

        List<string> upload_book_Image_toTemp(string tempPath);
        bool check_book(long classId, long subjectId, long bookCategoryId);
        IQueryable<Book> GetByStatus(bool status);
        IQueryable<Book> getbookCategory(long subjectId, long classId);
        IQueryable<long> GetBooksId(long subjectId, long classId, long categoryId);

    }
}
