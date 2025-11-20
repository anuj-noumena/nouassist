namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Services
{
    /// <summary>
    /// Domain service for customer segment profiling
    /// </summary>
    public class CustomerSegmentProfileService : DomainService, ICustomerSegmentProfileService
    {
        private readonly ICustomerSegmentProfileRepository _segmentRepository;
        private readonly ILogger<CustomerSegmentProfileService> _logger;

        public CustomerSegmentProfileService(
            ICustomerSegmentProfileRepository segmentRepository,
            ILogger<CustomerSegmentProfileService> logger)
        {
            _segmentRepository = segmentRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CustomerSegmentProfile> CreateSegmentProfileAsync(
            string name,
            string description,
            SegmentType type,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds)
        {
            try
            {
                var existingSegment = await _segmentRepository.FindByNameAsync(name);
                if (existingSegment != null)
                {
                    throw new BusinessException("MarketIntelligence:ErrorMessage:DuplicateSegmentName")
                        .WithData("name", name);
                }

                var segment = CustomerSegmentProfile.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    type,
                    demographics,
                    psychographics,
                    primaryNeeds,
                    CurrentTenant.Id
                );

                await _segmentRepository.InsertAsync(segment);
                return segment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create customer segment profile with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CustomerSegmentProfile> UpdateSegmentProfileAsync(
            Guid segmentId,
            string name,
            string description,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(segmentId);

                if (segment.Name != name)
                {
                    var existingSegment = await _segmentRepository.FindByNameAsync(name);
                    if (existingSegment != null && existingSegment.Id != segmentId)
                    {
                        throw new BusinessException("MarketIntelligence:ErrorMessage:DuplicateSegmentName")
                            .WithData("name", name);
                    }
                }

                segment.UpdateProfile(
                    name,
                    description,
                    demographics,
                    psychographics,
                    primaryNeeds
                );

                await _segmentRepository.UpdateAsync(segment);
                return segment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update customer segment profile {SegmentId}", segmentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateMarketSizingAsync(
            Guid segmentId,
            decimal estimatedSize,
            decimal estimatedValue)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(segmentId);
                segment.UpdateMarketSizing(estimatedSize, estimatedValue);
                await _segmentRepository.UpdateAsync(segment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update market sizing for segment {SegmentId}", segmentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddSegmentCharacteristicAsync(
            Guid segmentId,
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance = 0.5m,
            string notes = null)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(segmentId);
                
                segment.AddCharacteristic(
                    characteristicName,
                    characteristicValue,
                    category,
                    importance,
                    notes
                );

                await _segmentRepository.UpdateAsync(segment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add characteristic to segment {SegmentId}", segmentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<CustomerSegmentProfile>> GetActiveSegmentProfilesAsync()
        {
            try
            {
                return await _segmentRepository.GetActiveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get active segment profiles");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<CustomerSegmentProfile>> GetPrimarySegmentsAsync()
        {
            try
            {
                var primarySpec = new PrimaryCustomerSegmentSpecification();
                var activeSpec = new ActiveCustomerSegmentSpecification();
                var combinedSpec = primarySpec.And(activeSpec);

                return await _segmentRepository.GetListAsync(combinedSpec.ToExpression());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get primary segments");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<Guid, decimal>> EvaluateSegmentAttractivenessAsync()
        {
            try
            {
                var segments = await _segmentRepository.GetActiveAsync();
                var result = new Dictionary<Guid, decimal>();

                foreach (var segment in segments)
                {
                    result[segment.Id] = segment.CalculateAttractiveness();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to evaluate segment attractiveness");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<Guid, bool>> EvaluateSegmentDataCompletenessAsync()
        {
            try
            {
                var segments = await _segmentRepository.GetActiveAsync();
                var result = new Dictionary<Guid, bool>();

                foreach (var segment in segments)
                {
                    result[segment.Id] = segment.HasSufficientProfileData();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to evaluate segment data completeness");
                throw;
            }
        }
    }
}
