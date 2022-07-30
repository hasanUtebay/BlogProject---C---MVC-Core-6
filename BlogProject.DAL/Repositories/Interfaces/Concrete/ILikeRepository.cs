﻿using BlogProject.Models.Entities.Concrrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DAL.Repositories.Interfaces.Concrete
{
    public interface ILikeRepository
    {
        // Create
        void Create(Like entity);

        // Delete
        void Delete(Like entity);
        Like GetDefault(Expression<Func<Like, bool>> expression);
        List<Like> GetLikes(Expression<Func<Like, bool>> expression);
    }
}
