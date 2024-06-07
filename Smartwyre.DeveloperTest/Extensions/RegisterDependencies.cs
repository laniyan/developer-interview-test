using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;

namespace Smartwyre.DeveloperTest.Extensions
{
    public static class RegisterDependencies
    {
        public static IServiceCollection AddDeveloperTest(this IServiceCollection services)
        {
            services.AddScoped<IRebateDataStore, RebateDataStore>();
            services.AddScoped<IProductDataStore, ProductDataStore>();
            services.AddScoped<IRebateCalculator, FixedCashAmountCalculator>();
            services.AddScoped<IRebateCalculator, FixedRateRebateCalculator>();
            services.AddScoped<IRebateCalculator, AmountPerUomCalculator>();
            services.AddScoped<IRebateService, RebateService>();
            services.AddScoped<ICacheWrapper, CacheWrapper>();
            services.AddScoped<IRebateCalculatorFactory, RebateCalculatorFactory>();
            services.AddMemoryCache();
            services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();
            return services;
        }

    }
}