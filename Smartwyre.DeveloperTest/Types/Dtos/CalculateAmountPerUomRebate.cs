namespace Smartwyre.DeveloperTest.Types.Dtos
{
    public class CalculateAmountPerUomRebate : CalculateIncentiveTypeRebateBase
    {
        public decimal Volume { get; set; }

        public decimal Amount { get; set; }
    }
}