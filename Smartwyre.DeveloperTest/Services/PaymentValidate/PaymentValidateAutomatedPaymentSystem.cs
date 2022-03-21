using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    internal class PaymentValidateAutomatedPaymentSystem : IPaymentValidate
    {
        private readonly Account account;

        public PaymentValidateAutomatedPaymentSystem(Account account)
        {
            this.account = account;
        }

        public MakePaymentResult IsValid()
        {
            if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem)
                && account.Status == AccountStatus.Live)
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
