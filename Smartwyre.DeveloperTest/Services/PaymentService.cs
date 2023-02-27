using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            this.accountDataStore = accountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = CheckIfRequestIsValid(account, request);       

            if (result.Success)
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);

                result.Message = $"New balance: {account.Balance}";
            }

            return result;
        }

        private MakePaymentResult CheckIfRequestIsValid(Account account, MakePaymentRequest request)
        {
            var result = new MakePaymentResult();
            if (account == null)
            {
                result.Message = $"Account '{request.DebtorAccountNumber}' not found";
            }
            else if (!PaymentSchemeAllowed(account, request))
            {
                result.Message = $"Payment scheme '{request.PaymentScheme}' not allowed by account";
            }
            else if (account.Balance < request.Amount)
            {
                result.Message = $"Payment amount '{request.Amount}' is greater than balance '{account.Balance}'";
            }
            else if (account.Status != AccountStatus.Live)
            {
                result.Message = $"Account '{request.DebtorAccountNumber}' is not live";
            }
            else
            {
                result.Success = true;
            }
            return result;
        }

        private bool PaymentSchemeAllowed(Account account, MakePaymentRequest request)
        {
            var requestedPaymentScheme = MapPaymentScheme(request.PaymentScheme);

            return account.AllowedPaymentSchemes.HasFlag(requestedPaymentScheme);
        }

        private AllowedPaymentSchemes MapPaymentScheme(PaymentScheme paymentScheme)
        {
            return (AllowedPaymentSchemes)(1 << (int)paymentScheme);
        }
    }
}
