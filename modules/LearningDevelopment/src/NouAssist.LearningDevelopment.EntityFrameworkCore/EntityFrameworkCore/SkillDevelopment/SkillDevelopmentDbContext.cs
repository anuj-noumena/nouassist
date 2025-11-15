using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore
{
    [ConnectionStringName(SkillDevelopmentDbProperties.ConnectionStringName)]
    public class SkillDevelopmentDbContext 
        : AbpDbContext<SkillDevelopmentDbContext>, ISkillDevelopmentDbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillDevelopmentSession> SkillDevelopmentSessions { get; set; }
        public DbSet<SkillLog> SkillLogs { get; set; }
        public DbSet<SkillTree> SkillTrees { get; set; }
        public DbSet<ResourceHistory> ResourceHistories { get; set; }

        public SkillDevelopmentDbContext(DbContextOptions<SkillDevelopmentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSkillDevelopment(this);
        }
    }
}
