using System;
using System.Linq;
using System.Reflection;
using Smartwyre.DeveloperTest.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddRebateCalculatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var serviceType = typeof(IRebateCalculator);
        var rebateCalculatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(serviceType))
            .ToList();

        foreach (var implementationType in rebateCalculatorTypes)
            services.AddScoped(implementationType);

        return services;
    }
}
