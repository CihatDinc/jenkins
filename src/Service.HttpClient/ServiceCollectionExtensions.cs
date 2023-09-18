using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nebim.Era.Plt.Comm.Customer.HttpClient;
using Nebim.Era.Plt.Core.PlatformConfigs;
using Refit;

namespace Nebim.Era.Plt.Comm.Customer.Service.HttpClient;

using System.Text.Json;
using Core.Serialization.Json;
using HttpClient = System.Net.Http.HttpClient;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Era Customer Service client implementations to IoC container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="clientConfig"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHttpClientBuilder AddEraCustomerServiceClients(this IServiceCollection services, Action<HttpClient>? clientConfig = null)
    {
        var platformSvcSettings =
            services.BuildServiceProvider().GetRequiredService<IOptions<PlatformServicesSettings>>();

        if (string.IsNullOrWhiteSpace(platformSvcSettings.Value.CustomerServiceAddress))
            throw new ArgumentNullException(nameof(PlatformServicesSettings.CustomerServiceAddress));

        return services.AddEraCustomerApiClientsInternal(cfg =>
        {
            cfg.BaseAddress = new Uri(platformSvcSettings.Value.CustomerServiceAddress);
            clientConfig?.Invoke(cfg);
        });
    }

    private static IHttpClientBuilder AddEraCustomerApiClientsInternal(this IServiceCollection services, Action<HttpClient> config)
    {
        services.AddTransient<EraPlatformHeaderPropagationHandler>();

        var refitSettings = CreateRefitSettings();
        services.AddRefitClient<ICustomersApi>(refitSettings)
            .ConfigureHttpClient(config)
            .AddHttpMessageHandler<EraPlatformHeaderPropagationHandler>();

        return services.AddRefitClient<IPassportsApi>(refitSettings)
            .ConfigureHttpClient(config)
            .AddHttpMessageHandler<EraPlatformHeaderPropagationHandler>();
    }

    private static RefitSettings CreateRefitSettings()
    {
        var refitSettings = new RefitSettings();
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = JsonSerde.DefaultOptions.PropertyNameCaseInsensitive,
            PropertyNamingPolicy = JsonSerde.DefaultOptions.PropertyNamingPolicy,
            NumberHandling = JsonSerde.DefaultOptions.NumberHandling,
            DefaultIgnoreCondition = JsonSerde.DefaultOptions.DefaultIgnoreCondition
        };

        foreach (var converter in JsonSerde.DefaultOptions.Converters)
        {
            serializerOptions.Converters.Add(converter);
        }

        serializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        refitSettings.ContentSerializer = new SystemTextJsonContentSerializer(serializerOptions);

        return refitSettings;
    }
}
