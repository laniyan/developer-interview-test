using System.Reflection;
using FluentValidation;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.RebateCalculators;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    static readonly Assembly currentAssembly = Assembly.GetExecutingAssembly();

    public static IServiceCollection AddValidation(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(currentAssembly, ServiceLifetime.Transient);

    public static IServiceCollection AddDataStore(this IServiceCollection services)
    {
        services.AddScoped<IRebateDataStore, RebateDataStore>();
        services.AddScoped<IProductDataStore, ProductDataStore>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRebateCalculators();
        services.AddSingleton<RebateCalculatorTypeCollection>();
        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();

        services.AddScoped<IRebateService, RebateService>();
        return services;
    }

    public static IServiceCollection AddRebateCalculators(this IServiceCollection services)
        => services.AddRebateCalculatorsFromAssembly(currentAssembly);
}
