using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nebim.Era.Plt.Comm.Customer.Domain;

namespace Infrastructure.Data.EntityTypeConfigurations;

using Nebim.Era.Plt.Data.Mysql;

public class PassportEntityTypeConfiguration : IEntityTypeConfiguration<Passport>
{
    public void Configure(EntityTypeBuilder<Passport> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnType(MySqlDbTypes.UuidType).IsRequired();

        builder.Property(t => t.CustomerId).HasColumnType(MySqlDbTypes.UuidType).IsRequired();
        builder.Property(t => t.Nationality).HasColumnType(MySqlDbTypes.IntType).IsRequired();
        builder.Property(t => t.IssuingStateCode).HasColumnType(MySqlDbTypes.IntType).IsRequired();
        builder.Property(t => t.Number).HasColumnType(MySqlDbTypes.VarChar20Type).IsRequired();
        builder.Property(t => t.IssueDate).HasColumnType(MySqlDbTypes.DatetimeType).IsRequired();

        builder.Property(o => o.TenantId).HasColumnType(MySqlDbTypes.UuidType).IsRequired();
        builder.Property(o => o.CreatedAt).HasColumnType(MySqlDbTypes.DatetimeType).IsRequired();
        builder.Property(o => o.CreatedBy).HasColumnType(MySqlDbTypes.UuidType);
        builder.Property(o => o.UpdatedAt).HasColumnType(MySqlDbTypes.DatetimeType).IsRequired(false);
        builder.Property(o => o.UpdatedBy).HasColumnType(MySqlDbTypes.UuidType).IsRequired(false);
    }
}
