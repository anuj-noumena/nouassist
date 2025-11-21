using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories
{
    /// <summary>
    /// Repository for legal agreements
    /// </summary>
    public interface ILegalAgreementRepository : IRepository<LegalAgreement, Guid>
    {
        /// <summary>
        /// Gets agreements that are about to expire
        /// </summary>
        Task<List<LegalAgreement>> GetExpiringAgreementsAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets agreements by status
        /// </summary>
        Task<List<LegalAgreement>> GetByStatusAsync(
            FilingStatusType status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets agreements by type
        /// </summary>
        Task<List<LegalAgreement>> GetByTypeAsync(
            LegalAgreementType type,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets agreements by counterparty
        /// </summary>
        Task<List<LegalAgreement>> GetByCounterpartyAsync(
            string counterpartyName,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets active agreements as of a specific date
        /// </summary>
        Task<List<LegalAgreement>> GetActiveAgreementsAsync(
            DateTime referenceDate,
            CancellationToken cancellationToken = default);
    }
}
