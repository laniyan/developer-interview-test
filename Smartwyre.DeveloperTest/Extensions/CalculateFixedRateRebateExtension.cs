using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Extensions
{
    public static class CalculateFixedRateRebateExtension
    {
        public static CalculateFixedRateRebate AsCalculateFixedRateRebateDto(this CalculateFixedRateRebate _, Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return new CalculateFixedRateRebate
            {
                Incentive = rebate.Incentive,
                SupportedIncentive = product.SupportedIncentives,
                Volume = request.Volume,
                Percentage = rebate.Percentage,
                Price = product.Price
            };
        }
    }
}