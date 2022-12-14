using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult
            {
                Success = false
            };

            switch (request.PaymentScheme)
            {
                case PaymentScheme.BankToBankTransfer:
                    result.Success = RequestValidator(account, AllowedPaymentSchemes.BankToBankTransfer);
                    break;

                case PaymentScheme.ExpeditedPayments:
                    result.Success = RequestValidator(account, AllowedPaymentSchemes.ExpeditedPayments) && account.Balance >= request.Amount;
                    break;

                case PaymentScheme.AutomatedPaymentSystem:
                    result.Success = RequestValidator(account, AllowedPaymentSchemes.AutomatedPaymentSystem) && account.Status == AccountStatus.Live;
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                var accountDataStoreUpdateData = new AccountDataStore();
                accountDataStoreUpdateData.UpdateAccount(account);
            }

            return result;
        }

        private static bool RequestValidator(Account account, AllowedPaymentSchemes paymentScheme)
        {
            return account != null & account.AllowedPaymentSchemes.HasFlag(paymentScheme);        
        }
    }
}
