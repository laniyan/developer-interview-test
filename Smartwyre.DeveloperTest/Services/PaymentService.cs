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
            
            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.BankToBankTransfer:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.BankToBankTransfer))
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.ExpeditedPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.ExpeditedPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.AutomatedPaymentSystem:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem))
                    {
                        result.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
