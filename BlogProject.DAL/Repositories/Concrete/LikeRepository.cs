using BlogProject.DAL.Context;
using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DAL.Repositories.Concrete
{
    public class LikeRepository : ILikeRepository 
    {

        private readonly ProjectContext _context; 
        private readonly DbSet<Like> _table; 

        public LikeRepository(ProjectContext context)
        {
            _context = context;
            _table = context.Set<Like>(); 
        }

        // Create
        public void Create(Like entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        // Delete
        public void Delete(Like entity)
        {
            _table.Remove(entity);
            _context.SaveChanges();
        }

        // Get List
        public List<Like> GetLikes(Expression<Func<Like, bool>> expression)
        {
            return _table.Where(expression).ToList();
        }

        // Get One
        public Like GetDefault(Expression<Func<Like, bool>> expression)
        {
            return _table.Where(expression).FirstOrDefault();
        }
    }
}
