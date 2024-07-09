using Domain.Menus;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasConversion(
                name => name.Value,
                value => new Name(value));

        builder
            .HasMany(menu => menu.Orders)
            .WithOne()
            .HasForeignKey(order => order.MenuId)
            .IsRequired();
    }
}
