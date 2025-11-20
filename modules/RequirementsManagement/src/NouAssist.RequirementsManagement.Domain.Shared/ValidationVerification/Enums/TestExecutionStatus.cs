namespace NouAssist.RequirementsManagement.ValidationVerification.Enums
{
    /// <summary>
    /// Execution status of a test step or test case
    /// </summary>
    public enum TestExecutionStatus
    {
        /// <summary>
        /// Not yet executed
        /// </summary>
        NotExecuted = 0,

        /// <summary>
        /// Test passed successfully
        /// </summary>
        Pass = 1,

        /// <summary>
        /// Test failed
        /// </summary>
        Fail = 2,

        /// <summary>
        /// Test execution was blocked
        /// </summary>
        Blocked = 3,

        /// <summary>
        /// Test was skipped
        /// </summary>
        Skipped = 4
    }
}
