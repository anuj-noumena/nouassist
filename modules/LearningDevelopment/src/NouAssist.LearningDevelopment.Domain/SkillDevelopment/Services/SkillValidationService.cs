using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Services
{
    public class SkillValidationService : DomainService
    {
        public MasteryValidationResult ValidateMasteryReadiness(
            Skill skill,
            SkillDevelopmentSession session,
            List<ApplicationExample> applications)
        {
            var result = new MasteryValidationResult { SkillId = skill.Id };

            // Check proficiency score
            if (skill.CurrentProficiency.Score < 90)
            {
                result.AddCriterion("ProficiencyScore", false,
                    $"Current proficiency ({skill.CurrentProficiency.Score}%) is below mastery threshold (90%)");
            }
            else
            {
                result.AddCriterion("ProficiencyScore", true,
                    "Proficiency score meets mastery requirements");
            }

            // Check session completion
            if (!session.Completed)
            {
                result.AddCriterion("SessionCompletion", false,
                    "Development session must be completed");
            }
            else
            {
                result.AddCriterion("SessionCompletion", true,
                    "All development steps completed");
            }

            // Check practice time
            var totalPracticeHours = session.GetTotalPracticeMinutes() / 60;
            var requiredHours = skill.EstimatedPracticeHours;
            
            if (totalPracticeHours < requiredHours * 0.8) // Allow 20% variance
            {
                result.AddCriterion("PracticeTime", false,
                    $"Insufficient practice time. Completed: {totalPracticeHours}h, Required: ~{requiredHours}h");
            }
            else
            {
                result.AddCriterion("PracticeTime", true,
                    $"Adequate practice time completed ({totalPracticeHours}h)");
            }

            // Check real-world applications
            if (applications.Count < 2)
            {
                result.AddCriterion("RealWorldApplications", false,
                    "At least 2 real-world applications required for mastery validation");
            }
            else
            {
                result.AddCriterion("RealWorldApplications", true,
                    $"{applications.Count} real-world applications completed");
            }

            // Check all development steps have quality milestones
            foreach (DevelopmentStep step in Enum.GetValues(typeof(DevelopmentStep)))
            {
                if (session.Milestones.TryGetValue(step, out var milestone))
                {
                    if (!milestone.Completed)
                    {
                        result.AddCriterion($"Step{(int)step}Quality", false,
                            $"{step} milestone not completed");
                    }
                }
            }

            result.CalculateReadinessScore();
            return result;
        }

        public bool CanStartDevelopmentSession(Skill skill, List<Skill> prerequisiteSkills)
        {
            // Check skill status
            if (!skill.CanStartDevelopment())
            {
                return false;
            }

            // Check prerequisites
            if (skill.Prerequisites.Any())
            {
                var prerequisiteIds = skill.Prerequisites.ToList();
                var providedIds = prerequisiteSkills.Select(s => s.Id).ToList();

                // All prerequisites must be provided and sufficiently proficient
                if (!prerequisiteIds.All(id => providedIds.Contains(id)))
                {
                    return false;
                }

                if (prerequisiteSkills.Any(s => s.CurrentProficiency.Score < 70))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class MasteryValidationResult
    {
        public Guid SkillId { get; set; }
        public Dictionary<string, (bool Passed, string Message)> Criteria { get; set; } = new();
        public int ReadinessScore { get; private set; } // 0-100
        public bool IsReadyForMastery => ReadinessScore >= 90;

        public void AddCriterion(string name, bool passed, string message)
        {
            Criteria[name] = (passed, message);
        }

        public void CalculateReadinessScore()
        {
            if (!Criteria.Any())
            {
                ReadinessScore = 0;
                return;
            }

            var passedCount = Criteria.Count(c => c.Value.Passed);
            ReadinessScore = (passedCount * 100) / Criteria.Count;
        }

        public List<string> GetFailedCriteria()
        {
            return Criteria
                .Where(c => !c.Value.Passed)
                .Select(c => $"{c.Key}: {c.Value.Message}")
                .ToList();
        }
    }
}
