using FluentAssertions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ExtensionTests
{
    public class CalculateFixedCashAmountRebateExtensionTests
    {
        [Fact]
        public void AsCalculateFixedCashAmountRebateDto_ShouldReturnCorrectDto()
        {
            // Arrange
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 10 };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
            var request = new CalculateRebateRequest();
            var calculateFixedCashAmountRebate = new CalculateFixedCashAmountRebate();

            // Act
            var result = CalculateFixedCashAmountRebateExtension.AsCalculateFixedCashAmountRebateDto(calculateFixedCashAmountRebate, rebate, product, request);

            // Assert
            rebate.Incentive.Should().Be(result.Incentive);
            product.SupportedIncentives.Should().Be(result.SupportedIncentive);
            rebate.Amount.Should().Be(result.Amount);

            (result.Amount > 0).Should().BeTrue();
        }
    }
}