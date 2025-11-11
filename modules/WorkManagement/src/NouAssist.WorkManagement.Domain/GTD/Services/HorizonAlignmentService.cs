using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.GTD.Services
{
    public class HorizonAlignmentService : DomainService
    {
        private readonly IGtdHorizonAlignmentRepository _alignmentRepository;

        public HorizonAlignmentService(IGtdHorizonAlignmentRepository alignmentRepository)
        {
            _alignmentRepository = alignmentRepository;
        }

        public async Task<GtdHorizonAlignment> CreateAlignmentAsync(
            Guid ownerId,
            Guid higherHorizonId,
            GtdHorizonLevel higherLevel,
            string lowerEntityType,
            Guid lowerEntityId,
            int? initialStrength = null)
        {
            // Check if alignment already exists
            var existing = await _alignmentRepository
                .FindByHorizonAndEntityAsync(higherHorizonId, lowerEntityType, lowerEntityId);

            if (existing != null)
                return existing;

            var alignment = new GtdHorizonAlignment(
                GuidGenerator.Create(),
                ownerId,
                higherHorizonId,
                higherLevel,
                lowerEntityType,
                lowerEntityId
            );

            if (initialStrength.HasValue)
            {
                alignment.UpdateStrength(initialStrength.Value);
            }

            return await _alignmentRepository.InsertAsync(alignment);
        }

        public async Task<List<GtdHorizonAlignment>> GetMisalignedEntitiesAsync(
            Guid userId,
            int weakThreshold = 2)
        {
            return await _alignmentRepository
                .GetWeakAlignmentsAsync(userId, weakThreshold);
        }

        public async Task<AlignmentAnalysisResult> AnalyzeAlignmentAsync(
            Guid userId,
            GtdHorizonLevel level)
        {
            var alignments = await _alignmentRepository
                .GetByUserAndLevelAsync(userId, level);

            var result = new AlignmentAnalysisResult
            {
                TotalAlignments = alignments.Count,
                StrongAlignments = 0,
                ModerateAlignments = 0,
                WeakAlignments = 0,
                UnvalidatedAlignments = 0
            };

            foreach (var alignment in alignments)
            {
                if (!alignment.HasBeenValidated)
                {
                    result.UnvalidatedAlignments++;
                    continue;
                }

                if (alignment.HasStrongAlignment)
                    result.StrongAlignments++;
                else if (alignment.HasWeakAlignment)
                    result.WeakAlignments++;
                else
                    result.ModerateAlignments++;
            }

            result.OverallHealthScore = CalculateHealthScore(result);
            return result;
        }

        private double CalculateHealthScore(AlignmentAnalysisResult result)
        {
            if (result.TotalAlignments == 0)
                return 0;

            var validatedCount = result.TotalAlignments - result.UnvalidatedAlignments;
            if (validatedCount == 0)
                return 0;

            var weightedScore = 
                (result.StrongAlignments * 1.0) +
                (result.ModerateAlignments * 0.6) +
                (result.WeakAlignments * 0.2);

            return (weightedScore / validatedCount) * 100;
        }
    }

    public class AlignmentAnalysisResult
    {
        public int TotalAlignments { get; set; }
        public int StrongAlignments { get; set; }
        public int ModerateAlignments { get; set; }
        public int WeakAlignments { get; set; }
        public int UnvalidatedAlignments { get; set; }
        public double OverallHealthScore { get; set; }

        public bool IsHealthy => OverallHealthScore >= 70;
        public bool NeedsAttention => OverallHealthScore < 50;
    }
}
