using Microsoft.EntityFrameworkCore;
using NouAssist.ReflectionAndSupport.Reviews.Aggregates;
using NouAssist.ReflectionAndSupport.Reviews.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reviews.EntityFrameworkCore
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
