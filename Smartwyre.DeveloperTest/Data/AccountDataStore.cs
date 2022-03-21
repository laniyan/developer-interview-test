using Smartwyre.DeveloperTest.Services.PaymentValidate;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        private readonly IPaymentValidateFactory paymentValidateFactory;

        public AccountDataStore(IPaymentValidateFactory paymentValidateFactory)
        {
            this.paymentValidateFactory = paymentValidateFactory;
        }

        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            var allowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments
                                        | AllowedPaymentSchemes.BankToBankTransfer
                                        | AllowedPaymentSchemes.AutomatedPaymentSystem;
            return new Account(accountNumber, 100.00M, AccountStatus.Live, allowedPaymentSchemes, paymentValidateFactory);
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
