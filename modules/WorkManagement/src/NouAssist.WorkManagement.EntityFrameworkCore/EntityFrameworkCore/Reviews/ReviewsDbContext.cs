using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews
{
    [ConnectionStringName(ReviewsDbProperties.ConnectionStringName)]
    public class ReviewsDbContext : AbpDbContext<ReviewsDbContext>, IReviewsDbContext
    {
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewTemplate> ReviewTemplates { get; set; }
        public DbSet<ReviewProgress> ReviewProgresses { get; set; }
        public DbSet<ReviewItem> ReviewItems { get; set; }

        public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureReviews(this);
        }
    }
}
