using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas
{
    public interface IBusinessModelCanvasAppService : IApplicationService
    {
        Task<BusinessModelCanvasDto> GetAsync(Guid id);
        Task<PagedResultDto<BusinessModelCanvasDto>> GetListAsync(GetBusinessModelCanvasListDto input);
        Task<BusinessModelCanvasDto> CreateAsync(CreateBusinessModelCanvasDto input);
        Task<BusinessModelCanvasDto> UpdateAsync(Guid id, UpdateBusinessModelCanvasDto input);
        Task DeleteAsync(Guid id);

        // Version Management
        Task<CanvasVersionDto> CreateVersionAsync(Guid canvasId, CreateCanvasVersionDto input);
        Task<List<CanvasVersionDto>> GetVersionsAsync(Guid canvasId);
        Task<BusinessModelCanvasDto> SwitchToVersionAsync(Guid canvasId, int versionNumber);

        // Component Management
        Task<CanvasComponentDto> GetComponentAsync(Guid canvasId, CanvasComponentType componentType);
        Task<CanvasComponentDto> UpdateComponentAsync(Guid canvasId, UpdateCanvasComponentDto input);
        Task<List<CanvasComponentDto>> GetComponentsAsync(Guid canvasId, int? versionNumber = null);

        // Hypothesis Management
        Task<HypothesisDto> CreateHypothesisAsync(Guid canvasId, CreateHypothesisDto input);
        Task ValidateHypothesisAsync(Guid canvasId, Guid hypothesisId, ValidateHypothesisDto input);
        Task<List<HypothesisDto>> GetHypothesesAsync(Guid canvasId, CanvasComponentType? componentType = null);
        Task<List<HypothesisDto>> GetPendingHypothesesAsync(Guid canvasId);

        // Analysis and Suggestions
        Task<double> GetValidationProgressAsync(Guid canvasId);
        Task<List<string>> GetContentSuggestionsAsync(Guid canvasId, CanvasComponentType componentType);
        Task<object> AnalyzeCompletenessAsync(Guid canvasId);
    }
}
