using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.QualityAssurance.Repositories
{
    /// <summary>
    /// Repository for test plans
    /// </summary>
    public interface ITestPlanRepository : IRepository<TestPlan, Guid>
    {
        /// <summary>
        /// Gets a test plan with all its test cycles and runs
        /// </summary>
        Task<TestPlan> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets test plans for a specific release version
        /// </summary>
        Task<List<TestPlan>> GetByReleaseVersionAsync(
            string releaseVersion, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets test plans that cover a specific requirement
        /// </summary>
        Task<List<TestPlan>> GetByRequirementIdAsync(
            Guid requirementId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets active test plans
        /// </summary>
        Task<List<TestPlan>> GetActiveTestPlansAsync(
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets test plans matching a specification
        /// </summary>
        Task<List<TestPlan>> GetListAsync(
            QASpecification<TestPlan> specification,
            CancellationToken cancellationToken = default);
    }
}
