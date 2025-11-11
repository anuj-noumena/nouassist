using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.Reviews.Data
{
    public interface IReviewTemplateRepository : IRepository<ReviewTemplate, Guid>
    {
        Task<List<ReviewTemplate>> GetSystemTemplatesAsync(
            CancellationToken cancellationToken = default
        );

        Task<List<ReviewTemplate>> GetCustomTemplatesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );

        Task<ReviewTemplate> GetByReviewTypeAsync(
            ReviewType reviewType,
            Guid? ownerId = null,
            CancellationToken cancellationToken = default
        );

        Task<List<ReviewTemplate>> GetTemplatesForOwnerAsync(
            Guid ownerId,
            bool includeSystem = true,
            CancellationToken cancellationToken = default
        );
    }
}
