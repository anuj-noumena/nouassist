using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.SkillDevelopment.Entities
{
    public class ApplicationExample : Entity
    {
        public Guid MilestoneId { get; private set; }
        public Guid ExampleId { get; private set; }
        public string Description { get; private set; }
        public List<string> Outcomes { get; private set; }
        public int? ProficiencyImpact { get; private set; } // 0-100
        public DateTime CompletedAt { get; private set; }

        protected ApplicationExample() 
        { 
            Outcomes = new List<string>();
        }

        public ApplicationExample(
            Guid milestoneId, 
            string description, 
            DateTime completedAt)
        {
            MilestoneId = milestoneId;
            ExampleId = Guid.NewGuid();
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            CompletedAt = completedAt;
            Outcomes = new List<string>();
        }

        public void AddOutcome(string outcome)
        {
            if (!string.IsNullOrWhiteSpace(outcome))
                Outcomes.Add(outcome);
        }

        public void SetProficiencyImpact(int impact)
        {
            if (impact < 0 || impact > 100)
                throw new BusinessException("SkillDevelopment:InvalidImpact")
                    .WithData("Impact", impact);
            ProficiencyImpact = impact;
        }

        public override object[] GetKeys() => new object[] { MilestoneId, ExampleId };
    }
}
