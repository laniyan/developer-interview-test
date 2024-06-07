using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services
{
    public class RebateCalculatorFactory : IRebateCalculatorFactory
    {
        private readonly IEnumerable<IRebateCalculator> _rebateCalculators;

        public RebateCalculatorFactory(IEnumerable<IRebateCalculator> rebateCalculators)
        {
            _rebateCalculators = rebateCalculators;
        }

        public IRebateCalculator GetInstance(IncentiveType incentiveType)
        {
            return incentiveType switch
            {
                IncentiveType.FixedRateRebate =>
                    this.GetService(typeof(FixedRateRebateCalculator)),
                IncentiveType.AmountPerUom =>
                   this.GetService(typeof(AmountPerUomCalculator)),
                IncentiveType.FixedCashAmount =>
                   this.GetService(typeof(FixedCashAmountCalculator)),
                _ => throw new InvalidOperationException()
            };
        }

        private IRebateCalculator GetService(Type type)
        {
            return _rebateCalculators.FirstOrDefault(x => x.GetType() == type);
        }
    }
}