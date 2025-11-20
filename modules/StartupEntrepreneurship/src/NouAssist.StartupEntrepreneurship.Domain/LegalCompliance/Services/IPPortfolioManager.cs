namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Services
{
    /// <summary>
    /// Implementation of the IP portfolio management service
    /// </summary>
    public class IPPortfolioManager : DomainService, IIPPortfolioManager
    {
        private readonly IIntellectualPropertyRepository _ipRepository;
        private readonly ILogger<IPPortfolioManager> _logger;

        public IPPortfolioManager(
            IIntellectualPropertyRepository ipRepository,
            ILogger<IPPortfolioManager> logger)
        {
            _ipRepository = ipRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IntellectualPropertyAsset> CreateIPAssetAsync(
            string name,
            string description,
            IntellectualPropertyType type,
            string identifier,
            string issuingAuthority,
            string country,
            string state,
            DateTime filingDate,
            FilingStatusType initialStatus,
            DateTime? registrationDate,
            DateTime? expirationDate)
        {
            try
            {
                // Check for duplicate IP asset
                var exists = await _ipRepository.ExistsByIdentifierAsync(identifier, type);
                if (exists)
                {
                    throw new DuplicateIPAssetException(identifier, type);
                }

                // Create value objects
                var ipIdentifier = new IPIdentifier(identifier, type, issuingAuthority);
                var jurisdiction = new Jurisdiction(country, state);

                // Create IP asset
                var ipAsset = IntellectualPropertyAsset.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    type,
                    ipIdentifier,
                    jurisdiction,
                    filingDate,
                    initialStatus,
                    registrationDate,
                    expirationDate,
                    CurrentTenant.Id);

                // Persist the entity
                await _ipRepository.InsertAsync(ipAsset);

                return ipAsset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create IP asset {Name} of type {Type}", name, type);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IntellectualPropertyAsset> UpdateIPAssetAsync(
            Guid id,
            string name,
            string description,
            string country,
            string state)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(id);
                var jurisdiction = new Jurisdiction(country, state);

                ipAsset.Update(name, description, jurisdiction);

                await _ipRepository.UpdateAsync(ipAsset);

                return ipAsset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update IP asset {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateIPStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(id);
                ipAsset.UpdateStatus(newStatus, Clock.Now, notes);

                await _ipRepository.UpdateAsync(ipAsset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update status for IP asset {Id} to {Status}", id, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SetIPValuationAsync(
            Guid id,
            decimal estimatedValue,
            string currency,
            string valuationMethod,
            string valuationNotes = null)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(id);
                ipAsset.SetValuation(estimatedValue, currency, Clock.Now, valuationMethod, valuationNotes);

                await _ipRepository.UpdateAsync(ipAsset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set valuation for IP asset {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddIPFilingDeadlineAsync(
            Guid ipId,
            string title,
            DateTime deadline,
            string description = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(ipId);
                ipAsset.AddFilingDeadline(title, deadline, description, isRecurring, recurrencePattern);

                await _ipRepository.UpdateAsync(ipAsset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add filing deadline to IP asset {Id}", ipId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<IntellectualPropertyAsset>> GetIPAssetsWithApproachingDeadlinesAsync(int daysThreshold)
        {
            try
            {
                return await _ipRepository.GetWithApproachingDeadlinesAsync(daysThreshold);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get IP assets with approaching deadlines");
                throw;
            }
        }
    }
}
