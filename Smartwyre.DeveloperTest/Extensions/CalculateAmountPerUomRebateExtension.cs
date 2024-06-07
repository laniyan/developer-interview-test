using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Extensions
{
    public static class CalculateAmountPerUomRebateExtension
    {
        public static CalculateAmountPerUomRebate AsCalculateAmountPerUomRebateDto(this CalculateAmountPerUomRebate _, Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return new CalculateAmountPerUomRebate
            {
                Incentive = rebate.Incentive,
                SupportedIncentive = product.SupportedIncentives,
                Volume = request.Volume,
                Amount = rebate.Amount
            };
        }
    }
}