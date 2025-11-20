using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.DocumentGeneration
{
    public interface IDocumentGenerationAppService : IApplicationService
    {
        Task<TemplateDto> GetTemplateAsync(Guid id);
        
        Task<ListResultDto<TemplateDto>> GetTemplatesAsync();
        
        Task<TemplateDto> CreateTemplateAsync(CreateTemplateDto input);
        
        Task<TemplateDto> UpdateTemplateAsync(Guid id, CreateTemplateDto input);
        
        Task DeleteTemplateAsync(Guid id);
        
        Task<string> GenerateDocumentAsync(GenerateDocumentDto input);
        
        Task<string> GenerateAgreementDocumentAsync(Guid agreementId, Guid templateId, Dictionary<string, string> additionalValues = null);
        
        Task<string> GenerateCorporateRecordDocumentAsync(Guid recordId, Guid templateId, Dictionary<string, string> additionalValues = null);
    }
}
