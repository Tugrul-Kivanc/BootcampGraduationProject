using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(a => a.Category)
                .WithMany(b => b.Products)
                .HasForeignKey(c => c.CategoryId)
                .HasPrincipalKey(d => d.CategoryId);

            builder.Property(a => a.Name).HasMaxLength(30);
        }
    }
}
