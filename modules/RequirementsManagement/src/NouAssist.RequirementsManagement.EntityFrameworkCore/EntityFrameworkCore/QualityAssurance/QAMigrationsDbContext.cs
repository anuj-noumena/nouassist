using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    public class QAMigrationsDbContext : AbpDbContext<QAMigrationsDbContext>
    {
        public QAMigrationsDbContext(DbContextOptions<QAMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all QA module configurations
            builder.ConfigureQA();
        }
    }
}
