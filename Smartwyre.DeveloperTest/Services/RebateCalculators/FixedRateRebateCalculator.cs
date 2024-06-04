using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators;

[IncentiveTypeCalculator(IncentiveType.FixedRateRebate)]
public class FixedRateRebateCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Percentage != 0 && product.Price != 0 && request.Volume != 0;

    public async Task<decimal> Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => await Task.FromResult(product.Price * rebate.Percentage * request.Volume);

}
