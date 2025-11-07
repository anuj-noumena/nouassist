using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reviews.EntityFrameworkCore;

[ConnectionStringName(ReviewsDbProperties.ConnectionStringName)]
public interface IReviewsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
