using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore
{
    [ConnectionStringName(ReviewsDbProperties.ConnectionStringName)]
    public interface IReviewsDbContext : IEfCoreDbContext
    {
        DbSet<Review> Reviews { get; }
        DbSet<ReviewTemplate> ReviewTemplates { get; }
        DbSet<ReviewProgress> ReviewProgresses { get; }
        DbSet<ReviewItem> ReviewItems { get; }
    }
}
