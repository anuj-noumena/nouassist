using Microsoft.EntityFrameworkCore;
using NouAssist.Comments.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Comments.EntityFrameworkCore
{
    [ConnectionStringName(CommentsDbProperties.ConnectionStringName)]
    public class CommentsDbContext : AbpDbContext<CommentsDbContext>, ICommentsDbContext
    {
        public DbSet<Comment> Comments { get; set; }

        public CommentsDbContext(DbContextOptions<CommentsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureComments(this);
        }
    }
}
