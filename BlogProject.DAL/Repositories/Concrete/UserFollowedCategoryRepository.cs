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
    public class UserFollowedCategoryRepository : IUserFollowedCategoryRepository
    {

        private readonly ProjectContext _context;
        private readonly DbSet<UserFollowedCategory> _table;

        public UserFollowedCategoryRepository(ProjectContext context)
        {
            _context = context;
            _table = context.Set<UserFollowedCategory>();
        }

        // Create
        public void Create(UserFollowedCategory entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        // Delete
        public void Delete(UserFollowedCategory entity)
        {
            _table.Remove(entity);
            _context.SaveChanges();
        }

        // List
        public List<UserFollowedCategory> GetFollowedCategories(Expression<Func<UserFollowedCategory, bool>> expression)
        {
            return _table.Where(expression).ToList();
        }

        // Get One
        public UserFollowedCategory GetDefault(Expression<Func<UserFollowedCategory, bool>> expression)
        {
            return _table.Where(expression).FirstOrDefault();
        }
    }
}
