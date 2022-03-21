using Smartwyre.DeveloperTest.Services.PaymentValidate;

namespace Smartwyre.DeveloperTest.Types
{
    public class Account
    {
        private readonly IPaymentValidateFactory paymentValidateFactory;

        //Note to exlpain thinking:
        //made private set so IPaymentValidate can use Account properties, but Account still controls its data
        private string accountNumber;
        private decimal balance;
        private AccountStatus status;
        private AllowedPaymentSchemes allowedPaymentSchemes;

        public Account(
            string accountNumber, 
            decimal balance, 
            AccountStatus status, 
            AllowedPaymentSchemes allowedPaymentSchemes,
            IPaymentValidateFactory paymentValidateFactory)
        {
            this.paymentValidateFactory = paymentValidateFactory;

            this.accountNumber = accountNumber;
            this.balance = balance;
            this.status = status;
            this.allowedPaymentSchemes = allowedPaymentSchemes;
        }

        public string AccountNumber => accountNumber;
        public decimal Balance => balance;
        public AccountStatus Status => status;
        public AllowedPaymentSchemes AllowedPaymentSchemes => allowedPaymentSchemes;

        public MakePaymentResult ExecuteDebit(PaymentScheme paymentScheme, decimal debitAmount)
        {
            if (debitAmount < 0)
            {
                throw new InvalidPaymentRequestException(nameof(debitAmount));
            }

            var paymentValidate = paymentValidateFactory.Create(paymentScheme, this, debitAmount);
            if (paymentValidate.IsValid().Success)
            {
                balance -= debitAmount;
                return MakePaymentResult.CreateSuccess();
            }
            else
            {
                return MakePaymentResult.CreateFail();
            }
        }
    }
}
