namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO representing the result of comparing two versions
    /// </summary>
    public class VersionComparisonResultDto
    {
        /// <summary>
        /// First version being compared
        /// </summary>
        public string FirstVersion { get; set; }
        
        /// <summary>
        /// Second version being compared
        /// </summary>
        public string SecondVersion { get; set; }
        
        /// <summary>
        /// Flag indicating if the versions are identical
        /// </summary>
        public bool AreIdentical { get; set; }
        
        /// <summary>
        /// Structured representation of differences between versions
        /// </summary>
        public object Differences { get; set; }
    }
}
