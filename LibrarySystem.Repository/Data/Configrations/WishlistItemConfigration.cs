using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Enums;
using LibrarySystem.Core.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Data.Configrations
{
    public class WishlistItemConfigration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.Property(OI => OI.Price).HasColumnType("decimal(18,2)");
            builder.Property(b => b.Category)
                 .HasConversion(c => c.ToString(), c => (Category)Enum.Parse(typeof(Category), c));
           
        }


    }
}







