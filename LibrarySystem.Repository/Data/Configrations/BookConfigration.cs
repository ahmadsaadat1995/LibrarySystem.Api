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
    
        
        public class BookConfigration : IEntityTypeConfiguration<Book>
        {
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder.Property(b => b.Title).HasMaxLength(200).IsRequired();
                builder.Property(b => b.Price).HasColumnType("decimal(18,2)").IsRequired();

                builder.Property(b => b.Category)
                    .HasConversion(c => c.ToString(), c => (Category)Enum.Parse(typeof(Category), c));

               
                builder.HasOne(b => b.Auther)
                       .WithMany(a => a.Books)
                       .HasForeignKey(b => b.AutherId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(b => b.AdditionalInfo, ai =>
            {
                ai.Property(a => a.Format).HasColumnName("Format")
                    .HasConversion(f => f.ToString(), f => (Format)Enum.Parse(typeof(Format), f));
                ai.Property(a => a.Language).HasColumnName("Language")
                    .HasConversion(l => l.ToString(), l => (Language)Enum.Parse(typeof(Language), l));
                ai.Property(a => a.DatePublished).HasColumnName("DatePublished");
            });


        }
    }

    }

