namespace NouAssist.RequirementsManagement.QualityAssurance.Events
{
    /// <summary>
    /// Event raised when a test plan is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.QualityAssurance.TestPlanCreated")]
    public class TestPlanCreatedEto : QAEtoBase
    {
        /// <summary>
        /// The name of the test plan
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The version or release this test plan is associated with
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// The date when the test plan was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
