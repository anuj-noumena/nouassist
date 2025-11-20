namespace NouAssist.RequirementsManagement.Specification.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for formatting specifications
    /// </summary>
    public interface ISpecificationFormattingService : IDomainService
    {
        Task<string> FormatRequirementAsync(Requirement requirement, string format = "html");
        Task<string> FormatSpecificationDocumentAsync(SpecificationStructure structure, List<Requirement> requirements, string format = "html");
        Task<byte[]> GenerateDocumentAsync(SpecificationStructure structure, List<Requirement> requirements, string format = "pdf");
    }
}
