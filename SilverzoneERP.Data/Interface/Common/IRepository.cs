using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace SilverzoneERP.Data
{
    public interface IRepository<T> : IDisposable where T : BaseEntity    // methods of base repositry
    {
        IQueryable<T> GetAll();
        T FindById(long id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Create(T entity, bool save = true);
        T Update(T entity, bool save = true);
        T Delete(T entity);
        void DeleteWhere(IEnumerable<T> list);

        DbContextTransaction BeginTransaction();
        void Save();

        DateTime get_DateTime([Optional] DateTime date);
    }
}
