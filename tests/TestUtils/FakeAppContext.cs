using Nebim.Era.Plt.Context.Application;
using Nebim.Era.Plt.Context.GlobalFilter;
using Nebim.Era.Plt.Context.Tenant;
using Nebim.Era.Plt.Context.User;
using Nebim.Era.Plt.Core.Events;
using Nebim.Era.Plt.EventBus.EventBus;

public class FakeAppContext : IAppContext
{
    public ICurrentTenant CurrentTenant { get; } = new FakeTenantContext();
    public ICurrentUser CurrentUser { get; } = new FakeUserContext();
    public IFilterState SoftDeleteFilter { get; } = new FakeFilterState();
    public IFilterState MultiTenantFilter { get; } = new FakeFilterState();
}

public class FakeTenantContext : ICurrentTenant
{
    public static Guid CurrentTenantId = Guid.Parse("2741a9bd-f4e3-4db0-bbde-f9bf9219dc02");
    public Guid TenantId => CurrentTenantId;
    public bool IsAdminContext => true;
}

public class FakeUserContext : ICurrentUser
{
    public static Guid FakeUserId = Guid.Parse("00c43b1c-9be3-47a7-8431-5dc2017ef498");
    public static Guid FakeStoreId = Guid.Parse("00c43b1d-9be3-47a7-8431-5dc2017ef498");

    public void SetUserId(Guid userId)
    {
        // letft blank intentionally
    }

    public void SetStoreId(Guid storeId)
    {
        // letft blank intentionally
    }

    public Guid UserId { get; } = FakeUserId;
    public Guid? StoreId { get; } = FakeStoreId;
    public string CurrentCorrelationId { get; }
}

public class FakeFilterState : IFilterState
{
    public bool IsActive { get; }
}

public class FakeEventBus : IEventBus
{
    public Task Publish<T>(Guid key, T @event, CancellationToken cancellationToken = new CancellationToken()) where T : class, IIntegrationEvent
    {
        return Task.CompletedTask;
    }

    public Task Publish<T>(T @event, CancellationToken cancellationToken = new CancellationToken()) where T : class, IIntegrationEvent
    {
        return Task.CompletedTask;
    }
}