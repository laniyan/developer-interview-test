using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Extensions
{
    public static class CalculateFixedCashAmountRebateExtension
    {
        public static CalculateFixedCashAmountRebate AsCalculateFixedCashAmountRebateDto(this CalculateFixedCashAmountRebate _, Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return new CalculateFixedCashAmountRebate
            {
                Incentive = rebate.Incentive,
                SupportedIncentive = product.SupportedIncentives,
                Amount = rebate.Amount
            };
        }
    }
}