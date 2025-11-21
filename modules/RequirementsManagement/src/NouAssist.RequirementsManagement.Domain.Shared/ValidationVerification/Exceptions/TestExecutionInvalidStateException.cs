using System;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to execute a test in an invalid state
    /// </summary>
    public class TestExecutionInvalidStateException : ValidationVerificationDomainException
    {
        public TestExecutionInvalidStateException(Guid testCaseId, TestCaseStatus currentStatus)
            : base("ValidationVerification:ErrorMessage:TestExecutionInvalidState")
        {
            WithData("testCaseId", testCaseId);
            WithData("currentStatus", currentStatus);
        }
    }
}
