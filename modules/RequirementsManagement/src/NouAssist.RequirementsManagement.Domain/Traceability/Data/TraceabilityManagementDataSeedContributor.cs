using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Repositories;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.Traceability.Data
{
    /// <summary>
    /// Responsible for seeding initial data for the TraceabilityManagement module
    /// </summary>
    public class TraceabilityManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ITraceLinkRepository _traceLinkRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public TraceabilityManagementDataSeedContributor(
            ITraceLinkRepository traceLinkRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _traceLinkRepository = traceLinkRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _traceLinkRepository.GetCountAsync() > 0)
                {
                    return;
                }

                await SeedSampleLinksAsync();
            }
        }

        private async Task SeedSampleLinksAsync()
        {
            // Create some sample trace links for demonstration
            var links = new List<TraceLink>
            {
                CreateSampleTraceLink(
                    "REQ-001",
                    "User Authentication Requirement",
                    ArtifactType.Requirement,
                    "TEST-001",
                    "Login Test Case",
                    ArtifactType.Test,
                    LinkType.Verifies,
                    LinkDirection.Forward,
                    "Test case that verifies the authentication requirement"),
                    
                CreateSampleTraceLink(
                    "REQ-002",
                    "Data Export Requirement",
                    ArtifactType.Requirement,
                    "DESIGN-001",
                    "Export Module Design",
                    ArtifactType.Design,
                    LinkType.Refines,
                    LinkDirection.Forward,
                    "Design that refines the data export requirement"),
                    
                CreateSampleTraceLink(
                    "DESIGN-001",
                    "Export Module Design",
                    ArtifactType.Design,
                    "CODE-001",
                    "ExportService.cs",
                    ArtifactType.Code,
                    LinkType.Implements,
                    LinkDirection.Forward,
                    "Code that implements the export module design")
            };

            foreach (var link in links)
            {
                await _traceLinkRepository.InsertAsync(link);
            }

            await _traceLinkRepository.UnitOfWork.SaveChangesAsync();
        }

        private TraceLink CreateSampleTraceLink(
            string sourceId,
            string sourceName,
            ArtifactType sourceType,
            string targetId,
            string targetName,
            ArtifactType targetType,
            LinkType linkType,
            LinkDirection linkDirection,
            string description)
        {
            var sourceArtifactId = new ArtifactIdentifier(sourceId, sourceType);
            var targetArtifactId = new ArtifactIdentifier(targetId, targetType);

            return TraceLink.Create(
                _guidGenerator.Create(),
                sourceArtifactId,
                sourceName,
                targetArtifactId,
                targetName,
                linkType,
                linkDirection,
                null,
                description,
                _currentTenant.Id);
        }
    }
}
