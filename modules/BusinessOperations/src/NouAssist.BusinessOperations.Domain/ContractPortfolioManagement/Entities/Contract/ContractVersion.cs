using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract
{
    /// <summary>
    /// Represents a specific version of a contract
    /// </summary>
    public class ContractVersion : Entity<Guid>
    {
        public Guid ContractId { get; private set; }
        public VersionMetadata Metadata { get; private set; }
        public DocumentId DocumentId { get; private set; }
        public DateTime CreationTime { get; private set; }

        protected ContractVersion() { }

        public ContractVersion(
            Guid id,
            Guid contractId,
            VersionMetadata metadata,
            DocumentId documentId)
            : base(id)
        {
            ContractId = contractId;
            Metadata = metadata;
            DocumentId = documentId;
            CreationTime = DateTime.Now;
        }
    }
}
