using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Infrastructure.Data.Entities;

namespace OrdersService.Infrastructure.Data.Configuration
{
    public class OrderProductLinkConfiguration: IEntityTypeConfiguration<OrderProductLink>
    {
        public void Configure(EntityTypeBuilder<OrderProductLink> builder)
        {
            builder.HasKey(e => new {e.OrderId, e.ProductId});
        }
    }
}