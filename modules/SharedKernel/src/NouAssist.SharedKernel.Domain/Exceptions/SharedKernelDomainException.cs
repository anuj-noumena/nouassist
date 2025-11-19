using System;
using Volo.Abp;

namespace NouAssist.SharedKernel.Exceptions
{
    public class SharedKernelDomainException : BusinessException
    {
        public SharedKernelDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static SharedKernelDomainException InvalidImportanceScore(double value)
        {
            return new SharedKernelDomainException(
                "SharedKernel:InvalidImportanceScore",
                $"Importance score {value} is out of valid range (0.0-1.0).");
        }

        public static SharedKernelDomainException InvalidUrgencyScore(double value)
        {
            return new SharedKernelDomainException(
                "SharedKernel:InvalidUrgencyScore",
                $"Urgency score {value} is out of valid range (0.0-1.0).");
        }

        public static SharedKernelDomainException InvalidEmailFormat(string email)
        {
            return new SharedKernelDomainException(
                "SharedKernel:InvalidEmailFormat",
                $"Email address '{email}' is not in a valid format.");
        }

        public static SharedKernelDomainException InvalidRecurrenceRule()
        {
            return new SharedKernelDomainException(
                "SharedKernel:InvalidRecurrenceRule",
                "Invalid recurrence rule configuration.");
        }

        public static SharedKernelDomainException TagAlreadyExists(string tagName)
        {
            return new SharedKernelDomainException(
                "SharedKernel:TagAlreadyExists",
                $"Tag '{tagName}' already exists.");
        }
    }
}
