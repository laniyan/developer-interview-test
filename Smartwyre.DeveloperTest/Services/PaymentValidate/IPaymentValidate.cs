using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    public interface IPaymentValidate
    {
        MakePaymentResult IsValid();
    }
}
