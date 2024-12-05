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
    public class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status).HasConversion(v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");

            builder.OwnsOne(o => o.ShippingAddress, s => s.WithOwner());
            
            builder.HasOne(o => o.DeliveryMethod).WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
