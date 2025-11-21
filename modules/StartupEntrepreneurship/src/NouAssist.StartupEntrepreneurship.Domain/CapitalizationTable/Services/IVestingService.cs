using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Services
{
    /// <summary>
    /// Domain service for vesting calculations and management
    /// </summary>
    public interface IVestingService
    {
        /// <summary>
        /// Calculates vested amount for a security holder
        /// </summary>
        Task<ShareQuantity> CalculateVestedAmountAsync(
            Guid securityHolderId,
            DateTime asOfDate);

        /// <summary>
        /// Gets exercisable shares for a security holder
        /// </summary>
        Task<ShareQuantity> GetExercisableSharesAsync(
            Guid securityHolderId,
            DateTime asOfDate);

        /// <summary>
        /// Updates vesting for all security holders in a cap table
        /// </summary>
        Task UpdateVestingForCapTableAsync(
            Guid capTableId,
            DateTime asOfDate);

        /// <summary>
        /// Gets vesting schedule details for a security holder
        /// </summary>
        Task<List<VestingScheduleInfo>> GetVestingScheduleAsync(
            Guid securityHolderId);

        /// <summary>
        /// Calculates total unvested shares across all holders
        /// </summary>
        Task<ShareQuantity> CalculateTotalUnvestedSharesAsync(
            Guid capTableId,
            DateTime asOfDate);

        /// <summary>
        /// Processes option exercises
        /// </summary>
        Task ProcessOptionExerciseAsync(
            Guid securityHolderId,
            Guid securityIssuanceId,
            ShareQuantity quantity,
            DateTime exerciseDate);

        /// <summary>
        /// Marks expired options
        /// </summary>
        Task MarkExpiredOptionsAsync(
            DateTime asOfDate);
    }

    /// <summary>
    /// Information about a vesting schedule
    /// </summary>
    public class VestingScheduleInfo
    {
        public Guid Id { get; set; }
        public Guid SecurityIssuanceId { get; set; }
        public DateTime GrantDate { get; set; }
        public VestingTerms VestingTerms { get; set; }
        public ShareQuantity TotalShares { get; set; }
        public ShareQuantity VestedShares { get; set; }
        public VestingStatus Status { get; set; }
        public DateTime? LastVestingDate { get; set; }
        public DateTime? ExercisedDate { get; set; }
    }
}
