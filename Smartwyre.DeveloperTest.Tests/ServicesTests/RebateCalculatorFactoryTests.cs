using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ServicesTests
{
    public class RebateCalculatorFactoryTests
    {
        private readonly RebateCalculatorFactory _factory;
        private readonly IEnumerable<IRebateCalculator> _rebateCalculators;
        private readonly IRebateCalculator _fixedCashAmountCalculator;
        private readonly IRebateCalculator _fixedRateRebateCalculator;
        private readonly IRebateCalculator _amountPerUomCalculator;
        public RebateCalculatorFactoryTests()
        {
            _fixedCashAmountCalculator = new FixedCashAmountCalculator(new Mock<CalculateFixedCashAmountRebateValidator>().Object, new Mock<ILogger<FixedCashAmountCalculator>>().Object);
            _fixedRateRebateCalculator = new FixedRateRebateCalculator(new Mock<CalculateFixedRateRebateValidator>().Object, new Mock<ILogger<FixedRateRebateCalculator>>().Object);
            _amountPerUomCalculator = new AmountPerUomCalculator(new Mock<CalculateAmountPerUomRebateValidator>().Object, new Mock<ILogger<AmountPerUomCalculator>>().Object);

            _rebateCalculators = new List<IRebateCalculator>()
            {
                _fixedCashAmountCalculator,
                _fixedRateRebateCalculator,
                _amountPerUomCalculator
            };

            _factory = new RebateCalculatorFactory(_rebateCalculators);
        }

        [Theory]
        [InlineData(IncentiveType.FixedRateRebate)]
        [InlineData(IncentiveType.AmountPerUom)]
        [InlineData(IncentiveType.FixedCashAmount)]
        public void GetInstance_ShouldReturnCorrectCalculator(IncentiveType incentive)
        {
            switch (incentive)
            {
                case IncentiveType.FixedRateRebate:
                    _fixedRateRebateCalculator.Should().Be(_factory.GetInstance(incentive));
                    break;
                case IncentiveType.AmountPerUom:
                    _amountPerUomCalculator.Should().Be(_factory.GetInstance(incentive));
                    break;
                case IncentiveType.FixedCashAmount:
                    _fixedCashAmountCalculator.Should().Be(_factory.GetInstance(incentive));
                    break;
            }
        }

        [Fact]
        public void GetInstance_ShouldThrowExceptionForUnknownIncentiveType()
        {
            // Assert
            Assert.Throws<InvalidOperationException>(() => _factory.GetInstance((IncentiveType)999));
        }
    }
}