using System;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents an external interface specification for a requirement
    /// </summary>
    public class InterfaceSpecification : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public ApiSignature Signature { get; private set; }

        protected InterfaceSpecification() { }

        internal InterfaceSpecification(
            Guid id,
            Guid requirementId,
            ApiSignature signature) : base(id)
        {
            RequirementId = requirementId;
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        internal void UpdateSignature(ApiSignature signature)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }
    }
}
