using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements
{
    public interface ILegalAgreementAppService : IApplicationService
    {
        Task<LegalAgreementDto> GetAsync(Guid id);
        
        Task<PagedResultDto<LegalAgreementListDto>> GetListAsync(GetLegalAgreementListDto input);
        
        Task<LegalAgreementDto> CreateAsync(CreateLegalAgreementDto input);
        
        Task<LegalAgreementDto> UpdateAsync(Guid id, UpdateLegalAgreementDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<LegalAgreementDto> UpdateStatusAsync(Guid id, FilingStatusType newStatus, string notes = null);
        
        Task SetDocumentUrlAsync(Guid id, string documentUrl);
        
        Task<AgreementTermDto> AddTermAsync(Guid agreementId, CreateAgreementTermDto input);
        
        Task RemoveTermAsync(Guid agreementId, Guid termId);
        
        Task<AgreementTermDto> CompleteTermAsync(Guid agreementId, Guid termId);
        
        Task<AgreementReminderDto> AddReminderAsync(Guid agreementId, CreateAgreementReminderDto input);
        
        Task RemoveReminderAsync(Guid agreementId, Guid reminderId);
        
        Task<AgreementReminderDto> CompleteReminderAsync(Guid agreementId, Guid reminderId);
        
        Task<LegalAgreementDto> RenewAsync(Guid id, RenewAgreementDto input);
        
        Task<LegalAgreementDto> TerminateAsync(Guid id, TerminateAgreementDto input);
        
        Task<List<LegalAgreementListDto>> GetExpiringAgreementsAsync(int daysThreshold = 30);
        
        Task<List<LegalAgreementListDto>> GetActiveAgreementsAsync();
    }
}
