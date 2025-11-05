using Microsoft.EntityFrameworkCore;
using NouAssist.Reviews.Aggregates;
using NouAssist.Reviews.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Reviews.EntityFrameworkCore
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
