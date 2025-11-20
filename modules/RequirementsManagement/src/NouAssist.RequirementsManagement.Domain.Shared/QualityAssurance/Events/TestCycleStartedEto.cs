namespace NouAssist.RequirementsManagement.QualityAssurance.Events
{
    /// <summary>
    /// Event raised when a test cycle is started
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.QualityAssurance.TestCycleStarted")]
    public class TestCycleStartedEto : QAEtoBase
    {
        /// <summary>
        /// The name of the test cycle
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The ID of the test plan this cycle belongs to
        /// </summary>
        public Guid TestPlanId { get; set; }
        
        /// <summary>
        /// The build information associated with this test cycle
        /// </summary>
        public string BuildInformation { get; set; }
        
        /// <summary>
        /// The date when the test cycle was started
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
