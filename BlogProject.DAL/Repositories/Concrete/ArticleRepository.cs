using BlogProject.DAL.Context;
using BlogProject.DAL.Repositories.Abstract;
using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DAL.Repositories.Concrete
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository 
    {
        public ArticleRepository(ProjectContext context) : base(context) 
        {

        }

        public void Read(Article article)
        {
            article.ReadCounter++;
            _table.Update(article);
            _context.SaveChanges();
        }
    }
}
