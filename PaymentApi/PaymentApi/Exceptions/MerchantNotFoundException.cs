using System;
using System.Runtime.Serialization;

namespace PaymentApi.Exceptions
{
    public class MerchantNotFoundException : Exception
    {
        public MerchantNotFoundException()
        {
        }

        public MerchantNotFoundException(string message) : base(message)
        {
        }
    }
}