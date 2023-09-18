using JetBrains.Annotations;
using Nebim.Era.Plt.Context.Application;
using Nebim.Era.Plt.Core;

namespace Nebim.Era.Plt.Comm.Customer.Service.HttpClient;

[UsedImplicitly]
public sealed class EraPlatformHeaderPropagationHandler : DelegatingHandler
{
    private readonly IAppContext _appContext;

    public EraPlatformHeaderPropagationHandler(IAppContext appContext)
    {
        _appContext = appContext;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(KnownHttpHeader.TenantId, _appContext.CurrentTenant.TenantId.ToString());
        request.Headers.Add(KnownHttpHeader.UserId, _appContext.CurrentUser.UserId.ToString());
        request.Headers.Add(KnownHttpHeader.StoreId, _appContext.CurrentUser.StoreId.ToString());
        request.Headers.Add(KnownHttpHeader.CorrelationId, _appContext.CurrentUser.CurrentCorrelationId);

        return base.SendAsync(request, cancellationToken);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(KnownHttpHeader.TenantId, _appContext.CurrentTenant.TenantId.ToString());
        request.Headers.Add(KnownHttpHeader.UserId, _appContext.CurrentUser.UserId.ToString());
        request.Headers.Add(KnownHttpHeader.StoreId, _appContext.CurrentUser.StoreId.ToString());
        request.Headers.Add(KnownHttpHeader.CorrelationId, _appContext.CurrentUser.CurrentCorrelationId);
        return base.Send(request, cancellationToken);
    }
}