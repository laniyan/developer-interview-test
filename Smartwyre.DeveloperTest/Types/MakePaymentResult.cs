namespace Smartwyre.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        private MakePaymentResult()
        {
            // construct through static methods
        }

        public static MakePaymentResult CreateSuccess() => new () { Success = true };
        public static MakePaymentResult CreateFail() => new () { Success = false };

        public bool Success { get; set; }
    }
}
