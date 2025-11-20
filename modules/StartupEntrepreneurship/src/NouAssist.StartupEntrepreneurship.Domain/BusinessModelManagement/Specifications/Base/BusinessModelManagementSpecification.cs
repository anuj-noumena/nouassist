namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base
{
    /// <summary>
    /// Base specification class for Business Model Management domain
    /// </summary>
    public abstract class BusinessModelManagementSpecification<T> : Specification<T>
    {
        protected BusinessModelManagementSpecification() { }

        public BusinessModelManagementSpecification<T> And(BusinessModelManagementSpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public BusinessModelManagementSpecification<T> Or(BusinessModelManagementSpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public BusinessModelManagementSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
