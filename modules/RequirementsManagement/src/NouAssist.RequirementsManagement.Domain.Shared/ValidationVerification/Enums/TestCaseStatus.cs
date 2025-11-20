namespace NouAssist.RequirementsManagement.ValidationVerification.Enums
{
    /// <summary>
    /// Status of a test case
    /// </summary>
    public enum TestCaseStatus
    {
        /// <summary>
        /// Test case is in draft state
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Test case is ready for execution
        /// </summary>
        Ready = 1,

        /// <summary>
        /// Test case is currently being executed
        /// </summary>
        InExecution = 2,

        /// <summary>
        /// Test case execution is blocked
        /// </summary>
        Blocked = 3,

        /// <summary>
        /// Test case has been discontinued
        /// </summary>
        Obsolete = 4
    }
}
