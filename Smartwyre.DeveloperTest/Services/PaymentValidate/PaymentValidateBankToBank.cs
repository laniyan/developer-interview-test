using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    internal class PaymentValidateBankToBank : IPaymentValidate
    {
        private readonly Account account;

        public PaymentValidateBankToBank(Account account)
        {
            this.account = account;
        }

        public MakePaymentResult IsValid()
        {
            if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.BankToBankTransfer))
            {
                return MakePaymentResult.CreateSuccess();
            }
            else
            {
                return MakePaymentResult.CreateFail();
            }
        }
    }
}
