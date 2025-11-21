using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services
{
    /// <summary>
    /// Service for tracking compliance obligations
    /// </summary>
    public interface IComplianceTrackingService
    {
        /// <summary>
        /// Creates a new compliance obligation
        /// </summary>
        Task<ComplianceObligation> CreateObligationAsync(
            string title,
            string overview,
            string requirements,
            string notes,
            string country,
            string state,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null);
        
        /// <summary>
        /// Updates an existing compliance obligation
        /// </summary>
        Task<ComplianceObligation> UpdateObligationAsync(
            Guid id,
            string title,
            string overview,
            string requirements,
            string notes,
            string country,
            string state,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null);
        
        /// <summary>
        /// Updates the status of a compliance obligation
        /// </summary>
        Task UpdateObligationStatusAsync(
            Guid id,
            ComplianceStatus newStatus,
            string notes = null);
        
        /// <summary>
        /// Gets obligations with approaching deadlines
        /// </summary>
        Task<List<ComplianceObligation>> GetUpcomingObligationsAsync(int daysThreshold);
        
        /// <summary>
        /// Gets overdue obligations
        /// </summary>
        Task<List<ComplianceObligation>> GetOverdueObligationsAsync();
        
        /// <summary>
        /// Generates recurring obligations for completed obligations
        /// </summary>
        Task<int> GenerateRecurringObligationsAsync();
    }
}
