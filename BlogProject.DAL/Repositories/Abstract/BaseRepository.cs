using BlogProject.DAL.Context;
using BlogProject.DAL.Repositories.Interfaces.Abstract;
using BlogProject.Models.Entities.Abstract;
using BlogProject.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DAL.Repositories.Abstract
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity 
    {

        protected readonly ProjectContext _context; 

        protected readonly DbSet<T> _table; 
        public BaseRepository(ProjectContext context)
        {
            _context = context;
            _table = context.Set<T>(); 
        }

        // Any
        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _table.Any(expression);

        }               

        // Create
        public void Create(T entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        // Delete
        public void Delete(T entity)
        {
            entity.Statu = Statu.Passive;
            entity.RemovedDate = DateTime.Now;
            _context.SaveChanges();

        }

        // GetByDefault
        public TResult GetByDefault<TResult>
            (
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
            )
        {
            IQueryable<T> query = _table; 

            if (include != null) 
            {
                query = include(query); 
            }
            if (expression != null) 
            {
                query = query.Where(expression); 
            }
            return query.Select(selector).First(); 
        }

        // GetByDefaults
        public List<TResult> GetByDefaults<TResult>
            (
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null
            )
        {
            IQueryable<T> query = _table; 

            if (include != null) 
            {
                query = include(query); 
            }

            if (expression != null) 
            {
                query = query.Where(expression); 
            }
            if (orderby != null) 
            {
                return orderby(query).Select(selector).ToList(); 
            }
            return query.Select(selector).ToList(); 
        }

        // GetDefault
        public T GetDefault(Expression<Func<T, bool>> expression)
        {
            return _table.Where(expression).FirstOrDefault();
        }

        // GetDefaults
        public List<T> GetDefaults(Expression<Func<T, bool>> expression)
        {
            return _table.Where(expression).ToList();
        }

        // Update
        public async void Update(T entity)
        {
            entity.Statu = Statu.Modified;
            entity.ModifiedDate = DateTime.Now;            
            _table.Update(entity);
            _context.SaveChanges();
        }

        // Admin Approve
        public void Approve(T entity)
        {
            entity.Statu = Statu.Active;
            entity.AdminCheck = AdminCheck.Approved;
            _table.Update(entity);
            _context.SaveChanges();
        }

        // Admin Reject
        public void Reject(T entity)
        {
            entity.Statu = Statu.Passive;
            entity.AdminCheck = AdminCheck.Rejected;
            _table.Update(entity);
            _context.SaveChanges();
        }
    }
}
