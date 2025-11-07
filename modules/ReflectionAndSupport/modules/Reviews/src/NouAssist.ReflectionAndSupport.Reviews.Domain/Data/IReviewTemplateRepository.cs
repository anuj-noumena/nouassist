using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.Reviews.Aggregates;
using NouAssist.ReflectionAndSupport.Reviews.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.ReflectionAndSupport.Reviews.Data
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
