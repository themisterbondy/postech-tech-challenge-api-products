using System.Reflection;
using FluentValidation;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Postech.Fiap.Products.WebApi.Common;
using Postech.Fiap.Products.WebApi.Common.Behavior;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;

namespace Postech.Fiap.Products.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    private static readonly Assembly Assembly = typeof(Program).Assembly;

    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddMediatRConfiguration();
        services.AddSwaggerConfiguration();
        services.AddOpenTelemetryConfiguration();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddUseHealthChecksConfiguration(configuration);
        services.AddValidatorsFromAssembly(Assembly);

        services.AddProblemDetails();
        services.AddCarter();

        //Example dependency injection
        //services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }

    private static void AddMediatRConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }

    private static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = "PosTech MyFood API - Products",
                Version = "v1",
                Title = "PosTech MyFood API - Products"
            });
        });
    }

    private static void AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        var serviceName = $"{Assembly.GetName().Name}-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => resourceBuilder.AddService(serviceName!))
            .WithTracing(tracing =>
            {
                tracing.AddSource(serviceName!);
                tracing.ConfigureResource(resource => resource
                    .AddService(serviceName)
                    .AddTelemetrySdk());
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                tracing.AddOtlpExporter();
            });
    }

    public static void AddSerilogConfiguration(this IServiceCollection services,
        WebApplicationBuilder builder, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var applicationName =
            $"{Assembly.GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}";

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);
    }
}