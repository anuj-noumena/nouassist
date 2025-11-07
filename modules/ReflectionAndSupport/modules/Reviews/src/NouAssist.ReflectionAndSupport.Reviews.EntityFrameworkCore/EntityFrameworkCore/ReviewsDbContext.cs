using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Reviews.EntityFrameworkCore;

[ConnectionStringName(ReviewsDbProperties.ConnectionStringName)]
public class ReviewsDbContext : AbpDbContext<ReviewsDbContext>, IReviewsDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureReviews();
    }
}
