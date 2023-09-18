using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core.Serialization.Json;
using Nebim.Era.Plt.Core.Types;
using Nebim.Era.Plt.Data.Mysql;

namespace Infrastructure.Data.EntityTypeConfigurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnType(MySqlDbTypes.UuidType).IsRequired();

        builder.Property(o => o.Code).HasColumnType(MySqlDbTypes.VarChar50Type).IsRequired();
        builder.Property(o => o.Name).HasColumnType(MySqlDbTypes.VarChar50Type);
        builder.Property(o => o.Surname).HasColumnType(MySqlDbTypes.VarChar50Type);
        builder.Property(o => o.BirthDate).HasColumnType(MySqlDbTypes.DatetimeType);
        builder.Property(o => o.GenderCode).HasColumnType(MySqlDbTypes.IntType).HasConversion<int>();
        builder.Property(o => o.PhoneNumber).HasColumnType(MySqlDbTypes.VarChar20Type);
        builder.Property(o => o.Email).HasColumnType(MySqlDbTypes.VarChar50Type);
        builder.Property(o => o.CommunicationPreferences).HasColumnType(MySqlDbTypes.JsonType)
            .HasConversion(v => JsonSerde.Serialize(v, null),
                v => JsonSerde.Deserialize<List<CommunicationPreference>>(v, null)!);

        builder.Property(o => o.TenantId).HasColumnType(MySqlDbTypes.UuidType).IsRequired();
        builder.Property(o => o.CreatedAt).HasColumnType(MySqlDbTypes.DatetimeType).IsRequired();
        builder.Property(o => o.CreatedBy).HasColumnType(MySqlDbTypes.UuidType);
        builder.Property(o => o.UpdatedAt).HasColumnType(MySqlDbTypes.DatetimeType).IsRequired(false);
        builder.Property(o => o.UpdatedBy).HasColumnType(MySqlDbTypes.UuidType).IsRequired(false);

        // builder.HasIndex(e => new { e.Code, e.TenantId }).IsUnique();

        builder.OwnsMany(o => o.Addresses, ConfigureCustomerAddress);
    }

    private static void ConfigureCustomerAddress(OwnedNavigationBuilder<Customer, Address> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever().HasColumnType(MySqlDbTypes.UuidType).IsRequired();
        builder.Property(t => t.IsDefault).HasColumnType(MySqlDbTypes.TinyIntType).IsRequired();
        builder.Property(o => o.AddressName).HasColumnType(MySqlDbTypes.VarChar50Type).IsRequired();
        builder.Property(o => o.FirstName).HasColumnType(MySqlDbTypes.VarChar50Type).IsRequired();
        builder.Property(o => o.LastName).HasColumnType(MySqlDbTypes.VarChar50Type).IsRequired();
        builder.Property(o => o.PhoneNumber).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(o => o.Email).HasColumnType(MySqlDbTypes.VarChar50Type).IsRequired();
        builder.Property(t => t.IdentityNumber).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(o => o.CompanyName).HasColumnType(MySqlDbTypes.VarChar50Type);
        builder.Property(o => o.CountryId).HasColumnType(MySqlDbTypes.IntType);
        builder.Property(o => o.CountryName).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(o => o.CityId).HasColumnType(MySqlDbTypes.IntType);
        builder.Property(o => o.CityName).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(o => o.DistrictId).HasColumnType(MySqlDbTypes.IntType);
        builder.Property(o => o.DistrictName).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(t => t.AddressLine).HasColumnType(MySqlDbTypes.VarChar100Type);
        builder.Property(t => t.PostalCode).HasColumnType(MySqlDbTypes.VarChar10Type);
        builder.Property(t => t.InvoiceType).HasColumnType(MySqlDbTypes.IntType).HasConversion<int>().IsRequired();
        builder.Property(o => o.TaxOffice).HasColumnType(MySqlDbTypes.VarChar50Type);
        builder.Property(o => o.TaxNumber).HasColumnType(MySqlDbTypes.VarChar10Type);
    }
}
