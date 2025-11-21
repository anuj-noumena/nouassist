using System;

namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when a test cycle cannot be started
    /// </summary>
    public class TestCycleCannotBeStartedException : QADomainException
    {
        /// <summary>
        /// The ID of the test cycle
        /// </summary>
        public Guid TestCycleId { get; }
        
        /// <summary>
        /// The reason why the test cycle cannot be started
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Creates a new instance of TestCycleCannotBeStartedException
        /// </summary>
        public TestCycleCannotBeStartedException(Guid testCycleId, string reason)
            : base("QA:ErrorMessage:TestCycleCannotBeStarted")
        {
            TestCycleId = testCycleId;
            Reason = reason;
            WithData("testCycleId", testCycleId);
            WithData("reason", reason);
        }
    }
}
