namespace Smartwyre.DeveloperTest.Types.Dtos
{
    public class CalculateFixedRateRebate : CalculateIncentiveTypeRebateBase
    {
        public decimal Price { get; set; }

        public decimal Percentage { get; set; }

        public decimal Volume { get; set; }
    }
}