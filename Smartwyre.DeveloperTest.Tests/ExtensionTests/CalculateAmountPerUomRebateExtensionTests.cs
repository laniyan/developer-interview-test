using FluentAssertions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ExtensionTests
{
    public class CalculateAmountPerUomRebateExtensionTests
    {
        [Fact]
        public void AsCalculateAmountPerUomRebateDto_ShouldReturnCorrectDto()
        {
            // Arrange
            var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 10 };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
            var request = new CalculateRebateRequest { Volume = 5 };
            var calculateAmountPerUomRebate = new CalculateAmountPerUomRebate();

            // Act
            calculateAmountPerUomRebate = CalculateAmountPerUomRebateExtension.AsCalculateAmountPerUomRebateDto(calculateAmountPerUomRebate, rebate, product, request);

            // Assert
            rebate.Incentive.Should().Be(calculateAmountPerUomRebate.Incentive);
            product.SupportedIncentives.Should().Be(calculateAmountPerUomRebate.SupportedIncentive);
            request.Volume.Should().Be(calculateAmountPerUomRebate.Volume);
            rebate.Amount.Should().Be(calculateAmountPerUomRebate.Amount);

            (calculateAmountPerUomRebate.Volume > 0).Should().BeTrue();
            (calculateAmountPerUomRebate.Amount > 0).Should().BeTrue();
        }
    }
}