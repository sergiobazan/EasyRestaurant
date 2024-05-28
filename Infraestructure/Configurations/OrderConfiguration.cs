using Domain.Orders;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => o.Id);

        builder.Property(o => o.Description)
            .HasConversion(
                description => description!.Value,
                value => new Description(value));

        builder
            .HasMany(o => o.Dishes)
            .WithMany();

    }
}
