using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Context.Application;
using Nebim.Era.Plt.Core.Types;
using Nebim.Era.Plt.Data.EntityFramework;
using Nebim.Era.Plt.EventBus.EventBus;

namespace Infrastructure.Data;

public class CustomerDbContext : EraDbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IAppContext appContext, IEventBus? eventBus) : base(options, appContext, eventBus)
    {
    }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Passport> Passports { get; set; } = null!;
    public DbSet<Address> CustomerAddresses { get; set; } = null!;
}

[UsedImplicitly]
public class CustomerDbContextFactory : MigrationDbContextFactory<CustomerDbContext>
{
}
