using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ProjectExecution.ValueObjects
{
    /// <summary>
    /// Value object containing the essential details of a project
    /// </summary>
    public class ProjectDetails : ValueObject
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the project
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        public DateTime StartDate { get; private set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        public DateTime PlannedEndDate { get; private set; }
        
        protected ProjectDetails() { }
        
        public ProjectDetails(
            string name,
            string description,
            DateTime startDate,
            DateTime plannedEndDate)
        {
            SetName(name);
            SetDescription(description);
            SetDates(startDate, plannedEndDate);
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectExecutionConsts.MaxProjectNameLength
            );
        }
        
        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProjectExecutionConsts.MaxProjectDescriptionLength
            );
        }
        
        private void SetDates(DateTime startDate, DateTime plannedEndDate)
        {
            if (plannedEndDate < startDate)
            {
                throw new BusinessException("ProjectExecution:Validation:DateOrderInvalid");
            }
            
            StartDate = startDate;
            PlannedEndDate = plannedEndDate;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return StartDate;
            yield return PlannedEndDate;
        }
    }
}
