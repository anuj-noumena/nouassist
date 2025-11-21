using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a summary of proposal content
    /// </summary>
    public class ProposalContentSummary : ValueObject
    {
        /// <summary>
        /// Brief summary of the proposal
        /// </summary>
        public string Summary { get; private set; }
        
        /// <summary>
        /// Key sections included in the proposal
        /// </summary>
        public IReadOnlyList<string> KeySections => _keySections.AsReadOnly();
        private readonly List<string> _keySections;

        protected ProposalContentSummary()
        {
            _keySections = new List<string>();
        }

        public ProposalContentSummary(string summary, List<string> keySections = null)
        {
            Summary = Check.NotNullOrWhiteSpace(
                summary, 
                nameof(summary), 
                maxLength: FreelanceOperationsConsts.Proposal.MaxSummaryLength
            );
            
            _keySections = keySections ?? new List<string>();
        }

        /// <summary>
        /// Add a key section to the proposal summary
        /// </summary>
        public void AddKeySection(string section)
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Section name");
            }
            
            _keySections.Add(section);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Summary;
            
            foreach (var section in _keySections.OrderBy(s => s))
            {
                yield return section;
            }
        }
    }
}
