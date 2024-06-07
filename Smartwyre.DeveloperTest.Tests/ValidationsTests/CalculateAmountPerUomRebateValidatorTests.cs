using FluentAssertions;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;
using Smartwyre.DeveloperTest.Validations;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ValidationsTests
{
    public class CalculateAmountPerUomRebateValidatorTests
    {
        private readonly CalculateAmountPerUomRebateValidator _validator = new CalculateAmountPerUomRebateValidator();

        [Fact]
        public void Validate_ShouldNotHaveValidationErrors_WhenValuesIsValid()
        {
            // Arrange
            var request = new CalculateAmountPerUomRebate
            {
                Amount = 10,
                Volume = 50,
                SupportedIncentive = SupportedIncentiveType.AmountPerUom,
                Incentive = IncentiveType.AmountPerUom
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Validate_ShouldHaveValidationErrors_WhenValuesIsNotValid(decimal invalidValue)
        {
            // Arrange
            var request = new CalculateAmountPerUomRebate
            {
                Amount = invalidValue,
                Volume = invalidValue,
                SupportedIncentive = SupportedIncentiveType.FixedRateRebate,
                Incentive = IncentiveType.AmountPerUom
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public void Validate_ShouldHaveValidationErrors_WhenRequestIsNull()
        {
            // Arrange
            CalculateAmountPerUomRebate request = new();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
        }
    }
}