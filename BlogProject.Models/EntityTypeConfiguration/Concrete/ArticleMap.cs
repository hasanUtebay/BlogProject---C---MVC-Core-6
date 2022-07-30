using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.EntityTypeConfiguration.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.EntityTypeConfiguration.Concrete
{
    public class ArticleMap : BaseMap<Article> 
    {       
        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            // Title MaxLength(120) Required
            builder.Property(a => a.Title).HasMaxLength(120).IsRequired(true);

            // Content Required
            builder.Property(a => a.Content).IsRequired(true);

            // Image Required             
            builder.Property(a => a.Image).IsRequired(true);

            // Navigation Property. 
            builder.HasOne(a => a.AppUser).WithMany(a => a.Articles).HasForeignKey(a => a.AppUserID).OnDelete(DeleteBehavior.Restrict);
           
            builder.HasOne(a => a.Category).WithMany(a => a.Articles).HasForeignKey(a => a.CategoryID).OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder); 
        }
    }
}
