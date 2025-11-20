namespace NouAssist.BusinessOperations.FreelanceOperations.Data
{
    /// <summary>
    /// Seeds initial data for FreelanceOperations module
    /// </summary>
    public class FreelanceOperationsDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IClientRepository _clientRepository;
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public FreelanceOperationsDataSeedContributor(
            IClientRepository clientRepository,
            IFreelanceProjectRepository projectRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                // Only seed data if database is empty
                if (await _clientRepository.GetCountAsync() > 0)
                {
                    return;
                }

                // Seed sample client
                var sampleClient = FreelanceClient.Create(
                    _guidGenerator.Create(),
                    "Acme Corporation",
                    new ClientContactInfo("contact@acmecorp.com", "555-123-4567", "https://acmecorp.com"),
                    "Acme Inc.",
                    ClientStatus.Active,
                    "Demo Data",
                    "This is a sample client for demonstration purposes.",
                    context?.TenantId
                );

                await _clientRepository.InsertAsync(sampleClient);

                // Seed sample project
                var projectScope = new ProjectScopeDescription(
                    "Design and development of corporate website",
                    new List<string> { "Homepage design", "About page", "Contact form", "Responsive layout" },
                    true
                );

                var sampleProject = FreelanceProject.CreateHourlyProject(
                    _guidGenerator.Create(),
                    "Acme Website Redesign",
                    sampleClient.Id,
                    new HourlyRate(75, "USD"),
                    projectScope,
                    DateTime.UtcNow.AddDays(-10),
                    DateTime.UtcNow.AddDays(20),
                    context?.TenantId
                );

                sampleProject.ChangeStatus(ProjectStatus.Active, DateTime.UtcNow);
                await _projectRepository.InsertAsync(sampleProject);
            }
        }
    }
}
