namespace NouAssist.RequirementsManagement.Specification.Data
{
    /// <summary>
    /// Data seed contributor for Requirements Specification Management module
    /// </summary>
    public class RequirementSpecificationManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly ISpecificationStructureRepository _specificationStructureRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<RequirementSpecificationManagementDataSeedContributor> _logger;

        public RequirementSpecificationManagementDataSeedContributor(
            IRequirementRepository requirementRepository,
            ISpecificationStructureRepository specificationStructureRepository,
            IGuidGenerator guidGenerator,
            ILogger<RequirementSpecificationManagementDataSeedContributor> logger)
        {
            _requirementRepository = requirementRepository;
            _specificationStructureRepository = specificationStructureRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedSpecificationStructuresAsync();
            await SeedSampleRequirementsAsync();
        }

        private async Task SeedSpecificationStructuresAsync()
        {
            if (await _specificationStructureRepository.FindByNameAsync("Standard SRS Template") == null)
            {
                _logger.LogInformation("Seeding standard SRS template...");

                var structure = SpecificationStructure.Create(
                    _guidGenerator.Create(),
                    "Standard SRS Template",
                    "Software Requirements Specification",
                    "Standard template for software requirements specification documents",
                    null
                );

                // Add standard sections
                structure.AddSection("1. Introduction", "", 1);
                structure.AddSection("1.1 Purpose", "Describe the purpose of this document", 1, structure.Sections.First().Id);
                structure.AddSection("1.2 Scope", "Define the scope of the software system", 2, structure.Sections.First().Id);
                
                structure.AddSection("2. Overall Description", "", 2);
                structure.AddSection("2.1 Product Perspective", "Describe how the product relates to other products", 1, structure.Sections.Last().Id);
                structure.AddSection("2.2 Product Features", "Summarize the major features of the product", 2, structure.Sections.Last().Id);
                
                structure.AddSection("3. Specific Requirements", "", 3);
                structure.AddSection("3.1 Functional Requirements", "Detailed functional requirements", 1, structure.Sections.Last().Id);
                structure.AddSection("3.2 Non-Functional Requirements", "Performance, usability, and other non-functional requirements", 2, structure.Sections.Last().Id);

                await _specificationStructureRepository.InsertAsync(structure);
            }
        }

        private async Task SeedSampleRequirementsAsync()
        {
            var sampleRequirements = new[]
            {
                new { Id = "REQ-001", Title = "User Authentication", Type = RequirementType.Functional, Priority = RequirementPriority.High },
                new { Id = "REQ-002", Title = "Data Encryption", Type = RequirementType.NonFunctional, Priority = RequirementPriority.Critical },
                new { Id = "REQ-003", Title = "System Response Time", Type = RequirementType.NonFunctional, Priority = RequirementPriority.Medium },
                new { Id = "REQ-004", Title = "Business Rule Validation", Type = RequirementType.BusinessRule, Priority = RequirementPriority.High },
                new { Id = "REQ-005", Title = "Database Constraints", Type = RequirementType.Constraint, Priority = RequirementPriority.Medium }
            };

            foreach (var reqData in sampleRequirements)
            {
                if (await _requirementRepository.FindByIdentifierAsync(new RequirementIdentifier(reqData.Id)) == null)
                {
                    _logger.LogInformation("Seeding sample requirement {Identifier}...", reqData.Id);

                    var identifier = new RequirementIdentifier(reqData.Id);
                    var text = new RequirementText(reqData.Title, GetSampleDescription(reqData.Id));
                    
                    var requirement = Requirement.Create(
                        _guidGenerator.Create(),
                        identifier,
                        text,
                        reqData.Type,
                        reqData.Priority,
                        null
                    );

                    // Add sample acceptance criteria
                    requirement.AddAcceptanceCriterion(new CriterionStatement(
                        $"Given a valid scenario for {reqData.Title}, when the system processes the request, then it should meet the specified criteria",
                        "Sample acceptance criterion for demonstration",
                        CriterionType.GivenWhenThen
                    ));

                    await _requirementRepository.InsertAsync(requirement);
                }
            }
        }

        private string GetSampleDescription(string requirementId)
        {
            return requirementId switch
            {
                "REQ-001" => "The system shall provide secure user authentication using username/password and support multi-factor authentication for enhanced security.",
                "REQ-002" => "All sensitive data stored in the system shall be encrypted using AES-256 encryption standard.",
                "REQ-003" => "The system shall respond to user requests within 2 seconds under normal load conditions.",
                "REQ-004" => "The system shall enforce all business rules defined in the business rule catalog during data processing.",
                "REQ-005" => "The database shall enforce all defined constraints to maintain data integrity.",
                _ => "Sample requirement description for demonstration purposes."
            };
        }
    }
}
