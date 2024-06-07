using FluentAssertions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ExtensionTests
{
    public class CalculateFixedRateRebateExtensionTests
    {
        [Fact]
        public void AsCalculateFixedRateRebateDto_ShouldReturnCorrectDto()
        {
            // Arrange
            var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 };
            var request = new CalculateRebateRequest { Volume = 5 };
            var calculateFixedRateRebate = new CalculateFixedRateRebate();

            // Act
            var result = CalculateFixedRateRebateExtension.AsCalculateFixedRateRebateDto(calculateFixedRateRebate, rebate, product, request);

            // Assert
            rebate.Incentive.Should().Be(result.Incentive);
            product.SupportedIncentives.Should().Be(result.SupportedIncentive);
            request.Volume.Should().Be(result.Volume);
            rebate.Percentage.Should().Be(result.Percentage);
            product.Price.Should().Be(result.Price);

            (result.Volume > 0).Should().BeTrue();
            (result.Percentage > 0).Should().BeTrue();
            (result.Price > 0).Should().BeTrue();
        }
    }

}