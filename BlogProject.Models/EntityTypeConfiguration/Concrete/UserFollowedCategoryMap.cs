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
    public class UserFollowedCategoryMap : IEntityTypeConfiguration<UserFollowedCategory> 
    {
        public void Configure(EntityTypeBuilder<UserFollowedCategory> builder)
        {
            // Navigation Property.

            builder.HasKey(a => new { a.AppUserID, a.CategoryID }); 

            builder.HasOne(a=>a.AppUser).WithMany(a=>a.UserFollowedCategories).HasForeignKey(a=>a.AppUserID);
              
            builder.HasOne(a => a.Category).WithMany(a => a.UserFollowedCategories).HasForeignKey(a => a.CategoryID);
            
        }
    }
}
