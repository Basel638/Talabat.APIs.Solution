using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config.OrderConfig
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, shippingAddrees => shippingAddrees.WithOwner());

			builder.Property(order => order.Status)
				.HasConversion(
				
				(OStatus) => OStatus.ToString(),
				(OStatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)

				);

			builder.Property(Order => Order.Subtotal)
				.HasColumnType("decimal(12,2)");


			builder.HasOne(Order => Order.DeliveryMethod)
				.WithMany()
				.OnDelete(DeleteBehavior.SetNull);


			builder.HasMany(Order => Order.Items)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
