using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Data.Configrations
{
    public class WishlistConfigration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasOne(w => w.User)
                    .WithOne()
                    .HasForeignKey<Wishlist>(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
           

        }
       

    }

}
