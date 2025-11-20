namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Specifications.Base
{
    /// <summary>
    /// Base implementation of the specification pattern
    /// </summary>
    public abstract class Specification<T>
    {
        /// <summary>
        /// Converts the specification to an expression
        /// </summary>
        /// <returns>An expression representing the specification</returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Determines if an entity satisfies the specification
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>True if the entity satisfies the specification</returns>
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        /// <summary>
        /// Combines this specification with another using AND
        /// </summary>
        /// <param name="specification">The specification to combine with</param>
        /// <returns>A new specification representing the AND combination</returns>
        public Specification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Combines this specification with another using OR
        /// </summary>
        /// <param name="specification">The specification to combine with</param>
        /// <returns>A new specification representing the OR combination</returns>
        public Specification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Negates this specification
        /// </summary>
        /// <returns>A new specification representing the negation</returns>
        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    /// <summary>
    /// Combines two specifications using AND
    /// </summary>
    /// <typeparam name="T">The type of entity the specification applies to</typeparam>
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

    /// <summary>
    /// Combines two specifications using OR
    /// </summary>
    /// <typeparam name="T">The type of entity the specification applies to</typeparam>
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

    /// <summary>
    /// Negates a specification
    /// </summary>
    /// <typeparam name="T">The type of entity the specification applies to</typeparam>
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
