namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Specifications.Base
{
    /// <summary>
    /// Base implementation of the specification pattern
    /// </summary>
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public Specification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
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

    public class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
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

    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var bodyExpr = Expression.Not(Expression.Invoke(expression, paramExpr));

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }
}
