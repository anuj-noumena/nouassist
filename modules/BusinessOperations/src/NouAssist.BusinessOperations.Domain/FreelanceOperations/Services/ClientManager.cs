namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for managing freelance clients
    /// </summary>
    public class ClientManager : DomainService, IClientManager
    {
        private readonly IClientRepository _clientRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<ClientManager> _logger;

        public ClientManager(
            IClientRepository clientRepository,
            IGuidGenerator guidGenerator,
            ICurrentUser currentUser,
            ILogger<ClientManager> logger)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<FreelanceClient> CreateClientAsync(
            string name,
            ClientContactInfo contactInfo,
            string companyName = null,
            ClientStatus initialStatus = ClientStatus.Lead,
            string source = null,
            string notes = null)
        {
            try
            {
                var clientId = _guidGenerator.Create();
                
                var client = FreelanceClient.Create(
                    clientId,
                    name,
                    contactInfo,
                    companyName,
                    initialStatus,
                    source,
                    notes,
                    CurrentTenant.Id
                );

                await _clientRepository.InsertAsync(client);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create client with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateClientBasicInfoAsync(
            Guid clientId,
            string name,
            string companyName,
            string notes)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                client.UpdateBasicInfo(name, companyName, notes);

                await _clientRepository.UpdateAsync(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update basic info for client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateClientContactInfoAsync(
            Guid clientId,
            ClientContactInfo contactInfo)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                client.UpdateContactInfo(contactInfo);

                await _clientRepository.UpdateAsync(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update contact info for client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ChangeClientStatusAsync(
            Guid clientId,
            ClientStatus newStatus)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                client.ChangeStatus(newStatus);

                await _clientRepository.UpdateAsync(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change status of client {ClientId} to {Status}", 
                    clientId, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ClientCommunicationRecord> AddCommunicationRecordAsync(
            Guid clientId,
            string communicationType,
            string subject,
            string content,
            Guid? projectId = null,
            bool requiresFollowUp = false,
            DateTime? followUpDate = null)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                var userId = _currentUser.Id ?? throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
                
                var communicationDetails = new CommunicationLog(
                    DateTime.UtcNow,
                    communicationType,
                    subject,
                    content,
                    userId.Value
                );
                
                var communicationId = _guidGenerator.Create();
                var record = client.AddCommunicationRecord(
                    communicationId,
                    communicationDetails,
                    projectId,
                    requiresFollowUp,
                    followUpDate
                );

                await _clientRepository.UpdateAsync(client);
                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add communication record to client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<FreelanceClient>> GetClientsWithPendingFollowUpsAsync(DateTime? dueBy = null)
        {
            try
            {
                var specification = new ClientsWithPendingFollowUpsSpecification(dueBy);
                return await _clientRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get clients with pending follow-ups");
                throw;
            }
        }
    }
}
