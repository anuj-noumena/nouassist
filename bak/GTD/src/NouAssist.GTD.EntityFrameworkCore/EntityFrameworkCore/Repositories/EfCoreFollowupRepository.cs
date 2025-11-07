using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.GTD.Aggregates;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.GTD.EntityFrameworkCore.Repositories
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
