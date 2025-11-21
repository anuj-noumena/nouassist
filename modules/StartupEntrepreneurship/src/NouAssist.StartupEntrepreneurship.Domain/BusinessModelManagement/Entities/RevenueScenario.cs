using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a revenue scenario for simulation
    /// </summary>
    public class RevenueScenario : Entity<Guid>
    {
        public Guid RevenueModelId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ScenarioParameters Parameters { get; private set; }

        private readonly List<ProjectedRevenueData> _projections;
        public IReadOnlyCollection<ProjectedRevenueData> Projections => _projections.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime? LastSimulatedAt { get; private set; }

        protected RevenueScenario()
        {
            _projections = new List<ProjectedRevenueData>();
        }

        internal RevenueScenario(
            Guid id,
            Guid revenueModelId,
            string name,
            string description,
            ScenarioParameters parameters,
            Guid createdBy,
            DateTime createdAt) : base(id)
        {
            RevenueModelId = revenueModelId;
            SetName(name);
            Description = description;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            _projections = new List<ProjectedRevenueData>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: BusinessModelManagementConsts.RevenueModel.MaxScenarioNameLength
            );
        }

        public void UpdateParameters(ScenarioParameters newParameters)
        {
            Parameters = newParameters ?? throw new ArgumentNullException(nameof(newParameters));
            
            // Clear existing projections when parameters change
            _projections.Clear();
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void AddProjection(ProjectedRevenueData projection)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));

            // Remove existing projection for the same date
            var existingProjection = _projections.FirstOrDefault(p => p.ProjectionDate.Date == projection.ProjectionDate.Date);
            if (existingProjection != null)
            {
                _projections.Remove(existingProjection);
            }

            _projections.Add(projection);
        }

        public void ClearProjections()
        {
            _projections.Clear();
        }

        public void MarkAsSimulated(DateTime simulatedAt)
        {
            LastSimulatedAt = simulatedAt;
        }
    }
}
