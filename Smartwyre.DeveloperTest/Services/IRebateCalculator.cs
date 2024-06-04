using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;
public interface IRebateCalculator
{
    bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request);
    Task<decimal> Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}
