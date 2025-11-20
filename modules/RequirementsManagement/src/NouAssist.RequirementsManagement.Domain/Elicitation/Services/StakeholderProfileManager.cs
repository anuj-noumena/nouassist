namespace NouAssist.RequirementsManagement.Elicitation.Services
{
    /// <summary>
    /// Domain service for managing stakeholder profiles
    /// </summary>
    public class StakeholderProfileManager : DomainService, IStakeholderProfileManager
    {
        private readonly IStakeholderProfileRepository _stakeholderRepository;
        private readonly ILogger<StakeholderProfileManager> _logger;

        public StakeholderProfileManager(
            IStakeholderProfileRepository stakeholderRepository,
            ILogger<StakeholderProfileManager> logger)
        {
            _stakeholderRepository = stakeholderRepository;
            _logger = logger;
        }

        public async Task<StakeholderProfile> CreateAsync(
            string name,
            string description,
            StakeholderRole role,
            string organization,
            ContactInfo contactInfo,
            InfluenceData influenceData)
        {
            try
            {
                // Validate email uniqueness
                if (!await IsEmailUniqueAsync(contactInfo.Email))
                {
                    throw new DuplicateStakeholderEmailException(contactInfo.Email);
                }

                var stakeholder = StakeholderProfile.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    role,
                    organization,
                    contactInfo,
                    influenceData,
                    CurrentTenant.Id);

                await _stakeholderRepository.InsertAsync(stakeholder);

                _logger.LogInformation("Created stakeholder profile for {Name} with email {Email}",
                    name, contactInfo.Email);

                return stakeholder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create stakeholder profile for {Name}", name);
                throw;
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
        {
            var existingStakeholder = await _stakeholderRepository.FindByEmailAsync(email);
            
            if (existingStakeholder == null)
                return true;

            return excludeId.HasValue && existingStakeholder.Id == excludeId.Value;
        }

        public async Task<List<StakeholderProfile>> GetKeyStakeholdersAsync()
        {
            var specification = new KeyStakeholderSpecification();
            return await _stakeholderRepository.GetListAsync(specification.ToExpression());
        }

        public async Task<List<StakeholderProfile>> GetStakeholdersByRoleAsync(params StakeholderRole[] roles)
        {
            var specification = new StakeholdersByRoleSpecification(roles);
            return await _stakeholderRepository.GetListAsync(specification.ToExpression());
        }

        public async Task ValidateStakeholderAsync(Guid stakeholderId)
        {
            var stakeholder = await _stakeholderRepository.FindAsync(stakeholderId);
            if (stakeholder == null)
            {
                throw new StakeholderNotFoundException(stakeholderId);
            }

            if (!stakeholder.IsActive)
            {
                throw new BusinessException("Elicitation:ErrorMessage:StakeholderNotActive")
                    .WithData("stakeholderId", stakeholderId);
            }
        }
    }
}
