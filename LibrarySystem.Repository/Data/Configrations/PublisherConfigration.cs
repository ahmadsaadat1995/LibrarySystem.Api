using LibrarySystem.Core.Entitties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Data.Configrations
{
    public class PublisherConfigration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.Property(p => p.FullName).HasMaxLength(100).IsRequired();
            builder.HasMany(p => p.BookPublishers)
               .WithOne(bp => bp.Publisher)
               .HasForeignKey(bp => bp.PublisherId).OnDelete(DeleteBehavior.NoAction);
               
        }
    }
}
