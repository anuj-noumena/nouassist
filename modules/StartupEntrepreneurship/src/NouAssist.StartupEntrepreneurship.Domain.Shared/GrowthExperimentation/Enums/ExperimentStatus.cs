namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums
{
    /// <summary>
    /// Represents the current status of a growth experiment
    /// </summary>
    public enum ExperimentStatus
    {
        /// <summary>
        /// Experiment is in draft mode being designed
        /// </summary>
        Draft = 0,
        
        /// <summary>
        /// Experiment is ready to be started
        /// </summary>
        Ready = 1,
        
        /// <summary>
        /// Experiment is currently running
        /// </summary>
        Running = 2,
        
        /// <summary>
        /// Experiment has been paused
        /// </summary>
        Paused = 3,
        
        /// <summary>
        /// Experiment has been completed and results are being analyzed
        /// </summary>
        AnalysisInProgress = 4,
        
        /// <summary>
        /// Experiment is completed with analysis finalized
        /// </summary>
        Completed = 5,
        
        /// <summary>
        /// Experiment has been terminated before completion
        /// </summary>
        Terminated = 6
    }
}
