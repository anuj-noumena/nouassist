namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions
{
    /// <summary>
    /// Base exception for Product Discovery domain
    /// </summary>
    public class ProductDiscoveryDomainException : BusinessException
    {
        public ProductDiscoveryDomainException(string code)
            : base(code)
        {
        }

        public ProductDiscoveryDomainException(string code, string message)
            : base(code, message)
        {
        }

        public ProductDiscoveryDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
