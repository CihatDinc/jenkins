using System.Data;
using System.Net.Mime;
using System.Reflection;
using System.Security.Authentication;
using Application;
using FluentValidation;
using HealthChecks.UI.Client;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Mapper;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Comm.Customer.Domain.Contracts;
using Nebim.Era.Plt.Context;
using Nebim.Era.Plt.Core;
using Nebim.Era.Plt.Core.Events;
using Nebim.Era.Plt.Core.PlatformConfigs;
using Nebim.Era.Plt.Core.Serialization.Json;
using Nebim.Era.Plt.Data.EntityFramework;
using Nebim.Era.Plt.Data.Mysql;
using Nebim.Era.Plt.EventBus;
using Nebim.Era.Plt.EventBus.Dispatchers.Mediatr;
using Nebim.Era.Plt.EventBus.Transports.Kafka;
using Nebim.Era.Plt.OpenTelemetry;
using Serilog;
using Serilog.Exceptions;
using Service;
using Service.Mediator;
using Service.Middlewares;
using Service.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", false, true)
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithExceptionDetails()
    .CreateLogger();

Log.Logger = logger;

logger?.Information("Era Customer service starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    if (!builder.Environment.IsDevelopment())
    {
        ConfigureAwsSecretManager(builder.Configuration);
    }

    builder.AddTelemetry(ApplicationSettings.ServiceName,ApplicationSettings.ServiceVersion,Environment.MachineName);
    builder.Services.Configure<PlatformServicesSettings>(builder.Configuration.GetSection(PlatformServicesSettings.SectionName));
    builder.Services.Configure<JsonOptions>(ConfigureJsonOptions);
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
    builder.Services.AddSystemClock();

    builder.Services.AddPlatformContexts();
    builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);
    builder.Services.AddHeaderPropagation(option => option.Headers.Add(KnownHttpHeader.CorrelationId));
    builder.Services.AddData(builder.Configuration);

    builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssembly>();
    builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateCustomerValidator)));
    builder.Services.AddAutoMapper(typeof(CustomerMapperProfile));
    builder.Services.AddMediatR(mediatrConfig =>
    {
        mediatrConfig.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(CreateCustomerHandler))!, Assembly.GetAssembly(typeof(CreateCustomerRequest))!)
            .AddOpenBehavior(typeof(ValidationMiddleware<,>))
            .AddOpenBehavior(typeof(LoggingMiddleware<,>));
    });

    builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.SectionName));
    builder.Services
        .AddBus()
        .AddProducer<ChangedDataIntegrationEvent<CustomerEto>>(ApplicationSettings.ChangeDataCaptureTopicName)
        .UseMassTransitKafkaIntegration()
        .AddMediatrEventDispatcher();

    builder.Services.AddScoped<IPassportMapper, PassportMapper>();
    builder.Services.AddScoped<IPassportRepository, PassportRepository>();
    builder.Services.AddScoped<ICustomerMapper, CustomerMapper>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddEraDbContext<CustomerDbContext>(builder.Environment);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(ConfigureSwaggerGenOptions);
    builder.Services.AddFluentValidationRulesToSwagger();
    builder.Services.AddHealthChecks().AddMySql(builder.Configuration.GetRequiredSection("MySql:ReadWriteConnectionString").Get<string>() ?? throw new NoNullAllowedException());

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseSwagger();
    app.UseSwaggerUI(ConfigureSwaggerUiOptions);
    app.UseExceptionHandler(ConfigureExceptionHandler);
    app.UseCors(ConfigureCorsPolicyBuilder);
    app.UseMiddleware<AppContextResolutionMiddleware>();
    app.UseHeaderPropagation();
    // app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpLogging();
    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger?.Error(exception, "Era Customer service stopped because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    Log.CloseAndFlush();
}

