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
    /// Repository for test cycles
    /// </summary>
    public interface ITestCycleRepository : IRepository<TestCycle, Guid>
    {
        /// <summary>
        /// Gets a test cycle with all its test runs
        /// </summary>
        Task<TestCycle> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets test cycles for a specific test plan
        /// </summary>
        Task<List<TestCycle>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets active test cycles assigned to a specific user
        /// </summary>
        Task<List<TestCycle>> GetActiveByAssigneeIdAsync(
            Guid assigneeId, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets test cycles matching a specification
        /// </summary>
        Task<List<TestCycle>> GetListAsync(
            QASpecification<TestCycle> specification,
            CancellationToken cancellationToken = default);
    }
}
