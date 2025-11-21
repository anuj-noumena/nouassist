using System;

namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when a test plan is not found
    /// </summary>
    public class TestPlanNotFoundException : QADomainException
    {
        /// <summary>
        /// The ID of the test plan that was not found
        /// </summary>
        public Guid TestPlanId { get; }

        /// <summary>
        /// Creates a new instance of TestPlanNotFoundException
        /// </summary>
        public TestPlanNotFoundException(Guid id)
            : base("QA:ErrorMessage:TestPlanNotFound")
        {
            TestPlanId = id;
            WithData("testPlanId", id);
        }
    }
}
