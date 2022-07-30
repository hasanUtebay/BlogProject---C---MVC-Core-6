using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.EntityTypeConfiguration.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.EntityTypeConfiguration.Concrete
{
    public class CommentMap : BaseMap<Comment> 
    {       
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Text Required
            builder.Property(a => a.Text).IsRequired(true);

            // Navigation Property.
            builder.HasOne(a=>a.AppUser).WithMany(a=>a.Comments).HasForeignKey(a=>a.AppUserID);

            // Navigation Property.
            builder.HasOne(a => a.Article).WithMany(a => a.Comments).HasForeignKey(a => a.ArticleID);

            base.Configure(builder); 
        }
    }
}
