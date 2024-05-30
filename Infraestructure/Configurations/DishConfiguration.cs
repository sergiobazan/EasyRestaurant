using Domain.Dishes;
using Domain.Menus;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations;

internal class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(d => d.Name)
            .HasConversion(
                name => name.Value,
                value => new Name(value))
            .HasMaxLength(100);

        builder.Property(d => d.Price)
            .HasConversion(
                price => price.Amount,
                amount => Price.Create(amount).Value);

        builder.Property(d => d.Description)
            .HasConversion(
                description => description.Value,
                value => new Description(value))
            .HasMaxLength (100);

        builder.Property(d => d.Quantity)
            .HasConversion(
                quantity => quantity.Value,
                value => Quantity.Create(value).Value);
    }
}
