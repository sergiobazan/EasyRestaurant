using Domain.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasIndex(m => m.Id);

        builder
            .HasMany(menu => menu.Orders)
            .WithOne()
            .HasForeignKey(order => order.MenuId)
            .IsRequired();
    }
}
