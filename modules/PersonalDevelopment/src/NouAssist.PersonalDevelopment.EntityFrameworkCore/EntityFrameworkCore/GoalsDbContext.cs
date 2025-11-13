using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.Goals.EntityFrameworkCore
{
    [ConnectionStringName(GoalsDbProperties.ConnectionStringName)]
    public class GoalsDbContext : AbpDbContext<GoalsDbContext>, IGoalsDbContext
    {
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        public GoalsDbContext(DbContextOptions<GoalsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureGoals(this);
        }
    }
}
