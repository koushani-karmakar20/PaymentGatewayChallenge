using System;
using System.Runtime.Serialization;

namespace BankStoreApi.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
        {
        }

        public CustomerNotFoundException(string message) : base(message)
        {
        }
    }
}