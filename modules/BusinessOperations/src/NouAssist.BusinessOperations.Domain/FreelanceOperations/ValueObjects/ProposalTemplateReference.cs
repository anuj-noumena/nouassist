using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to a proposal template
    /// </summary>
    public class ProposalTemplateReference : ValueObject
    {
        /// <summary>
        /// ID of the template
        /// </summary>
        public Guid TemplateId { get; private set; }
        
        /// <summary>
        /// Name of the template
        /// </summary>
        public string TemplateName { get; private set; }
        
        /// <summary>
        /// Version of the template used
        /// </summary>
        public string Version { get; private set; }

        protected ProposalTemplateReference()
        {
        }

        public ProposalTemplateReference(Guid templateId, string templateName, string version = "1.0")
        {
            TemplateId = templateId;
            TemplateName = Check.NotNullOrWhiteSpace(templateName, nameof(templateName));
            Version = Check.NotNullOrWhiteSpace(version, nameof(version));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TemplateId;
            yield return TemplateName;
            yield return Version;
        }
    }
}
