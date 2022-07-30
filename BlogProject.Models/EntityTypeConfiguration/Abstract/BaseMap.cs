using BlogProject.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.EntityTypeConfiguration.Abstract
{
    public abstract class BaseMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity 
    {
        public virtual void Configure(EntityTypeBuilder<T> builder) 
        {
          
            builder.HasKey(a => a.ID);

            // CreateDate required. 
            builder.Property(a => a.CreateDate).IsRequired(true);
            // ModifiedDate nullable
            builder.Property(a => a.ModifiedDate).IsRequired(false);
            // RemovedDate nullable
            builder.Property(a => a.RemovedDate).IsRequired(false);
            // Statu required
            builder.Property(a => a.Statu).IsRequired(true);

        }
    }
}
