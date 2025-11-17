using System;
using System.Linq.Expressions;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Specifications
{
    public class ActiveSkillsSpecification : Specification<Skill>
    {
        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.Status == SkillStatus.InProgress ||
                 s.Status == SkillStatus.Practicing ||
                 s.Status == SkillStatus.Reviewing;
    }

    public class MasteredSkillsSpecification : Specification<Skill>
    {
        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.Status == SkillStatus.Mastered;
    }

    public class SkillsByProficiencyRangeSpecification : Specification<Skill>
    {
        private readonly int _minScore;
        private readonly int _maxScore;

        public SkillsByProficiencyRangeSpecification(int minScore, int maxScore)
        {
            _minScore = minScore;
            _maxScore = maxScore;
        }

        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.CurrentProficiency.Score >= _minScore &&
                 s.CurrentProficiency.Score <= _maxScore;
    }

    public class SkillsWithActiveDevelopmentSpecification : Specification<Skill>
    {
        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.ActiveDevelopmentSessionId != null &&
                 (s.Status == SkillStatus.InProgress ||
                  s.Status == SkillStatus.Practicing);
    }

    public class SkillsByCategorySpecification : Specification<Skill>
    {
        private readonly string _category;

        public SkillsByCategorySpecification(string category)
        {
            _category = category;
        }

        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.Category == _category;
    }

    public class CategorySkillsSpecification : Specification<Skill>
    {
        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.IsCategory;
    }

    public class SkillsNeedingReviewSpecification : Specification<Skill>
    {
        private readonly DateTime _lastReviewThreshold;

        public SkillsNeedingReviewSpecification(int daysSinceLastReview = 30)
        {
            _lastReviewThreshold = DateTime.UtcNow.AddDays(-daysSinceLastReview);
        }

        public override Expression<Func<Skill, bool>> ToExpression() =>
            s => s.Status == SkillStatus.Mastered &&
                 s.LastModificationTime < _lastReviewThreshold;
    }
}
