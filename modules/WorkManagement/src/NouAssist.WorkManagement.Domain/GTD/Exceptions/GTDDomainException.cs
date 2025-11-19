using System;
using Volo.Abp;

namespace NouAssist.WorkManagement.GTD.Exceptions
{
    public class GTDDomainException : BusinessException
    {
        public GTDDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static GTDDomainException InvalidGTDStatus()
        {
            return new GTDDomainException(
                "GTD:InvalidStatus",
                "Cannot perform this operation in the current GTD item status.");
        }

        public static GTDDomainException InvalidProcessingDecision()
        {
            return new GTDDomainException(
                "GTD:InvalidProcessingDecision",
                "Invalid GTD processing decision.");
        }

        public static GTDDomainException ProjectAlreadyCompleted()
        {
            return new GTDDomainException(
                "GTD:ProjectAlreadyCompleted",
                "Cannot modify a completed GTD project.");
        }

        public static GTDDomainException InvalidContextAssignment()
        {
            return new GTDDomainException(
                "GTD:InvalidContextAssignment",
                "Invalid context assignment for GTD item.");
        }
    }
}
