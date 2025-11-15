using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.SkillDevelopment.Services
{
    public class SkillProgressionService : DomainService
    {
        public SkillProgressionResult ProgressSkill(
            Skill skill,
            SkillDevelopmentSession session,
            List<Skill> prerequisiteSkills,
            Guid userId)
        {
            var result = new SkillProgressionResult();
            
            // Validate prerequisites
            var validation = ValidatePrerequisites(skill, prerequisiteSkills);
            if (!validation.IsValid)
            {
                result.Errors = validation.Errors;
                return result;
            }

            // Calculate proficiency gain based on session progress
            var proficiencyGain = CalculateProficiencyGain(session);
            var newProficiencyScore = Math.Min(100, skill.CurrentProficiency.Score + proficiencyGain);
            
            // Update skill proficiency
            skill.UpdateProficiency(
                newProficiencyScore,
                $"Progressed through {session.CurrentStep}. Gain: {proficiencyGain} points.",
                userId);

            result.IsSuccessful = true;
            result.UpdatedSkill = skill;
            result.ProficiencyGain = proficiencyGain;
            result.NewProficiencyScore = newProficiencyScore;

            return result;
        }

        public ValidationResult ValidatePrerequisites(Skill skill, List<Skill> prerequisiteSkills)
        {
            var result = new ValidationResult();
            
            if (!skill.Prerequisites.Any())
                return result; // No prerequisites

            var prerequisiteIds = skill.Prerequisites.ToList();
            var providedIds = prerequisiteSkills.Select(s => s.Id).ToList();

            // Check all prerequisites are provided
            var missingPrerequisites = prerequisiteIds.Except(providedIds).ToList();
            if (missingPrerequisites.Any())
            {
                result.AddError($"Missing prerequisite skills: {string.Join(", ", missingPrerequisites)}");
                return result;
            }

            // Check each prerequisite is mastered or at high proficiency
            foreach (var prereqSkill in prerequisiteSkills)
            {
                if (prereqSkill.CurrentProficiency.Score < 70)
                {
                    result.AddError(
                        $"Prerequisite skill '{prereqSkill.Title}' has insufficient proficiency " +
                        $"({prereqSkill.CurrentProficiency.Score}%). Minimum 70% required.");
                }
            }

            return result;
        }

        private int CalculateProficiencyGain(SkillDevelopmentSession session)
        {
            if (session.Completed)
                return session.OverallProficiencyGain ?? 0;

            // Calculate based on current progress
            var completedSteps = session.Milestones.Count(m => m.Value.Completed);
            var totalPracticeMinutes = session.GetTotalPracticeMinutes();
            
            // Base gain: 5 points per completed step
            var baseGain = completedSteps * 5;
            
            // Practice bonus: 1 point per 30 minutes of practice (max 15 points)
            var practiceBonus = Math.Min(15, totalPracticeMinutes / 30);
            
            // Application bonus: 5 points per real-world application (max 10 points)
            var applicationBonus = Math.Min(10, session.GetTotalApplicationsCount() * 5);
            
            return baseGain + practiceBonus + applicationBonus;
        }

        public PracticePlanRecommendation GeneratePracticePlan(
            Skill skill,
            ProficiencyLevel currentLevel,
            ProficiencyLevel targetLevel,
            int availableHoursPerWeek)
        {
            var recommendation = new PracticePlanRecommendation
            {
                SkillId = skill.Id,
                CurrentLevel = currentLevel,
                TargetLevel = targetLevel,
                EstimatedWeeks = CalculateEstimatedWeeks(currentLevel, targetLevel, availableHoursPerWeek)
            };

            // Generate practice items based on level gap
            var levelGap = (int)targetLevel - (int)currentLevel;
            var practiceItems = new List<PracticePlanItem>();

            for (int i = 0; i < levelGap; i++)
            {
                var currentPhaseLevel = (ProficiencyLevel)((int)currentLevel + i + 1);
                practiceItems.AddRange(GeneratePracticeItemsForLevel(skill, currentPhaseLevel));
            }

            recommendation.PracticeItems = practiceItems;
            return recommendation;
        }

        private int CalculateEstimatedWeeks(
            ProficiencyLevel current,
            ProficiencyLevel target,
            int hoursPerWeek)
        {
            var levelGap = (int)target - (int)current;
            var totalHoursNeeded = levelGap * 40; // 40 hours per proficiency level
            return (int)Math.Ceiling((double)totalHoursNeeded / hoursPerWeek);
        }

        private List<PracticePlanItem> GeneratePracticeItemsForLevel(
            Skill skill,
            ProficiencyLevel level)
        {
            var items = new List<PracticePlanItem>();
            
            switch (level)
            {
                case ProficiencyLevel.Beginner:
                    items.Add(new PracticePlanItem(
                        "Study fundamentals and basic concepts",
                        120,
                        $"Understand core principles of {skill.Title}",
                        skill.Resources.ToList()));
                    items.Add(new PracticePlanItem(
                        "Complete introductory exercises",
                        180,
                        "Practice basic techniques"));
                    break;
                    
                case ProficiencyLevel.Intermediate:
                    items.Add(new PracticePlanItem(
                        "Work on intermediate projects",
                        300,
                        "Apply skills in realistic scenarios"));
                    items.Add(new PracticePlanItem(
                        "Learn advanced concepts",
                        240,
                        "Deepen theoretical understanding"));
                    break;
                    
                case ProficiencyLevel.Advanced:
                    items.Add(new PracticePlanItem(
                        "Build complex projects",
                        480,
                        "Demonstrate mastery through substantial work"));
                    items.Add(new PracticePlanItem(
                        "Study expert-level resources",
                        360,
                        "Understand nuances and edge cases"));
                    break;
                    
                case ProficiencyLevel.Expert:
                    items.Add(new PracticePlanItem(
                        "Contribute to open-source or teach others",
                        600,
                        "Share expertise and gain recognition"));
                    items.Add(new PracticePlanItem(
                        "Innovate and create original work",
                        720,
                        "Push boundaries of the skill"));
                    break;
            }

            return items;
        }
    }

    // Supporting result classes
    public class SkillProgressionResult
    {
        public bool IsSuccessful { get; set; }
        public Skill UpdatedSkill { get; set; }
        public int ProficiencyGain { get; set; }
        public int NewProficiencyScore { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }

    public class PracticePlanRecommendation
    {
        public Guid SkillId { get; set; }
        public ProficiencyLevel CurrentLevel { get; set; }
        public ProficiencyLevel TargetLevel { get; set; }
        public int EstimatedWeeks { get; set; }
        public List<PracticePlanItem> PracticeItems { get; set; }
    }
}
