using System;
using System.Linq.Expressions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.Base
{
    /// <summary>
    /// Base specification class for implementing the specification pattern
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// Gets the expression representing the specification
        /// </summary>
        /// <returns>Expression representing the specification</returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Checks if an entity satisfies the specification
        /// </summary>
        /// <param name="entity">Entity to check</param>
        /// <returns>True if the entity satisfies the specification</returns>
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        /// <summary>
        /// Combines this specification with another using logical AND
        /// </summary>
        /// <param name="specification">The specification to combine with</param>
        /// <returns>A new specification that is the logical AND of both specifications</returns>
        public Specification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Combines this specification with another using logical OR
        /// </summary>
        /// <param name="specification">The specification to combine with</param>
        /// <returns>A new specification that is the logical OR of both specifications</returns>
        public Specification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Negates this specification
        /// </summary>
        /// <returns>A new specification that is the logical NOT of this specification</returns>
        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    /// <summary>
    /// Specification representing the logical AND of two specifications
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
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

            var exprBody = Expression.AndAlso(
                Expression.Invoke(leftExpression, paramExpr),
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }

    /// <summary>
    /// Specification representing the logical OR of two specifications
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
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

            var exprBody = Expression.OrElse(
                Expression.Invoke(leftExpression, paramExpr),
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }

    /// <summary>
    /// Specification representing the logical NOT of a specification
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
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

            var exprBody = Expression.Not(
                Expression.Invoke(expression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }
}
