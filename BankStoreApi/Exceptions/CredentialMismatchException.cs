using System;
using System.Runtime.Serialization;

namespace BankStoreApi.Exceptions
{
    public class CredentialMismatchException : Exception
    {
        public CredentialMismatchException()
        {
        }

        public CredentialMismatchException(string message) : base(message)
        {
        }
    }
}