using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators;

[IncentiveTypeCalculator(IncentiveType.AmountPerUom)]
public class AmountPerUomCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => rebate.Amount == 0 || request.Volume == 0;

    public async Task<decimal> Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        => await Task.FromResult(rebate.Amount * request.Volume);
}
