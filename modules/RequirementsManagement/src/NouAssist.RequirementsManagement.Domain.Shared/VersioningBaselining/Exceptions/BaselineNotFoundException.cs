using System;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when a baseline cannot be found
    /// </summary>
    public class BaselineNotFoundException : VersionControlDomainException
    {
        /// <summary>
        /// ID of the baseline
        /// </summary>
        public Guid BaselineId { get; }

        /// <summary>
        /// Creates a new BaselineNotFoundException
        /// </summary>
        public BaselineNotFoundException(Guid baselineId)
            : base("VersionControl:ErrorMessage:BaselineNotFound")
        {
            BaselineId = baselineId;
            
            WithData("baselineId", baselineId);
        }
    }
}
