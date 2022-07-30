using BlogProject.Models.Entities.Concrrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.EntityTypeConfiguration.Concrete
{
    public class LikeMap : IEntityTypeConfiguration<Like> 
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(a => new { a.AppUserID, a.ArticleID }); 

            // Navigation Property.
            builder.HasOne(a => a.AppUser).WithMany(a => a.Likes).HasForeignKey(a => a.AppUserID);

            // Navigation Property.
            builder.HasOne(a => a.Article).WithMany(a => a.Likes).HasForeignKey(a => a.ArticleID);
        }
    }
}
