using NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Repositories
{
    public abstract class CollaborationRepositoryBase<TEntity> : CollaborationRepositoryBase<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
    {
        protected CollaborationRepositoryBase(IDbContextProvider<ICollaborationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public abstract class CollaborationRepositoryBase<TEntity, TKey> : EfCoreRepository<ICollaborationDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected CollaborationRepositoryBase(IDbContextProvider<ICollaborationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
