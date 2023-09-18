using System.Security.Authentication;
using Microsoft.Extensions.Primitives;
using Nebim.Era.Plt.Context.Application;
using Nebim.Era.Plt.Core;

namespace Service.Middlewares;

using System.Diagnostics;
using Nebim.Era.Plt.OpenTelemetry.Logging;
using Nebim.Era.Plt.OpenTelemetry.Tracing;

public class AppContextResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public AppContextResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IAppContext appContext, IAppContextBuilder appContextBuilder,
        ILogger<AppContextResolutionMiddleware> logger)
    {
        var correlationId = GetCorrelationId(httpContext).ToString();

        httpContext
            .Response
            .Headers
            .TryAdd(KnownHttpHeader.CorrelationId, correlationId);

        httpContext
            .Request
            .Headers
            .TryAdd(KnownHttpHeader.CorrelationId, correlationId);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "not known";

        var tenantId = httpContext.Request.Headers[KnownHttpHeader.TenantId].FirstOrDefault() ??
                       httpContext.Request.Query[KnownHttpHeader.TenantId].FirstOrDefault();
        var userId = httpContext.Request.Headers[KnownHttpHeader.UserId].FirstOrDefault() ??
                     httpContext.Request.Query[KnownHttpHeader.UserId].FirstOrDefault();
        var storeId = httpContext.Request.Headers[KnownHttpHeader.StoreId].FirstOrDefault() ??
                      httpContext.Request.Query[KnownHttpHeader.StoreId].FirstOrDefault();
        var isStoreIdExists = Guid.TryParse(storeId, out var guidStoreId);
        guidStoreId = isStoreIdExists ? guidStoreId : Guid.Empty;

        if (tenantId is null)
        {
            if (environment is "Development")
            {
                // local ortamda geliştirenler token kullanmasın
                tenantId = "aec2b720-270a-47be-95f6-1c337d28c7e2";
            }
            else
            {
                throw new AuthenticationException("Tenant Id can't resolve from HttpContext!");
            }
        }

        if (userId is null)
        {
            if (environment is "Development")
            {
                // local ortamda geliştirenler token kullanmasın
                userId = "aec2b720-270a-47be-95f6-1c337d28c7e2";
            }
            else
            {
                throw new AuthenticationException("User Id can't resolve from HttpContext!");
            }
        }

        appContextBuilder
            .ConfigureCurrentTenant(tenantSettings => { tenantSettings.TenantId = Guid.Parse(tenantId); })
            .ConfigureCurrentUser(userSettings =>
            {
                userSettings.UserId = Guid.Parse(userId);
                userSettings.StoreId = guidStoreId;
                userSettings.CurrentCorrelationId = correlationId;
            })
            .ConfigureGlobalFilters(globalFilterSettings =>
            {
                globalFilterSettings.MultiTenancyFilter = true;
                globalFilterSettings.SoftDeleteFilter = true;
            })
            .Build();

        #region Telemetry Enrichers

        using var log = logger.PushProperties(new Dictionary<string, object?>
        {
            {nameof(KnownHttpHeader.TenantId), tenantId}, {nameof(KnownHttpHeader.UserId), userId}, {nameof(KnownHttpHeader.CorrelationId), correlationId},
        });

        Activity.Current.EnrichWithTenantId(tenantId);
        Activity.Current.EnrichWithUserId(userId);
        Activity.Current.EnrichWithCorrelationId(correlationId);

        #endregion Telemetry Enrichers

        await _next(httpContext);
    }

    private static StringValues GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(KnownHttpHeader.CorrelationId, out var correlationId))
        {
            return correlationId;
        }
        else
        {
            return Uuid.Next().ToString();
        }
    }
}
