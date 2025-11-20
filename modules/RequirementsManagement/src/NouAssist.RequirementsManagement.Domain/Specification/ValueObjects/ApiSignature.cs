namespace NouAssist.RequirementsManagement.Specification.ValueObjects
{
    /// <summary>
    /// Value object representing an API signature
    /// </summary>
    public class ApiSignature : ValueObject
    {
        public string InterfaceName { get; private set; }
        public string Endpoint { get; private set; }
        public string HttpMethod { get; private set; }
        public InterfaceType Type { get; private set; }
        public string Description { get; private set; }

        protected ApiSignature() { }

        public ApiSignature(
            string interfaceName,
            string endpoint,
            string httpMethod,
            InterfaceType type,
            string description = null)
        {
            SetInterfaceName(interfaceName);
            SetEndpoint(endpoint);
            SetHttpMethod(httpMethod);
            Type = type;
            SetDescription(description);
        }

        private void SetInterfaceName(string interfaceName)
        {
            InterfaceName = Check.NotNullOrWhiteSpace(
                interfaceName,
                nameof(interfaceName),
                maxLength: RequirementSpecificationManagementConsts.ExternalInterface.MaxNameLength
            );
        }

        private void SetEndpoint(string endpoint)
        {
            Endpoint = Check.NotNullOrWhiteSpace(
                endpoint,
                nameof(endpoint),
                maxLength: RequirementSpecificationManagementConsts.ExternalInterface.MaxEndpointLength
            );
        }

        private void SetHttpMethod(string httpMethod)
        {
            HttpMethod = Check.NotNullOrWhiteSpace(httpMethod, nameof(httpMethod));
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: RequirementSpecificationManagementConsts.ExternalInterface.MaxDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InterfaceName;
            yield return Endpoint;
            yield return HttpMethod;
            yield return Type;
            yield return Description;
        }
    }
}
