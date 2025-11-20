namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Data
{
    /// <summary>
    /// Data seed contributor for Requirements Analysis Tools domain
    /// </summary>
    public class RequirementsAnalysisToolsDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGlossaryTermRepository _glossaryTermRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<RequirementsAnalysisToolsDataSeedContributor> _logger;

        public RequirementsAnalysisToolsDataSeedContributor(
            IGlossaryTermRepository glossaryTermRepository,
            IGuidGenerator guidGenerator,
            ILogger<RequirementsAnalysisToolsDataSeedContributor> logger)
        {
            _glossaryTermRepository = glossaryTermRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedGlossaryTermsAsync();
        }

        private async Task SeedGlossaryTermsAsync()
        {
            if (await _glossaryTermRepository.GetCountAsync() > 0)
            {
                return; // Already seeded
            }

            _logger.LogInformation("Seeding Requirements Analysis Tools glossary terms...");

            var terms = new[]
            {
                new { Name = "Functional Requirement", Definition = "A requirement that specifies what a system should do. It describes the functional behavior or business logic that the system must perform.", Context = "Requirements Engineering", Examples = "User login, Generate report, Process payment" },
                new { Name = "Non-Functional Requirement", Definition = "A requirement that specifies how a system should behave. It describes quality attributes, performance criteria, or constraints.", Context = "Requirements Engineering", Examples = "Response time under 2 seconds, 99.9% uptime, Support 10,000 concurrent users" },
                new { Name = "User Story", Definition = "A brief, simple description of a feature told from the perspective of the user or customer who desires the new capability.", Context = "Agile Development", Examples = "As a customer, I want to view my order history so that I can track my purchases" },
                new { Name = "Acceptance Criteria", Definition = "Specific conditions that a software product must satisfy to be accepted by the user, customer, or other stakeholder.", Context = "Requirements Engineering", Examples = "Given/When/Then scenarios, Checklist of requirements" },
                new { Name = "Business Rule", Definition = "A statement that defines or constrains some aspect of the business and always resolves to either true or false.", Context = "Business Analysis", Examples = "Customers must be 18 or older to purchase alcohol, Discount cannot exceed 50%" },
                new { Name = "Stakeholder", Definition = "Any individual, group, or organization that can affect, be affected by, or perceive themselves to be affected by a decision or activity.", Context = "Requirements Engineering", Examples = "End users, Product owner, Development team, Business sponsor" },
                new { Name = "MoSCoW", Definition = "A prioritization technique that categorizes requirements into Must have, Should have, Could have, and Won't have this time.", Context = "Prioritization", Examples = "Must: Core login functionality, Should: Password reset, Could: Social login, Won't: Biometric authentication" },
                new { Name = "WSJF", Definition = "Weighted Shortest Job First - A prioritization model used to sequence work by calculating the relative cost of delay divided by the relative job size.", Context = "Prioritization", Examples = "Feature A: Cost of Delay 50, Job Size 20, WSJF = 2.5" },
                new { Name = "Impact Analysis", Definition = "The process of identifying and evaluating the potential consequences of proposed changes to a system or process.", Context = "Requirements Engineering", Examples = "Technical impact, Business impact, User impact assessment" },
                new { Name = "Traceability", Definition = "The ability to link requirements to their origins and to track them throughout the development lifecycle.", Context = "Requirements Engineering", Examples = "Requirement R1 traces to User Story US1 and Test Case TC1" }
            };

            foreach (var termData in terms)
            {
                var term = GlossaryTerm.Create(
                    _guidGenerator.Create(),
                    termData.Name,
                    termData.Definition,
                    null, // No tenant for seed data
                    termData.Context,
                    termData.Examples
                );

                // Auto-approve seed terms
                term.Approve(Guid.Empty, DateTime.UtcNow);

                await _glossaryTermRepository.InsertAsync(term);
            }

            _logger.LogInformation("Successfully seeded {TermCount} glossary terms", terms.Length);
        }
    }
}
