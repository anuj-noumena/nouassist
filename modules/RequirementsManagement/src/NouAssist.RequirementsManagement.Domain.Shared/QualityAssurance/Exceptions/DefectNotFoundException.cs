using System;

namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when a defect is not found
    /// </summary>
    public class DefectNotFoundException : QADomainException
    {
        /// <summary>
        /// The ID of the defect that was not found
        /// </summary>
        public Guid DefectId { get; }

        /// <summary>
        /// Creates a new instance of DefectNotFoundException
        /// </summary>
        public DefectNotFoundException(Guid id)
            : base("QA:ErrorMessage:DefectNotFound")
        {
            DefectId = id;
            WithData("defectId", id);
        }
    }
}
