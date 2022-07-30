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
    public class CategoryMap : BaseMap<Category> 
    {       
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            // Name  Required
            builder.Property(a => a.Name).IsRequired(true);

            // Description Required
            builder.Property(a => a.Description).IsRequired(true);

            base.Configure(builder); 
        }
    }
}
