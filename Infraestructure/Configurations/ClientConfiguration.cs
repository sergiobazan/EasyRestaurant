using Domain.Clients;
using Domain.Orders;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable(nameof(Client));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasConversion(
                name => name.Value, 
                value => new Name(value))
            .HasMaxLength(256);

        builder.OwnsOne(client => client.Phone, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Prefix).HasMaxLength(3);
            phoneBuilder.Property(p => p.Value).HasMaxLength(9);
        });

        builder.Property(client => client.Gender)
            .HasConversion(
                gender => gender.Value,
                value => Gender.Create(value).Value)
            .HasMaxLength(1);

        builder
            .HasMany<Order>()
            .WithOne()
            .HasForeignKey(order => order.ClientId)
            .IsRequired();
    }
}
