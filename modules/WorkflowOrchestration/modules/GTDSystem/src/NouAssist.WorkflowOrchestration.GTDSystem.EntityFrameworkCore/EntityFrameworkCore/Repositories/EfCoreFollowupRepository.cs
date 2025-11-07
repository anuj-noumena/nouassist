using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore.Repositories
{
    public class EfCoreFollowupRepository 
        : EfCoreRepository<IGTDSystemDbContext, Followup, Guid>
    {
        public EfCoreFollowupRepository(
            IDbContextProvider<IGTDSystemDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
