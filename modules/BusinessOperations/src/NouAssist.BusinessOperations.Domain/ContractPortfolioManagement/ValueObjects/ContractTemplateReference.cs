using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to a contract template
    /// </summary>
    public class ContractTemplateReference : ValueObject
    {
        public Guid TemplateId { get; private set; }
        public string TemplateName { get; private set; }
        public string Version { get; private set; }

        protected ContractTemplateReference() { }

        public ContractTemplateReference(Guid templateId, string templateName, string version)
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
