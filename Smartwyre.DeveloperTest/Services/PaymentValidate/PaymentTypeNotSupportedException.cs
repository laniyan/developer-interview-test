using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    public class PaymentTypeNotSupportedException : Exception
    {
        private readonly PaymentScheme paymentScheme;

        public PaymentTypeNotSupportedException(PaymentScheme paymentScheme) : base()
        {
            this.paymentScheme = paymentScheme;
        }

        public PaymentScheme PaymentScheme => paymentScheme;
    }
}
