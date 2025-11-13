using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore
{
    public static class ReviewsDbContextModelCreatingExtensions
    {
        public static void ConfigureReviews(
            this ModelBuilder builder,
            ReviewsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(ReviewsDbContext).Assembly);
        }
    }
}
