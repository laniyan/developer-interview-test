using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    internal class PaymentValidateExpeditedPayments : IPaymentValidate
    {
        private readonly Account account;
        private readonly decimal debitAmount;

        public PaymentValidateExpeditedPayments(Account account, decimal debitAmount)
        {
            this.account = account;
            this.debitAmount = debitAmount;
        }

        public MakePaymentResult IsValid()
        {
            if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.ExpeditedPayments)
                && account.Balance >= debitAmount)
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
