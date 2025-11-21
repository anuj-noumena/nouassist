using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents a milestone in a portfolio roadmap
    /// </summary>
    public class RoadmapMilestone : Entity<Guid>
    {
        public Guid RoadmapId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public MilestoneDate Date { get; private set; }
        public Guid? PortfolioItemId { get; private set; }
        public List<ResourceSkillRequirement> SkillRequirements { get; private set; }

        protected RoadmapMilestone() 
        {
            SkillRequirements = new List<ResourceSkillRequirement>();
        }

        public RoadmapMilestone(
            Guid id,
            Guid roadmapId,
            string title,
            string description,
            MilestoneDate date,
            Guid? portfolioItemId = null)
            : base(id)
        {
            RoadmapId = roadmapId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            Date = date;
            PortfolioItemId = portfolioItemId;
            SkillRequirements = new List<ResourceSkillRequirement>();
        }

        public void UpdateDetails(
            string title,
            string description,
            MilestoneDate date)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            Date = date;
        }

        public void MarkAsCompleted(DateTime completionDate)
        {
            Date.MarkAsCompleted(completionDate);
        }

        public void ResetCompletionStatus()
        {
            Date.ResetCompletion();
        }

        public void AddSkillRequirement(
            string skillName,
            int requiredLevel,
            bool isMandatory,
            string description = null)
        {
            var requirement = new ResourceSkillRequirement(
                skillName,
                requiredLevel,
                isMandatory,
                description);

            SkillRequirements.Add(requirement);
        }

        public void RemoveSkillRequirement(string skillName)
        {
            var requirement = SkillRequirements.FirstOrDefault(r => r.SkillName == skillName);
            if (requirement != null)
            {
                SkillRequirements.Remove(requirement);
            }
        }
    }
}
