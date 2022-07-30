using BlogProject.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DAL.Repositories.Interfaces.Abstract
{
    public interface IBaseRepository<T> where T : BaseEntity 
    {
        // Create
        void Create(T entity);

        // Update
        void Update(T entity);

        // Delete
        void Delete(T entity);

        // Any
        bool Any(Expression<Func<T, bool>> expression);

        // Select One
        T GetDefault(Expression<Func<T, bool>> expression);

        // Select Many
        List<T> GetDefaults(Expression<Func<T, bool>> expression);

        
        TResult GetByDefault<TResult>
            (
            Expression<Func<T, TResult>> selector, 
            Expression<Func<T, bool>> expression, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
            );

       
        List<TResult> GetByDefaults<TResult>
            (
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> expression, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null 
            );

       
        void Approve(T entity);

        void Reject(T entity);

    }
}
