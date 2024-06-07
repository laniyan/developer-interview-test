using FluentAssertions;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;
using Smartwyre.DeveloperTest.Validations;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ValidationsTests
{
    public class CalculateFixedRateRebateValidatorTests
    {
        private readonly CalculateFixedRateRebateValidator _validator = new CalculateFixedRateRebateValidator();

        [Fact]
        public void Validate_ShouldNotHaveValidationErrors_WhenValuesIsValid()
        {
            // Arrange
            var request = new CalculateFixedRateRebate
            {
                Percentage = 10,
                Price = 100,
                Volume = 50,
                SupportedIncentive = SupportedIncentiveType.FixedRateRebate,
                Incentive = IncentiveType.FixedRateRebate
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
            var request = new CalculateFixedRateRebate
            {
                Percentage = invalidValue,
                Price = invalidValue,
                Volume = invalidValue,
                SupportedIncentive = SupportedIncentiveType.FixedRateRebate,
                Incentive = IncentiveType.FixedRateRebate
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
            CalculateFixedRateRebate request = new();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
        }
    }
}