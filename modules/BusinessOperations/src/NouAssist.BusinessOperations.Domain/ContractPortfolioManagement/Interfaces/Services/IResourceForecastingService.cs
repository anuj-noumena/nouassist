using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Models;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for forecasting resource needs
    /// </summary>
    public interface IResourceForecastingService
    {
        /// <summary>
        /// Gets resource requirements for a portfolio
        /// </summary>
        Task<ResourceRequirements> GetPortfolioResourceRequirementsAsync(
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Gets skill requirements for a roadmap milestone
        /// </summary>
        Task<List<ResourceSkillRequirement>> GetMilestoneSkillRequirementsAsync(
            Guid roadmapId,
            Guid milestoneId);
            
        /// <summary>
        /// Analyzes resource allocation conflicts
        /// </summary>
        Task<List<ResourceConflict>> AnalyzeResourceConflictsAsync(
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate);
    }
}
