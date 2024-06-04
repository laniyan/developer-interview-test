using System;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateCalculatorFactory(
    IServiceProvider serviceProvider,
    RebateCalculatorTypeCollection calculators
    ) : IRebateCalculatorFactory
{

    private readonly IServiceProvider serviceProvider = serviceProvider;

    public IRebateCalculator Create(IncentiveType incentiveType)
    {
        var calc = calculators.GetFor(incentiveType)
            ?? throw new Exception($"No rebate calculator was found for incentive type {incentiveType}.");

        return serviceProvider.GetService(calc) as IRebateCalculator;
    }
}
