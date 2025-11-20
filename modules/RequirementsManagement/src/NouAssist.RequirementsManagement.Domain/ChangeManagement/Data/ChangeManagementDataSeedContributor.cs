using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using NouAssist.RequirementsManagement.ChangeManagement.Repositories;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.ChangeManagement.Data
{
    /// <summary>
    /// Data seed contributor for Change Management module
    /// </summary>
    public class ChangeManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRequirementChangeRequestRepository _changeRequestRepository;

        public ChangeManagementDataSeedContributor(
            IRequirementChangeRequestRepository changeRequestRepository)
        {
            _changeRequestRepository = changeRequestRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _changeRequestRepository.CountAsync() > 0)
            {
                return; // Already seeded
            }

            // Seed sample change requests
            await SeedSampleChangeRequestsAsync();
        }

        private async Task SeedSampleChangeRequestsAsync()
        {
            // Sample change request 1
            var changeRequest1 = RequirementChangeRequest.Create(
                Guid.NewGuid(),
                new RequestIdentifier("CR-2024-001"),
                "Update User Authentication Requirements",
                "Need to enhance user authentication to support multi-factor authentication",
                new AffectedRequirementRef(Guid.NewGuid(), "1.0"),
                Guid.NewGuid(), // Sample submitter ID
                DateTime.UtcNow
            );

            changeRequest1.AddRationale(
                "Increased security requirements due to recent security audits",
                "Security Enhancement"
            );

            changeRequest1.AddImpactStatement(
                new ImpactDetails(ImpactType.Cost, costImpact: 15000m),
                "Additional development and testing required for MFA implementation"
            );

            changeRequest1.AddImpactStatement(
                new ImpactDetails(ImpactType.Time, timeImpactDays: 21),
                "Three weeks additional development time required"
            );

            await _changeRequestRepository.InsertAsync(changeRequest1);

            // Sample change request 2
            var changeRequest2 = RequirementChangeRequest.Create(
                Guid.NewGuid(),
                new RequestIdentifier("CR-2024-002"),
                "Modify Reporting Dashboard Requirements",
                "Update reporting requirements to include new KPIs and metrics",
                new AffectedRequirementRef(Guid.NewGuid(), "2.1"),
                Guid.NewGuid(), // Sample submitter ID
                DateTime.UtcNow.AddDays(-5)
            );

            changeRequest2.AddRationale(
                "Business stakeholders need additional analytics for decision making",
                "Business Enhancement"
            );

            changeRequest2.AddImpactStatement(
                new ImpactDetails(ImpactType.Scope, dependencies: "Data warehouse integration"),
                "Requires integration with existing data sources"
            );

            await _changeRequestRepository.InsertAsync(changeRequest2);
        }
    }
}
