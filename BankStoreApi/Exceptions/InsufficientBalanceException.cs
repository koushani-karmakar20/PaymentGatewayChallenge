using System;
using System.Runtime.Serialization;

namespace BankStoreApi.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException()
        {
        }

        public InsufficientBalanceException(string message) : base(message)
        {
        }
    }
}