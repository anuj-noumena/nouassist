using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base
{
    /// <summary>
    /// Base specification class for CapTable domain
    /// </summary>
    public abstract class CapTableSpecification<T> : Specification<T>
    {
        public abstract override Expression<Func<T, bool>> ToExpression();

        public CapTableSpecification<T> And(CapTableSpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public CapTableSpecification<T> Or(CapTableSpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public CapTableSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public class AndSpecification<T> : CapTableSpecification<T>
    {
        private readonly CapTableSpecification<T> _left;
        private readonly CapTableSpecification<T> _right;

        public AndSpecification(CapTableSpecification<T> left, CapTableSpecification<T> right)
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

    public class OrSpecification<T> : CapTableSpecification<T>
    {
        private readonly CapTableSpecification<T> _left;
        private readonly CapTableSpecification<T> _right;

        public OrSpecification(CapTableSpecification<T> left, CapTableSpecification<T> right)
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

    public class NotSpecification<T> : CapTableSpecification<T>
    {
        private readonly CapTableSpecification<T> _specification;

        public NotSpecification(CapTableSpecification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var paramExpr = expression.Parameters.Single();
            var bodyExpr = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }
}
