using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract
{
    /// <summary>
    /// Represents an amendment to a contract
    /// </summary>
    public class ContractAmendment : Entity<Guid>
    {
        public Guid ContractId { get; private set; }
        public AmendmentDetails Details { get; private set; }
        public DocumentId DocumentId { get; private set; }
        public DateTime CreationTime { get; private set; }

        protected ContractAmendment() { }

        public ContractAmendment(
            Guid id,
            Guid contractId,
            AmendmentDetails details,
            DocumentId documentId)
            : base(id)
        {
            ContractId = contractId;
            Details = details;
            DocumentId = documentId;
            CreationTime = DateTime.Now;
        }

        public void UpdateDetails(AmendmentDetails details)
        {
            Details = details;
        }
    }
}
