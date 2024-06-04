using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;
public interface IRebateCalculatorFactory
{
    IRebateCalculator Create(IncentiveType incentiveType);
}