using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation.Configurations
{
    internal class ShopplingListDetailsConfiguration : IEntityTypeConfiguration<ShoppingListDetails>
    {
        public void Configure(EntityTypeBuilder<ShoppingListDetails> builder)
        {
            builder.HasOne(a => a.ShoppingList)
                .WithMany(b => b.Products)
                .HasForeignKey(c => c.ShoppingListId);

            builder.HasOne(a => a.Product)
                .WithMany(b => b.ShoppingLists)
                .HasForeignKey(c => c.ProductId);

            builder.HasKey(a => new { a.ShoppingListId, a.ProductId });

            builder.Property(a => a.Quantity).HasDefaultValue(0);
            builder.Property(a => a.Note).HasMaxLength(100);

            builder.HasCheckConstraint("CK_Quantity", "[Quantity] > 0").ToTable("ShoppingListDetails");
        }
    }
}
