﻿using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.EntityTypeConfiguration.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.EntityTypeConfiguration.Concrete
{
    public class AppUserMap  : BaseMap<AppUser> 
    {       
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // FirstName MaxLength(30) required
            builder.Property(a => a.FirstName).HasMaxLength(30).IsRequired(true);

            // LastName MaxLength(40) required
            builder.Property(a => a.LastName).HasMaxLength(40).IsRequired(true);

            // UserName  required
            builder.Property(a => a.UserName).IsRequired(true);

            // Password  required
            builder.Property(a => a.Password).IsRequired(true);

            // Image MaxLength(200) required
            builder.Property(a => a.Image).HasMaxLength(200).IsRequired(true);

            base.Configure(builder); 
        }
    }
}
