namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of FeatureDefinition repository
    /// </summary>
    public class EfCoreFeatureDefinitionRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, FeatureDefinition, Guid>, IFeatureDefinitionRepository
    {
        public EfCoreFeatureDefinitionRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<FeatureDefinition>> GetListAsync(
            Expression<Func<FeatureDefinition, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(f => f.PriorityScores);
            }

            return await query
                .Where(predicate)
                .OrderByDescending(f => f.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetByPriorityAsync(
            FeaturePriority priority,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                f => f.Priority == priority,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetByMvpStatusAsync(
            MvpStatus mvpStatus,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                f => f.MvpStatus == mvpStatus,
                includeDetails,
                cancellationToken);
        }

        public async Task<FeatureDefinition> GetWithPriorityScoresAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var feature = await query
                .Include(f => f.PriorityScores)
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (feature == null)
                throw new FeatureDefinitionNotFoundException(id);

            return feature;
        }

        public async Task<List<FeatureDefinition>> GetMvpReadyAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(f => f.PriorityScores)
                .Where(f => f.Priority >= FeaturePriority.High &&
                           f.MvpStatus == MvpStatus.Planned &&
                           f.PriorityScores.Any())
                .OrderByDescending(f => f.Priority)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetHighImpactAsync(
            decimal minimumPriorityScore = 10m,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(f => f.PriorityScores)
                .Where(f => f.PriorityScores.Any())
                .ToListAsync(cancellationToken)
                .ContinueWith(task =>
                {
                    var features = task.Result;
                    return features
                        .Where(f => f.PriorityScores.Average(s => s.CalculatedScore) >= minimumPriorityScore)
                        .OrderByDescending(f => f.PriorityScores.Average(s => s.CalculatedScore))
                        .ToList();
                }, cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetNeedingPrioritizationAsync(
            int daysSinceLastPrioritization = 30,
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysSinceLastPrioritization);
            var query = await GetQueryableAsync();

            return await query
                .Include(f => f.PriorityScores)
                .Where(f => !f.PriorityScores.Any() ||
                           f.PriorityScores.All(s => s.CalculatedAt <= cutoffDate))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetValidatedHypothesisAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                f => f.RelatedHypothesisId.HasValue,
                true,
                cancellationToken);
        }

        public async Task<FeatureStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (fromDate.HasValue)
                query = query.Where(f => f.CreationTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(f => f.CreationTime <= toDate.Value);

            var features = await query
                .Include(f => f.PriorityScores)
                .ToListAsync(cancellationToken);

            var totalFeatures = features.Count;

            var featuresByPriority = features
                .GroupBy(f => f.Priority)
                .ToDictionary(g => g.Key, g => g.Count());

            var featuresByMvpStatus = features
                .GroupBy(f => f.MvpStatus)
                .ToDictionary(g => g.Key, g => g.Count());

            var featuresWithPriorityScores = features.Count(f => f.PriorityScores.Any());
            var featuresInMvp = features.Count(f => f.IsIncludedInMvp());
            var featuresNeedingPrioritization = features.Count(f => 
                !f.PriorityScores.Any() || 
                f.PriorityScores.All(s => s.CalculatedAt <= DateTime.UtcNow.AddDays(-30)));

            var avgPriorityScore = features
                .Where(f => f.PriorityScores.Any())
                .Select(f => f.GetAveragePriorityScore())
                .DefaultIfEmpty(0)
                .Average();

            var highImpactFeatures = features
                .Where(f => f.PriorityScores.Any())
                .Count(f => f.GetAveragePriorityScore() >= 10m);

            var featuresByHypothesis = features
                .Where(f => f.RelatedHypothesisId.HasValue)
                .GroupBy(f => f.RelatedHypothesisId.Value)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var featuresByPrototype = features
                .Where(f => f.RelatedPrototypeId.HasValue)
                .GroupBy(f => f.RelatedPrototypeId.Value)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            return new FeatureStatistics
            {
                TotalFeatures = totalFeatures,
                FeaturesByPriority = featuresByPriority,
                FeaturesByMvpStatus = featuresByMvpStatus,
                FeaturesWithPriorityScores = featuresWithPriorityScores,
                FeaturesInMvp = featuresInMvp,
                FeaturesNeedingPrioritization = featuresNeedingPrioritization,
                AveragePriorityScore = avgPriorityScore,
                HighImpactFeatures = highImpactFeatures,
                FeaturesByHypothesis = featuresByHypothesis,
                FeaturesByPrototype = featuresByPrototype
            };
        }

        public async Task<List<FeatureDefinition>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<FeatureDefinition>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(f => f.Name.ToLower().Contains(searchTerm) ||
                           f.Description.ToLower().Contains(searchTerm) ||
                           f.UserStory.ToLower().Contains(searchTerm) ||
                           f.BusinessValue.ToLower().Contains(searchTerm))
                .OrderByDescending(f => f.Priority)
                .ThenByDescending(f => f.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetOrderedByPriorityAsync(
            int? maxResults = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .Include(f => f.PriorityScores)
                .OrderByDescending(f => f.Priority)
                .ThenByDescending(f => f.PriorityScores.Any() ? f.PriorityScores.Max(s => s.CalculatedScore) : 0);

            if (maxResults.HasValue)
                query = query.Take(maxResults.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<FeaturePriority, int>> GetPriorityDistributionAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .GroupBy(f => f.Priority)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);
        }

        public async Task<Dictionary<MvpStatus, int>> GetMvpStatusDistributionAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .GroupBy(f => f.MvpStatus)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetByHypothesisAsync(
            Guid hypothesisId,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                f => f.RelatedHypothesisId == hypothesisId,
                true,
                cancellationToken);
        }

        public async Task<List<FeatureDefinition>> GetByPrototypeAsync(
            Guid prototypeId,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                f => f.RelatedPrototypeId == prototypeId,
                true,
                cancellationToken);
        }

        public override async Task<IQueryable<FeatureDefinition>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(f => f.PriorityScores);
        }
    }
}
