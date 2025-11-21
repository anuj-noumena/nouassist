using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    public static class QADbContextModelCreatingExtensions
    {
        public static void ConfigureQA(
            this ModelBuilder builder,
            Action<QAModelBuilderConfigurationOptions>? optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new QAModelBuilderConfigurationOptions(
                QADbProperties.DbTablePrefix,
                QADbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(QADbContext).Assembly);
        }
    }

    public class QAModelBuilderConfigurationOptions
    {
        public string TablePrefix { get; set; }
        public string? Schema { get; set; }

        public QAModelBuilderConfigurationOptions(string tablePrefix = "", string? schema = null)
        {
            TablePrefix = tablePrefix;
            Schema = schema;
        }
    }
}
