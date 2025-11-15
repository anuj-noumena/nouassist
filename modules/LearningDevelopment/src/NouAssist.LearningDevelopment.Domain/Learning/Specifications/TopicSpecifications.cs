using System;
using System.Linq.Expressions;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Learning.Specifications
{
    public class ActiveTopicsSpecification : Specification<Topic>
    {
        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.Status == TopicStatus.Active;
    }

    public class CompletedTopicsSpecification : Specification<Topic>
    {
        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.Status == TopicStatus.Completed;
    }

    public class TopicsByDifficultySpecification : Specification<Topic>
    {
        private readonly DifficultyLevel _difficulty;

        public TopicsByDifficultySpecification(DifficultyLevel difficulty)
        {
            _difficulty = difficulty;
        }

        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.Difficulty == _difficulty;
    }

    public class TopicsByCategorySpecification : Specification<Topic>
    {
        private readonly string _category;

        public TopicsByCategorySpecification(string category)
        {
            _category = category;
        }

        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.Category == _category;
    }

    public class TopicsWithProgressAboveSpecification : Specification<Topic>
    {
        private readonly int _minimumProgress;

        public TopicsWithProgressAboveSpecification(int minimumProgress)
        {
            _minimumProgress = minimumProgress;
        }

        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.Progress >= _minimumProgress;
    }

    public class FolderTopicsSpecification : Specification<Topic>
    {
        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.IsFolder;
    }

    public class RootTopicsSpecification : Specification<Topic>
    {
        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.ParentId == null;
    }

    public class ChildTopicsSpecification : Specification<Topic>
    {
        private readonly Guid _parentId;

        public ChildTopicsSpecification(Guid parentId)
        {
            _parentId = parentId;
        }

        public override Expression<Func<Topic, bool>> ToExpression() =>
            t => t.ParentId == _parentId;
    }
}
