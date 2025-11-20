using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    // Apply connection string name specific to this module's DB context
    [ConnectionStringName("QA")]
    public interface IQADbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root in this module
        DbSet<TestPlan> TestPlans { get; }
        DbSet<TestCycle> TestCycles { get; }
        DbSet<DefectReport> DefectReports { get; }
        DbSet<QualityMetricSnapshot> QualityMetricSnapshots { get; }
    }
}
