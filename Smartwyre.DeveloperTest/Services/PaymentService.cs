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
            if (string.IsNullOrWhiteSpace(request.DebtorAccountNumber))
            {
                throw new InvalidPaymentRequestException(nameof(MakePaymentRequest.DebtorAccountNumber));
            }

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                throw new AccountNotFoundException(request.DebtorAccountNumber);
            }

            var result = account.ExecuteDebit(request.PaymentScheme, request.Amount);
            if (result.Success)
            {
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
