using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreation.Configurations
{
    internal class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
    {
        public void Configure(EntityTypeBuilder<ShoppingList> builder)
        {
            builder.HasOne(a => a.User)
                .WithMany(b => b.ShoppingLists)
                .HasForeignKey(c => c.UserId)
                .HasPrincipalKey(d => d.UserId);

            builder.Property(a => a.ShoppingListName).HasMaxLength(30);
        }
    }
}
