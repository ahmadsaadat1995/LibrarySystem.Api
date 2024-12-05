using LibrarySystem.Core.Entitties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Repository.Data.Configurations
{
    public class BookPublisherConfiguration : IEntityTypeConfiguration<BookPublisher>
    {
        public void Configure(EntityTypeBuilder<BookPublisher> builder)
        {
            builder.HasKey(bp => new { bp.BookId, bp.PublisherId });

            builder.HasOne(bp => bp.Publisher)
                   .WithMany(p => p.BookPublishers)
                   .HasForeignKey(bp => bp.PublisherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bp => bp.Book)
                   .WithMany(b => b.BookPublishers)
                   .HasForeignKey(bp => bp.BookId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
