using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Data
{
    /// <summary>
    /// Data seed contributor for Product Discovery module
    /// </summary>
    public class ProductDiscoveryDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ICustomerSegmentRepository _segmentRepository;
        private readonly ICustomerInterviewRepository _interviewRepository;
        private readonly IProblemHypothesisRepository _hypothesisRepository;
        private readonly ISolutionPrototypeRepository _prototypeRepository;
        private readonly IFeatureDefinitionRepository _featureRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ProductDiscoveryDataSeedContributor> _logger;

        public ProductDiscoveryDataSeedContributor(
            ICustomerSegmentRepository segmentRepository,
            ICustomerInterviewRepository interviewRepository,
            IProblemHypothesisRepository hypothesisRepository,
            ISolutionPrototypeRepository prototypeRepository,
            IFeatureDefinitionRepository featureRepository,
            IGuidGenerator guidGenerator,
            ILogger<ProductDiscoveryDataSeedContributor> logger)
        {
            _segmentRepository = segmentRepository;
            _interviewRepository = interviewRepository;
            _hypothesisRepository = hypothesisRepository;
            _prototypeRepository = prototypeRepository;
            _featureRepository = featureRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {
                await SeedCustomerSegmentsAsync();
                await SeedCustomerInterviewsAsync();
                await SeedProblemHypothesesAsync();
                await SeedSolutionPrototypesAsync();
                await SeedFeatureDefinitionsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding Product Discovery data");
                throw;
            }
        }

        private async Task SeedCustomerSegmentsAsync()
        {
            if (await _segmentRepository.GetCountAsync() > 0)
                return;

            _logger.LogInformation("Seeding customer segments...");

            var segments = new[]
            {
                new CustomerSegment(
                    _guidGenerator.Create(),
                    "Product Managers",
                    "Product managers working on digital products",
                    new SegmentCriteria(new Dictionary<string, object>
                    {
                        ["Role"] = "Product Manager",
                        ["Industry"] = "Technology",
                        ["Experience"] = "3+ years"
                    }, 5),
                    10,
                    null
                ),
                new CustomerSegment(
                    _guidGenerator.Create(),
                    "Startup Founders",
                    "Early-stage startup founders validating product ideas",
                    new SegmentCriteria(new Dictionary<string, object>
                    {
                        ["Role"] = "Founder",
                        ["CompanyStage"] = "Seed",
                        ["Industry"] = "Technology"
                    }, 3),
                    8,
                    null
                ),
                new CustomerSegment(
                    _guidGenerator.Create(),
                    "UX Researchers",
                    "User experience researchers conducting product validation",
                    new SegmentCriteria(new Dictionary<string, object>
                    {
                        ["Role"] = "UX Researcher",
                        ["Focus"] = "Product Validation",
                        ["ToolUsage"] = "Research Tools"
                    }, 4),
                    6,
                    null
                )
            };

            foreach (var segment in segments)
            {
                await _segmentRepository.InsertAsync(segment);
            }

            _logger.LogInformation("Successfully seeded {Count} customer segments", segments.Length);
        }

        private async Task SeedCustomerInterviewsAsync()
        {
            if (await _interviewRepository.GetCountAsync() > 0)
                return;

            _logger.LogInformation("Seeding customer interviews...");

            var segments = await _segmentRepository.GetListAsync();
            if (!segments.Any()) return;

            var pmSegment = segments.First(s => s.Name == "Product Managers");

            var interviewGuide = new InterviewGuide(
                "Understand pain points in product discovery and validation",
                new List<string>
                {
                    "What challenges do you face when validating product ideas?",
                    "How do you currently gather customer feedback?",
                    "What tools do you use for product research?",
                    "How do you measure product-market fit?",
                    "What would make your validation process more efficient?"
                },
                45
            );

            var interviews = new[]
            {
                CustomerInterview.Create(
                    _guidGenerator.Create(),
                    "PM Discovery Process Interview",
                    "Understanding how product managers approach discovery",
                    InterviewType.ProblemDiscovery,
                    DateTime.UtcNow.AddDays(7),
                    "Research Team",
                    "Sarah Johnson",
                    "sarah.johnson@example.com",
                    interviewGuide,
                    pmSegment.Id
                ),
                CustomerInterview.Create(
                    _guidGenerator.Create(),
                    "Validation Tools Interview",
                    "Exploring current validation tools and methods",
                    InterviewType.SolutionValidation,
                    DateTime.UtcNow.AddDays(10),
                    "Research Team",
                    "Michael Chen",
                    "michael.chen@example.com",
                    interviewGuide,
                    pmSegment.Id
                )
            };

            foreach (var interview in interviews)
            {
                await _interviewRepository.InsertAsync(interview);
            }

            _logger.LogInformation("Successfully seeded {Count} customer interviews", interviews.Length);
        }

        private async Task SeedProblemHypothesesAsync()
        {
            if (await _hypothesisRepository.GetCountAsync() > 0)
                return;

            _logger.LogInformation("Seeding problem hypotheses...");

            var segments = await _segmentRepository.GetListAsync();
            if (!segments.Any()) return;

            var pmSegment = segments.First(s => s.Name == "Product Managers");

            var hypotheses = new[]
            {
                ProblemHypothesis.Create(
                    _guidGenerator.Create(),
                    new HypothesisStatement(
                        "Difficulty in organizing and analyzing customer feedback",
                        "Product managers in tech companies",
                        "A centralized feedback management system",
                        "Improved decision-making and faster validation cycles"
                    ),
                    "Product managers struggle with scattered feedback from multiple sources, making it difficult to identify patterns and prioritize features effectively.",
                    pmSegment.Id
                ),
                ProblemHypothesis.Create(
                    _guidGenerator.Create(),
                    new HypothesisStatement(
                        "Lack of structured approach to hypothesis validation",
                        "Early-stage product teams",
                        "A guided validation framework with templates",
                        "More rigorous and efficient validation process"
                    ),
                    "Teams often validate hypotheses ad-hoc without systematic approaches, leading to biased results and missed opportunities.",
                    pmSegment.Id
                )
            };

            foreach (var hypothesis in hypotheses)
            {
                // Add sample validation attempts
                hypothesis.AddValidationAttempt(
                    "Customer Interviews",
                    "Conduct 5 customer interviews to validate the problem",
                    DateTime.UtcNow.AddDays(5)
                );

                hypothesis.AddValidationAttempt(
                    "Survey",
                    "Send survey to target segment about current pain points",
                    DateTime.UtcNow.AddDays(10)
                );

                await _hypothesisRepository.InsertAsync(hypothesis);
            }

            _logger.LogInformation("Successfully seeded {Count} problem hypotheses", hypotheses.Length);
        }

        private async Task SeedSolutionPrototypesAsync()
        {
            if (await _prototypeRepository.GetCountAsync() > 0)
                return;

            _logger.LogInformation("Seeding solution prototypes...");

            var hypotheses = await _hypothesisRepository.GetListAsync();
            if (!hypotheses.Any()) return;

            var firstHypothesis = hypotheses.First();

            var prototypes = new[]
            {
                SolutionPrototype.Create(
                    _guidGenerator.Create(),
                    "Feedback Management Dashboard",
                    "Centralized dashboard for collecting and analyzing customer feedback",
                    PrototypeType.Mockup,
                    "Test the core concept of centralized feedback management",
                    "Product managers and UX researchers",
                    firstHypothesis.Id
                ),
                SolutionPrototype.Create(
                    _guidGenerator.Create(),
                    "Validation Framework Tool",
                    "Interactive tool for guiding hypothesis validation processes",
                    PrototypeType.InteractivePrototype,
                    "Validate the need for structured validation guidance",
                    "Product teams and startup founders",
                    firstHypothesis.Id
                )
            };

            foreach (var prototype in prototypes)
            {
                // Add initial version
                prototype.AddVersion(
                    "v1.0",
                    "Initial concept version",
                    new PrototypeLink(
                        "https://figma.com/prototype/example",
                        "Figma Prototype",
                        "figma",
                        "View-only access, no login required"
                    ),
                    DateTime.UtcNow,
                    "Design Team"
                );

                await _prototypeRepository.InsertAsync(prototype);
            }

            _logger.LogInformation("Successfully seeded {Count} solution prototypes", prototypes.Length);
        }

        private async Task SeedFeatureDefinitionsAsync()
        {
            if (await _featureRepository.GetCountAsync() > 0)
                return;

            _logger.LogInformation("Seeding feature definitions...");

            var hypotheses = await _hypothesisRepository.GetListAsync();
            var prototypes = await _prototypeRepository.GetListAsync();

            if (!hypotheses.Any()) return;

            var firstHypothesis = hypotheses.First();
            var firstPrototype = prototypes.FirstOrDefault();

            var features = new[]
            {
                FeatureDefinition.Create(
                    _guidGenerator.Create(),
                    "Feedback Collection Widget",
                    "Embeddable widget for collecting user feedback on websites",
                    "As a product manager, I want to embed a feedback widget on my website so that I can continuously collect user insights.",
                    "Given a feedback widget is embedded, when a user submits feedback, then it should be categorized and stored in the central dashboard.",
                    "Enables continuous feedback collection without disrupting user experience",
                    firstHypothesis.Id,
                    firstPrototype?.Id
                ),
                FeatureDefinition.Create(
                    _guidGenerator.Create(),
                    "Insight Pattern Detection",
                    "AI-powered pattern detection in customer feedback",
                    "As a product manager, I want the system to automatically identify patterns in feedback so that I can quickly spot trends.",
                    "Given sufficient feedback data, when patterns are detected, then they should be highlighted with confidence scores and examples.",
                    "Saves time in manual analysis and prevents missing important patterns",
                    firstHypothesis.Id,
                    firstPrototype?.Id
                ),
                FeatureDefinition.Create(
                    _guidGenerator.Create(),
                    "Validation Template Library",
                    "Pre-built templates for different types of validation experiments",
                    "As a product team member, I want access to validation templates so that I can ensure consistency in my experiments.",
                    "Given validation templates, when a user selects a template, then it should provide step-by-step guidance and success criteria.",
                    "Ensures systematic and comprehensive validation approaches",
                    firstHypothesis.Id,
                    firstPrototype?.Id
                )
            };

            foreach (var feature in features)
            {
                // Add priority scoring
                var priorityInput = new PriorityInput(
                    impact: 4,
                    effort: 3,
                    confidence: 4,
                    "Based on initial customer research and technical assessment"
                );

                feature.AddPriorityScore(
                    priorityInput,
                    DateTime.UtcNow,
                    "Product Team"
                );

                // Set MVP status for high-priority features
                if (feature.Priority >= FeaturePriority.High)
                {
                    feature.UpdateMvpStatus(MvpStatus.Planned);
                }

                await _featureRepository.InsertAsync(feature);
            }

            _logger.LogInformation("Successfully seeded {Count} feature definitions", features.Length);
        }
    }
}
