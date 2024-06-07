using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Interfaces
{
    public interface IRebateCalculatorFactory
    {
        IRebateCalculator GetInstance(IncentiveType incentiveType);
    }
}