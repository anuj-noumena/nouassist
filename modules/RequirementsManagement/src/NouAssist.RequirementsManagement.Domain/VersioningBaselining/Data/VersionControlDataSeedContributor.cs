using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Data
{
    /// <summary>
    /// Data seed contributor for the VersionControl module
    /// </summary>
    public class VersionControlDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IBaselineDefinitionRepository _baselineRepository;
        private readonly IRequirementBranchRepository _branchRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public VersionControlDataSeedContributor(
            IBaselineDefinitionRepository baselineRepository,
            IRequirementBranchRepository branchRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _baselineRepository = baselineRepository;
            _branchRepository = branchRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        /// <summary>
        /// Seeds initial data for the VersionControl module
        /// </summary>
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _baselineRepository.GetCountAsync() > 0)
            {
                return; // Already seeded
            }
            
            // Create a "Main" baseline
            var mainBaseline = BaselineDefinition.Create(
                _guidGenerator.Create(),
                "Main",
                "Main baseline for requirements",
                _currentTenant.Id);
            
            await _baselineRepository.InsertAsync(mainBaseline, true);
            
            // Create a "Main" branch
            var author = new AuthorRef(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "System");
                
            var mainBranch = RequirementBranch.CreateFromBaseline(
                _guidGenerator.Create(),
                new BranchName("Main"),
                "Main development branch",
                mainBaseline.Id,
                author,
                DateTime.UtcNow,
                _currentTenant.Id);
                
            await _branchRepository.InsertAsync(mainBranch, true);
        }
    }
}
