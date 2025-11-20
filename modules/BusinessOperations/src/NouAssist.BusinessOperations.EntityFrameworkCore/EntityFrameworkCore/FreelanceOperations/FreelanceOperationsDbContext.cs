using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore
{
    [ConnectionStringName(FreelanceOperationsDbProperties.ConnectionStringName)]
    public class FreelanceOperationsDbContext : 
        AbpDbContext<FreelanceOperationsDbContext>, IFreelanceOperationsDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<FreelanceProject> Projects { get; set; }
        public DbSet<FreelanceClient> Clients { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<UpworkEngagement> UpworkEngagements { get; set; }

        public FreelanceOperationsDbContext(DbContextOptions<FreelanceOperationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureFreelanceOperations();
        }
    }
}
