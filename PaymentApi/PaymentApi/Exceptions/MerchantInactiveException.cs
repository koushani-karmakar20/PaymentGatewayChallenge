using System;
using System.Runtime.Serialization;

namespace PaymentApi.Exceptions
{
    public class MerchantInactiveException : Exception
    {
        public  MerchantInactiveException()
        {
        }

        public  MerchantInactiveException(string message) : base(message)
        {
        }
    }
}