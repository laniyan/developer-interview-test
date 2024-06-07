using FluentValidation;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Validations
{
    public class CalculateAmountPerUomRebateValidator : AbstractValidator<CalculateAmountPerUomRebate>
    {
        public CalculateAmountPerUomRebateValidator()
        {
            RuleFor(x => x.Volume).GreaterThan(0).WithMessage(x => $"The Volume value must be greater than zero. Current value: {x.Volume}");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage(x => $"The Amount value must be greater than zero. Current value: {x.Amount}");
            RuleFor(x => x.SupportedIncentive).Equal(SupportedIncentiveType.AmountPerUom)
                .WithMessage(x => $"'Supported Incentive' must be 'AmountPerUom' for a AmountPerUom calculation. Current value: {x.SupportedIncentive}");
            RuleFor(x => x.Incentive).Equal(IncentiveType.AmountPerUom)
                .WithMessage(x => $"'Incentive Type' must be 'AmountPerUom' for a AmountPerUom calculation. Current value: {x.Incentive}");
        }
    }
}