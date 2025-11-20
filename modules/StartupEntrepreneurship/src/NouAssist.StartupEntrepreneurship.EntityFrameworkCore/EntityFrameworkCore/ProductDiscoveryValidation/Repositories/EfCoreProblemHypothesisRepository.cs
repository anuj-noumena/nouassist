namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ProblemHypothesis repository
    /// </summary>
    public class EfCoreProblemHypothesisRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, ProblemHypothesis, Guid>, IProblemHypothesisRepository
    {
        public EfCoreProblemHypothesisRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ProblemHypothesis>> GetListAsync(
            Expression<Func<ProblemHypothesis, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(h => h.ValidationAttempts);
            }

            return await query
                .Where(predicate)
                .OrderByDescending(h => h.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProblemHypothesis>> GetByStatusAsync(
            ValidationStatus status,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                h => h.Status == status,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<ProblemHypothesis>> GetByCustomerSegmentAsync(
            Guid customerSegmentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                h => h.RelatedCustomerSegmentId == customerSegmentId,
                includeDetails,
                cancellationToken);
        }

        public async Task<ProblemHypothesis> GetWithValidationDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var hypothesis = await query
                .Include(h => h.ValidationAttempts)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

            if (hypothesis == null)
                throw new ProblemHypothesisNotFoundException(id);

            return hypothesis;
        }

        public async Task<List<ProblemHypothesis>> GetStronglyValidatedAsync(
            int minimumValidationAttempts = 2,
            decimal minimumFitScore = 70m,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(h => h.ValidationAttempts)
                .Where(h => h.Status == ValidationStatus.Validated &&
                           h.ValidationAttempts.Count >= minimumValidationAttempts &&
                           h.FitMetric != null)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProblemHypothesis>> GetReadyForValidationAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(h => h.ValidationAttempts)
                .Where(h => h.Status == ValidationStatus.NotStarted &&
                           h.ValidationAttempts.Any())
                .OrderBy(h => h.ValidationAttempts.Min(v => v.PlannedStartDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProblemHypothesis>> GetStalledValidationAsync(
            int daysWithoutProgress = 14,
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysWithoutProgress);
            var query = await GetQueryableAsync();

            return await query
                .Include(h => h.ValidationAttempts)
                .Where(h => h.Status == ValidationStatus.InProgress &&
                           h.ValidationAttempts.All(v => 
                               v.Status == ValidationStatus.NotStarted ||
                               (v.ActualStartDate.HasValue && 
                                v.ActualStartDate.Value <= cutoffDate && 
                                !v.CompletedDate.HasValue)))
                .ToListAsync(cancellationToken);
        }

        public async Task<ValidationPortfolioSummary> GetPortfolioSummaryAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (fromDate.HasValue)
                query = query.Where(h => h.CreationTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(h => h.CreationTime <= toDate.Value);

            var hypotheses = await query
                .Include(h => h.ValidationAttempts)
                .ToListAsync(cancellationToken);

            var totalHypotheses = hypotheses.Count;
            var statusDistribution = hypotheses
                .GroupBy(h => h.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            var hypothesesWithAttempts = hypotheses.Count(h => h.ValidationAttempts.Any());
            var completedValidations = hypotheses.Count(h => 
                h.Status == ValidationStatus.Validated ||
                h.Status == ValidationStatus.Invalidated ||
                h.Status == ValidationStatus.Inconclusive);

            var overallSuccessRate = completedValidations > 0 
                ? (decimal)hypotheses.Count(h => h.Status == ValidationStatus.Validated) / completedValidations * 100
                : 0;

            var commonMethods = hypotheses
                .SelectMany(h => h.ValidationAttempts)
                .GroupBy(v => v.Method)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var avgDaysToValidation = CalculateAverageDaysToValidation(hypotheses);
            var hypothesesNeedingAttention = GetHypothesesNeedingAttention(hypotheses).Count;

            return new ValidationPortfolioSummary
            {
                TotalHypotheses = totalHypotheses,
                StatusDistribution = statusDistribution,
                HypothesesWithValidationAttempts = hypothesesWithAttempts,
                CompletedValidations = completedValidations,
                OverallSuccessRate = overallSuccessRate,
                AverageDaysToValidation = avgDaysToValidation,
                CommonValidationMethods = commonMethods,
                HypothesesNeedingAttention = hypothesesNeedingAttention
            };
        }

        public async Task<List<ProblemHypothesis>> SearchByStatementAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<ProblemHypothesis>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(h => h.Statement.Problem.ToLower().Contains(searchTerm) ||
                           h.Statement.Solution.ToLower().Contains(searchTerm) ||
                           h.Statement.Target.ToLower().Contains(searchTerm) ||
                           h.Context.ToLower().Contains(searchTerm))
                .OrderByDescending(h => h.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProblemHypothesis>> GetNeedingAttentionAsync(
            CancellationToken cancellationToken = default)
        {
            var hypotheses = await GetListAsync(h => true, true, cancellationToken);
            return GetHypothesesNeedingAttention(hypotheses);
        }

        public async Task<Dictionary<ValidationStatus, int>> GetStatusDistributionAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .GroupBy(h => h.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);
        }

        public override async Task<IQueryable<ProblemHypothesis>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(h => h.ValidationAttempts);
        }

        private int CalculateAverageDaysToValidation(List<ProblemHypothesis> hypotheses)
        {
            var validatedHypotheses = hypotheses
                .Where(h => h.ValidatedDate.HasValue)
                .ToList();

            if (!validatedHypotheses.Any())
                return 0;

            var totalDays = 0;
            var count = 0;

            foreach (var hypothesis in validatedHypotheses)
            {
                var firstAttempt = hypothesis.ValidationAttempts
                    .Where(v => v.ActualStartDate.HasValue)
                    .OrderBy(v => v.ActualStartDate)
                    .FirstOrDefault();

                if (firstAttempt != null)
                {
                    var days = (hypothesis.ValidatedDate.Value - firstAttempt.ActualStartDate.Value).Days;
                    totalDays += days;
                    count++;
                }
            }

            return count > 0 ? totalDays / count : 0;
        }

        private List<ProblemHypothesis> GetHypothesesNeedingAttention(List<ProblemHypothesis> hypotheses)
        {
            var needingAttention = new List<ProblemHypothesis>();

            foreach (var hypothesis in hypotheses)
            {
                // No validation attempts after 30 days
                if (hypothesis.Status == ValidationStatus.NotStarted && 
                    !hypothesis.ValidationAttempts.Any() &&
                    hypothesis.CreationTime <= DateTime.UtcNow.AddDays(-30))
                {
                    needingAttention.Add(hypothesis);
                    continue;
                }

                // Stalled validation (in progress > 14 days)
                if (hypothesis.Status == ValidationStatus.InProgress &&
                    hypothesis.ValidationAttempts.Any(v => 
                        v.Status == ValidationStatus.InProgress &&
                        v.ActualStartDate.HasValue &&
                        v.ActualStartDate.Value <= DateTime.UtcNow.AddDays(-14)))
                {
                    needingAttention.Add(hypothesis);
                    continue;
                }

                // Low success rate with multiple attempts
                var completedAttempts = hypothesis.ValidationAttempts
                    .Where(v => v.Status != ValidationStatus.NotStarted && v.Status != ValidationStatus.InProgress)
                    .ToList();

                if (completedAttempts.Count >= 3)
                {
                    var successRate = (decimal)completedAttempts.Count(v => v.Status == ValidationStatus.Validated) / completedAttempts.Count;
                    if (successRate < 0.5m)
                    {
                        needingAttention.Add(hypothesis);
                    }
                }
            }

            return needingAttention;
        }
    }
}
