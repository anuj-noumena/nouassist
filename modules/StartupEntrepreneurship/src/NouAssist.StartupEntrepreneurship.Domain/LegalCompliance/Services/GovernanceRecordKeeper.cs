namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Services
{
    /// <summary>
    /// Implementation of the governance record keeping service
    /// </summary>
    public class GovernanceRecordKeeper : DomainService, IGovernanceRecordKeeper
    {
        private readonly ICorporateRecordRepository _recordRepository;
        private readonly ILogger<GovernanceRecordKeeper> _logger;

        public GovernanceRecordKeeper(
            ICorporateRecordRepository recordRepository,
            ILogger<GovernanceRecordKeeper> logger)
        {
            _recordRepository = recordRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CorporateRecord> CreateCorporateRecordAsync(
            string title,
            string description,
            CorporateRecordType type,
            DateTime recordDate,
            string referenceNumber = null)
        {
            try
            {
                var record = CorporateRecord.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    type,
                    recordDate,
                    referenceNumber,
                    CurrentTenant.Id);

                await _recordRepository.InsertAsync(record);

                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create corporate record {Title} of type {Type}", title, type);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CorporateRecord> UpdateCorporateRecordAsync(
            Guid id,
            string title,
            string description,
            DateTime recordDate,
            string referenceNumber = null)
        {
            try
            {
                var record = await _recordRepository.GetAsync(id);
                
                record.Update(
                    title,
                    description,
                    recordDate,
                    referenceNumber);

                await _recordRepository.UpdateAsync(record);

                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update corporate record {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateCorporateRecordStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null)
        {
            try
            {
                var record = await _recordRepository.GetAsync(id);
                record.UpdateStatus(newStatus, Clock.Now, notes);

                await _recordRepository.UpdateAsync(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update status for corporate record {Id} to {Status}", id, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<CorporateRecord>> GetCorporateRecordsByTypeAsync(CorporateRecordType type)
        {
            try
            {
                return await _recordRepository.GetByTypeAsync(type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get corporate records by type {Type}", type);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<CorporateRecord>> GetCorporateRecordsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                return await _recordRepository.GetByDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get corporate records by date range");
                throw;
            }
        }
    }
}
