using System;
using NouAssist.WorkManagement.GTD.Aggregates;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.GTD.EntityFrameworkCore.Repositories
{
    public class EfCoreFollowupRepository 
        : EfCoreRepository<IGtdDbContext, Followup, Guid>
    {
        public EfCoreFollowupRepository(
            IDbContextProvider<IGtdDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
