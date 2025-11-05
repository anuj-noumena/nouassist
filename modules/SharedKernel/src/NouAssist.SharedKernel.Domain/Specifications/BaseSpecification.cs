using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace NouAssist.SharedKernel.Specifications;

public class BaseSpecification<T> : Specification<T>
{
    private readonly Expression<Func<T, bool>> _expression;

    public BaseSpecification()
    {
        _expression = t => true;
    }

    public BaseSpecification(Expression<Func<T, bool>> rule)
    {
        _expression = rule;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return _expression;
    }
}