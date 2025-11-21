using System;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Exception thrown when a test case is not found
    /// </summary>
    public class TestCaseNotFoundException : ValidationVerificationDomainException
    {
        public TestCaseNotFoundException(Guid testCaseId)
            : base("ValidationVerification:ErrorMessage:TestCaseNotFound")
        {
            WithData("testCaseId", testCaseId);
        }
    }
}
