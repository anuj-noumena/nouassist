using System;

namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when a test cycle is not found
    /// </summary>
    public class TestCycleNotFoundException : QADomainException
    {
        /// <summary>
        /// The ID of the test cycle that was not found
        /// </summary>
        public Guid TestCycleId { get; }

        /// <summary>
        /// Creates a new instance of TestCycleNotFoundException
        /// </summary>
        public TestCycleNotFoundException(Guid id)
            : base("QA:ErrorMessage:TestCycleNotFound")
        {
            TestCycleId = id;
            WithData("testCycleId", id);
        }
    }
}