void ConfigureAwsSecretManager(IConfigurationBuilder configurationManager)
{
    configurationManager.AddSecretsManager(configurator: options =>
    {
        options.PollingInterval = TimeSpan.FromHours(1);
        options.SecretFilter = entry => entry.Name.StartsWith(ApplicationSettings.AppSecretPrefix,StringComparison.InvariantCultureIgnoreCase) || entry.Name.StartsWith(ApplicationSettings.AppSecretCommonPrefix,StringComparison.InvariantCultureIgnoreCase);
        options.KeyGenerator = (_, key) => key.Substring(key.IndexOf(':') + 1);
    });
}

void ConfigureJsonOptions(JsonOptions options)
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonSerde.DefaultOptions.PropertyNameCaseInsensitive;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonSerde.DefaultOptions.PropertyNamingPolicy;
    options.JsonSerializerOptions.NumberHandling = JsonSerde.DefaultOptions.NumberHandling;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonSerde.DefaultOptions.DefaultIgnoreCondition;
    foreach (var converter in JsonSerde.DefaultOptions.Converters)
    {
        options.JsonSerializerOptions.Converters.Add(converter);
    }
}

void ConfigureCorsPolicyBuilder(CorsPolicyBuilder corsPolicyBuilder)
{
    corsPolicyBuilder.AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true);
}

void ConfigureSwaggerUiOptions(SwaggerUIOptions c)
{
    // open swagger ui as a welcome page
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Era API");
    c.RoutePrefix = "swagger";
}

void ConfigureExceptionHandler(IApplicationBuilder applicationBuilder)
{
    applicationBuilder.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        context.Response.StatusCode = exceptionHandlerPathFeature?.Error switch
        {
            EraNotFoundException => StatusCodes.Status404NotFound,
            AuthenticationException => StatusCodes.Status401Unauthorized,
            EraValidationException => StatusCodes.Status400BadRequest,
            EraUnprocessableException => StatusCodes.Status422UnprocessableEntity,
            EraConflictException => StatusCodes.Status409Conflict,
            EraBusinessException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.Headers.Add(context.Request.Headers.FirstOrDefault(t => t.Key == KnownHttpHeader.CorrelationId));
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var error = ErrorDetails.CreateFrom(exceptionHandlerPathFeature?.Error!);
        await context.Response.WriteAsync(JsonSerde.Serialize(error));
    });
}

void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
{
    var introductionPath = Path.Combine(AppContext.BaseDirectory, "OpenApi", "Docs", "Introduction.md");
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = ApplicationSettings.ServiceVersion,
        Title = "Era Customer API",
        Description = File.Exists(introductionPath) ? File.ReadAllText(introductionPath) : "Era Unified Commerce Customer API",

        Contact = new OpenApiContact
        {
            Name = "Support",
            Url = new Uri("https://www.eradev.cloud/support")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://www.eradev.cloud/license")
        }
    });

    options.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
    {
        Description = "Please enter a valid token without a prefix.",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT"
    });

    options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();

    options.EnableAnnotations();

    var appContractXmlFilePath = GetPath(Assembly.GetAssembly(typeof(ApplicationContractsAssembly))!.GetName().Name);
    var xmlFilePath = GetPath(Assembly.GetExecutingAssembly().GetName().Name);

    options.IncludeXmlComments(xmlFilePath, true);
    options.IncludeXmlComments(appContractXmlFilePath, true);

    options.AddEnumsWithValuesFixFilters(o =>
    {
        // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' or its alias from XEnumDescriptionsAlias for schema extensions) for applied filters
        o.IncludeDescriptions = true;

        // add remarks for descriptions from xml-comments
        o.IncludeXEnumRemarks = true;

        // get descriptions from DescriptionAttribute then from xml-comments
        o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;

        // get descriptions from xml-file comments on the specified path
        // should use "options.IncludeXmlComments(xmlFilePath);" before
        o.IncludeXmlCommentsFrom(xmlFilePath);
        // the same for another xml-files...
        o.IncludeXmlCommentsFrom(appContractXmlFilePath);
    });
    //for enum examples
    options.UseAllOfToExtendReferenceSchemas();

    string GetPath(string? assemblyName)
    {
        return Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
    }
}
