namespace IntegrationTests;

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Nebim.Era.Plt.Data.Mysql;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        var configuration = ConfigurationHelper.BuildConfiguration(userSecretsAssembly: GetType().Assembly);
        var connectionString = MySqlConnectionStringProvider.Build(configuration["MySql:ConnectionString"]);
        DbContextOptionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion)
            .UseSnakeCaseNamingConvention();
        InitializeDatabase();
    }

    public CustomerDbContext CreateDbContext()
    {
        return new CustomerDbContext(DbContextOptionsBuilder.Options, new FakeAppContext(), new FakeEventBus());
    }

    private DbContextOptionsBuilder<CustomerDbContext> DbContextOptionsBuilder { get; }
    private static readonly object InitializeDatabaseLock = new();
    private static volatile bool _databaseInitialized;

    private void InitializeDatabase()
    {
        if (_databaseInitialized == false)
        {
            lock (InitializeDatabaseLock)
            {
                if (_databaseInitialized == false)
                {
                    using (var context = CreateDbContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _databaseInitialized = true;
                }
            }
        }
    }

    public void Dispose()
    {
        // aynı serviste 2 aggregate olunca biri create olurken diğeri delete ediyor paralel çalışmalarına çözüm lazım

        // using (var context = CreateDbContext())
        // {
        //     context.Database.EnsureDeleted();
        // }
    }
}

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
