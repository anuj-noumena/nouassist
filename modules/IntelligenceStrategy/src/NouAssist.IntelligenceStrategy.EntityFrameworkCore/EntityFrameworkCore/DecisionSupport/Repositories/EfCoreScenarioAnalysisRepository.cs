using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.DecisionSupport.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DecisionSupport.Repositories
{
    public class EfCoreScenarioAnalysisRepository : 
        EfCoreRepository<DecisionSupportDbContext, ScenarioAnalysis, Guid>,
        IScenarioAnalysisRepository
    {
        public EfCoreScenarioAnalysisRepository(
            IDbContextProvider<DecisionSupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ScenarioAnalysis> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.ScenarioAnalyses
                .Include(sa => sa.ScenarioModels)
                    .ThenInclude(sm => sm.Outcomes)
                .FirstOrDefaultAsync(sa => sa.Id == id);
        }

        public async Task<List<ScenarioAnalysis>> GetByDecisionModelIdAsync(Guid decisionModelId)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.ScenarioAnalyses
                .Where(sa => sa.DecisionModelId == decisionModelId)
                .ToListAsync();
        }
    }
}
