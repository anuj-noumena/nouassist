namespace NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base
{
    /// <summary>
    /// Base specification class for QA domain
    /// </summary>
    public abstract class QASpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public QASpecification<T> And(QASpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public QASpecification<T> Or(QASpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public QASpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public class AndSpecification<T> : QASpecification<T>
    {
        private readonly QASpecification<T> _left;
        private readonly QASpecification<T> _right;

        public AndSpecification(QASpecification<T> left, QASpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var bodyExpr = Expression.AndAlso(
                Expression.Invoke(leftExpression, paramExpr),
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }

    public class OrSpecification<T> : QASpecification<T>
    {
        private readonly QASpecification<T> _left;
        private readonly QASpecification<T> _right;

        public OrSpecification(QASpecification<T> left, QASpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var bodyExpr = Expression.OrElse(
                Expression.Invoke(leftExpression, paramExpr),
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }

    public class NotSpecification<T> : QASpecification<T>
    {
        private readonly QASpecification<T> _specification;

        public NotSpecification(QASpecification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var bodyExpr = Expression.Not(
                Expression.Invoke(expression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }
}
