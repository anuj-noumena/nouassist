namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the status of a defect
    /// </summary>
    public enum DefectStatus
    {
        /// <summary>
        /// Defect has been submitted but not yet reviewed
        /// </summary>
        New = 0,
        
        /// <summary>
        /// Defect has been assigned to a developer
        /// </summary>
        Assigned = 1,
        
        /// <summary>
        /// Defect is being investigated
        /// </summary>
        InProgress = 2,
        
        /// <summary>
        /// Defect has been fixed but not verified
        /// </summary>
        Fixed = 3,
        
        /// <summary>
        /// QA has verified the fix
        /// </summary>
        Verified = 4,
        
        /// <summary>
        /// Defect could not be reproduced
        /// </summary>
        CannotReproduce = 5,
        
        /// <summary>
        /// Determined not to be a defect
        /// </summary>
        NotADefect = 6,
        
        /// <summary>
        /// Defect deferred to future release
        /// </summary>
        Deferred = 7,
        
        /// <summary>
        /// Defect is a duplicate of another defect
        /// </summary>
        Duplicate = 8,
        
        /// <summary>
        /// Defect has been closed
        /// </summary>
        Closed = 9,
        
        /// <summary>
        /// Fix was rejected by QA
        /// </summary>
        Rejected = 10
    }
}
