using FluentValidation;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Validations
{
    public class CalculateFixedCashAmountRebateValidator : AbstractValidator<CalculateFixedCashAmountRebate>
    {
        public CalculateFixedCashAmountRebateValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage(x => $"The Amount value must be greater than zero. Current value: {x.Amount}");
            RuleFor(x => x.SupportedIncentive).Equal(SupportedIncentiveType.FixedCashAmount)
                .WithMessage(x => $"'Supported Incentive' must be 'FixedCashAmount' for a FixedCashAmount calculation. Current value: {x.SupportedIncentive}");
            RuleFor(x => x.Incentive).Equal(IncentiveType.FixedCashAmount)
                .WithMessage(x => $"'Incentive Type' must be 'FixedCashAmount' for a FixedCashAmount calculation. Current value: {x.Incentive}");
        }
    }
}