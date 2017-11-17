using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Runtime.InteropServices;
using SilverzoneERP.Entities.Models.Common;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        public SilverzoneERPContext _dbContext;
        public readonly IDbSet<T> _dbset;
       
        public BaseRepository (SilverzoneERPContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbset.AsQueryable<T>();
        }

        public virtual T FindById(long id)
        {
            return _dbset.Find(id);     // to find record using identity value > i.e is on each table
        }

        public IQueryable<T> FindBy(Expression<Func<T,bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        // if we perform DML operation into multiple tables/bulk insert in a single table in a method
        // then we will pass save = false & call save() explicitly
        public virtual T Create(T entity, bool save = true)  // default value set to true
        {
            if (entity == null) throw new ArgumentNullException("entity not found");

            _dbset.Add(entity);
            if (save) Save();
            return entity;
        }

        public virtual T Update(T entity, bool save = true)
        {
            if (entity == null) throw new ArgumentNullException("entity not found");

            _dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            if (save) Save();
            return entity;
        }

        // delete single record
        public virtual T Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity not found");

            _dbset.Remove(entity);
            Save();         // save changes after delete record
            return entity;
        }

        /* To Delete multiple record at a time :)
           <param name="predicate"> contains an expression like > where clause OR list */
        public virtual void DeleteWhere(IEnumerable<T> list)
        {
            _dbContext.Set<T>()
                .RemoveRange(list);       // directly contain a list of record

            Save();   // save changes 
        }


        /* suppose we r inseting data into multiple tables/bulk insert in a single table in a method
           then instead of calling save() using Add() again & again
           we r going to call save() once after add records in the table
         */
        public virtual void Save() 
        {
            _dbContext.SaveChanges();
        }

        // transaction will start for the whole DB > 
        // so can use if want to use for multiple table modification ..
        public DbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
      
        // conert datetime into a specific format
        // bcoz project uploaded on server will return server Machine date rather then Client Machine
        public DateTime get_DateTime([Optional] DateTime date)
        {
            if (date == DateTime.MinValue)
                return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                return Convert.ToDateTime(date.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        //****************************  Clean up Recources ************************************

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        // to clean up the recources,when this class finish its work OR no longer in use
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
