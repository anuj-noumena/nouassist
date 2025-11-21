using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents a compliance record for a portfolio
    /// </summary>
    public class PortfolioComplianceRecord : Entity<Guid>
    {
        public Guid PortfolioId { get; private set; }
        public ComplianceRule Rule { get; private set; }
        public bool IsCompliant { get; private set; }
        public string EvidenceDescription { get; private set; }
        public DateTime AssessmentDate { get; private set; }

        protected PortfolioComplianceRecord() { }

        public PortfolioComplianceRecord(
            Guid id,
            Guid portfolioId,
            ComplianceRule rule,
            bool isCompliant,
            string evidenceDescription,
            DateTime assessmentDate)
            : base(id)
        {
            PortfolioId = portfolioId;
            Rule = rule;
            IsCompliant = isCompliant;
            EvidenceDescription = evidenceDescription;
            AssessmentDate = assessmentDate;
        }

        public void UpdateStatus(
            bool isCompliant,
            string evidenceDescription,
            DateTime assessmentDate)
        {
            IsCompliant = isCompliant;
            EvidenceDescription = evidenceDescription;
            AssessmentDate = assessmentDate;
        }
    }
}
