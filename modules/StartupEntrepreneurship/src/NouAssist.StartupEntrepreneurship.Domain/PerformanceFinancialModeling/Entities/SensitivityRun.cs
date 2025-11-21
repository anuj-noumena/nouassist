using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents a sensitivity analysis run
    /// </summary>
    public class SensitivityRun : Entity<Guid>
    {
        public Guid ModelId { get; private set; }
        public Guid ScenarioId { get; private set; }
        public string Name { get; private set; }
        public DateTime RunDate { get; private set; }
        public string VariablesAnalyzed { get; private set; }
        public string Configuration { get; private set; }
        public string Results { get; private set; }
        public string Notes { get; private set; }

        protected SensitivityRun() { }

        internal SensitivityRun(Guid modelId, Guid scenarioId, string name, 
                              List<string> variablesAnalyzed, string configuration, 
                              string results, string notes = null) : base(Guid.NewGuid())
        {
            ModelId = modelId;
            ScenarioId = scenarioId;
            SetName(name);
            RunDate = DateTime.UtcNow;
            VariablesAnalyzed = string.Join(",", variablesAnalyzed);
            Configuration = configuration;
            Results = results;
            SetNotes(notes);
        }

        public void UpdateResults(string results, string notes = null)
        {
            Results = results;
            SetNotes(notes);
        }

        public List<string> GetAnalyzedVariables()
        {
            return VariablesAnalyzed?.Split(',').ToList() ?? new List<string>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PerformanceFinancialModelingConsts.MaxScenarioNameLength
            );
        }

        private void SetNotes(string notes)
        {
            if (!string.IsNullOrEmpty(notes))
            {
                Notes = Check.Length(
                    notes.Trim(),
                    nameof(notes),
                    maxLength: PerformanceFinancialModelingConsts.MaxNotesLength
                );
            }
        }
    }
}
