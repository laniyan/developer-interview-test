using FluentValidation;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Validations
{
    public class CalculateFixedRateRebateValidator : AbstractValidator<CalculateFixedRateRebate>
    {
        public CalculateFixedRateRebateValidator()
        {
            RuleFor(x => x.Percentage).GreaterThan(0).WithMessage(x => $"The percentage value must be greater than zero. Current value: {x.Percentage}");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage(x => $"The Price value must be greater than zero. Current value: {x.Price}");
            RuleFor(x => x.Volume).GreaterThan(0).WithMessage(x => $"The Volume value must be greater than zero. Current value: {x.Volume}");
            RuleFor(x => x.SupportedIncentive).Equal(SupportedIncentiveType.FixedRateRebate)
                .WithMessage(x => $"'Supported Incentive' must be 'FixedRateRebate' for a FixedRateRebate calculation. Current value: {x.SupportedIncentive}");
            RuleFor(x => x.Incentive).Equal(IncentiveType.FixedRateRebate)
                .WithMessage(x => $"'Incentive Type' must be 'FixedRateRebate' for a FixedRateRebate calculation. Current value: {x.Incentive}");
        }
    }
}