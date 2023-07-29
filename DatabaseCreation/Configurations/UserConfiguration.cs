using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(30);
            builder.Property(a => a.Password).HasMaxLength(128);

            builder.HasCheckConstraint("CK_Password", "LEN([Password]) > 15").ToTable("User");
        }
    }
}
