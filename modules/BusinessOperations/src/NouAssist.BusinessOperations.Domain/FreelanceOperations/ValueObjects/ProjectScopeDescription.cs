using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a project scope description
    /// </summary>
    public class ProjectScopeDescription : ValueObject
    {
        /// <summary>
        /// The detailed description of the project scope
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Deliverables included in the project scope
        /// </summary>
        public IReadOnlyList<string> Deliverables => _deliverables.AsReadOnly();
        private readonly List<string> _deliverables;
        
        /// <summary>
        /// Whether the scope is open to changes
        /// </summary>
        public bool IsFlexible { get; private set; }

        protected ProjectScopeDescription()
        {
            _deliverables = new List<string>();
        }

        public ProjectScopeDescription(
            string description, 
            List<string> deliverables = null,
            bool isFlexible = false)
        {
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: FreelanceOperationsConsts.Project.MaxDescriptionLength
            );
            
            _deliverables = deliverables ?? new List<string>();
            IsFlexible = isFlexible;
        }

        /// <summary>
        /// Add a deliverable to the project scope
        /// </summary>
        public void AddDeliverable(string deliverable)
        {
            if (string.IsNullOrWhiteSpace(deliverable))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Deliverable");
            }
            
            _deliverables.Add(deliverable);
        }

        /// <summary>
        /// Remove a deliverable from the project scope
        /// </summary>
        public void RemoveDeliverable(string deliverable)
        {
            _deliverables.Remove(deliverable);
        }

        /// <summary>
        /// Update the flexibility status of the scope
        /// </summary>
        public void SetFlexibility(bool isFlexible)
        {
            IsFlexible = isFlexible;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return IsFlexible;
            
            foreach (var deliverable in _deliverables.OrderBy(d => d))
            {
                yield return deliverable;
            }
        }
    }
}
