using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Localization;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Managers;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Permissions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas
{
    [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.Default)]
    public class BusinessModelCanvasAppService : ApplicationService, IBusinessModelCanvasAppService
    {
        private readonly IBusinessModelCanvasRepository _canvasRepository;
        private readonly BusinessModelCanvasManager _canvasManager;
        private readonly ICanvasEditorService _canvasEditorService;
        private readonly IHypothesisTrackingService _hypothesisTrackingService;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public BusinessModelCanvasAppService(
            IBusinessModelCanvasRepository canvasRepository,
            BusinessModelCanvasManager canvasManager,
            ICanvasEditorService canvasEditorService,
            IHypothesisTrackingService hypothesisTrackingService,
            ICurrentUser currentUser,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _canvasRepository = canvasRepository;
            _canvasManager = canvasManager;
            _canvasEditorService = canvasEditorService;
            _hypothesisTrackingService = hypothesisTrackingService;
            _currentUser = currentUser;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(BusinessModelManagementResource);
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<BusinessModelCanvasDto> GetAsync(Guid id)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithVersionsAndComponentsAsync(id);
                
                // Check tenant/ownership access
                await CheckCanvasAccessAsync(canvas);

                return await MapCanvasToDtoAsync(canvas);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<PagedResultDto<BusinessModelCanvasDto>> GetListAsync(GetBusinessModelCanvasListDto input)
        {
            try
            {
                var query = await _canvasRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Title.Contains(input.Filter) || x.Description.Contains(input.Filter));

                if (input.OwnerId.HasValue)
                {
                    query = query.Where(x => x.OwnerId == input.OwnerId.Value);
                }

                if (input.ValidationStatus.HasValue)
                {
                    query = query.Where(x => x.Components.Any(c => c.ValidationState.Status == input.ValidationStatus.Value));
                }

                if (input.CreatedAfter.HasValue)
                {
                    query = query.Where(x => x.CreationTime >= input.CreatedAfter.Value);
                }

                if (input.CreatedBefore.HasValue)
                {
                    query = query.Where(x => x.CreationTime <= input.CreatedBefore.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var sorting = input.Sorting.IsNullOrWhiteSpace() ? nameof(Aggregates.BusinessModelCanvas.CreationTime) + " desc" : input.Sorting;
                var items = await AsyncExecuter.ToListAsync(
                    query.OrderBy(sorting).PageBy(input));

                var dtos = new List<BusinessModelCanvasDto>();
                foreach (var item in items)
                {
                    dtos.Add(await MapCanvasToDtoAsync(item));
                }

                return new PagedResultDto<BusinessModelCanvasDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.Create)]
        public virtual async Task<BusinessModelCanvasDto> CreateAsync(CreateBusinessModelCanvasDto input)
        {
            try
            {
                // Use the new BusinessModelCanvasManager for creation with validation
                var canvas = await _canvasManager.CreateAsync(
                    input.Title,
                    input.Description,
                    _currentUser.GetId()
                );

                // Persist the canvas
                await _canvasRepository.InsertAsync(canvas, autoSave: true);

                return await MapCanvasToDtoAsync(canvas);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.Update)]
        public virtual async Task<BusinessModelCanvasDto> UpdateAsync(Guid id, UpdateBusinessModelCanvasDto input)
        {
            try
            {
                var canvas = await _canvasRepository.GetAsync(id);
                await CheckCanvasAccessAsync(canvas);

                // Use the manager for update with validation
                await _canvasManager.UpdateAsync(id, input.Title, input.Description);

                return await MapCanvasToDtoAsync(canvas);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var canvas = await _canvasRepository.GetAsync(id);
                await CheckCanvasAccessAsync(canvas);

                // Use the manager for delete with business rules validation
                await _canvasManager.DeleteAsync(id, hardDelete: false);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.CreateVersion)]
        public virtual async Task<CanvasVersionDto> CreateVersionAsync(Guid canvasId, CreateCanvasVersionDto input)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithVersionsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var version = await _canvasEditorService.CreateVersionAsync(
                    canvas,
                    input.VersionName,
                    input.Notes,
                    _currentUser.GetId(),
                    _clock.Now
                );

                return ObjectMapper.Map<Entities.CanvasVersion, CanvasVersionDto>(version);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<List<CanvasVersionDto>> GetVersionsAsync(Guid canvasId)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithVersionsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                return canvas.Versions
                    .OrderByDescending(v => v.VersionNumber)
                    .Select(v => ObjectMapper.Map<Entities.CanvasVersion, CanvasVersionDto>(v))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.Update)]
        public virtual async Task<BusinessModelCanvasDto> SwitchToVersionAsync(Guid canvasId, int versionNumber)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithVersionsAndComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                canvas.SwitchToVersion(versionNumber);
                await _canvasRepository.UpdateAsync(canvas);

                return await MapCanvasToDtoAsync(canvas);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<CanvasComponentDto> GetComponentAsync(Guid canvasId, CanvasComponentType componentType)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var component = canvas.GetComponent(componentType);
                if (component == null)
                {
                    throw new UserFriendlyException(L["ComponentNotFound", componentType]);
                }

                return ObjectMapper.Map<Entities.CanvasComponent, CanvasComponentDto>(component);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.ManageComponents)]
        public virtual async Task<CanvasComponentDto> UpdateComponentAsync(Guid canvasId, UpdateCanvasComponentDto input)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var content = ObjectMapper.Map<ComponentContentDto, ComponentContent>(input.Content);
                
                await _canvasEditorService.ValidateComponentContentAsync(input.ComponentType, content);

                canvas.UpdateComponent(input.ComponentType, content, _currentUser.GetId(), _clock.Now);
                await _canvasRepository.UpdateAsync(canvas);

                var component = canvas.GetComponent(input.ComponentType);
                return ObjectMapper.Map<Entities.CanvasComponent, CanvasComponentDto>(component);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<List<CanvasComponentDto>> GetComponentsAsync(Guid canvasId, int? versionNumber = null)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                IReadOnlyCollection<Entities.CanvasComponent> components;
                if (versionNumber.HasValue)
                {
                    components = canvas.GetComponentsForVersion(versionNumber.Value);
                }
                else
                {
                    components = canvas.Components.Where(c => c.VersionId == canvas.CurrentVersionId).ToList();
                }

                return components
                    .Select(c => ObjectMapper.Map<Entities.CanvasComponent, CanvasComponentDto>(c))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.ManageHypotheses)]
        public virtual async Task<HypothesisDto> CreateHypothesisAsync(Guid canvasId, CreateHypothesisDto input)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var hypothesis = await _hypothesisTrackingService.CreateHypothesisAsync(
                    canvas,
                    input.ComponentType,
                    input.Assumption,
                    _currentUser.GetId(),
                    _clock.Now
                );

                return ObjectMapper.Map<Entities.AssociatedHypothesis, HypothesisDto>(hypothesis);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.ManageHypotheses)]
        public virtual async Task ValidateHypothesisAsync(Guid canvasId, Guid hypothesisId, ValidateHypothesisDto input)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var testStartDate = input.TestStartDate ?? _clock.Now.AddDays(-7);
                var testEndDate = input.TestEndDate ?? _clock.Now;

                canvas.ValidateHypothesis(
                    hypothesisId,
                    input.TestDescription,
                    input.TestResult,
                    input.FinalStatus,
                    testStartDate,
                    testEndDate
                );

                await _canvasRepository.UpdateAsync(canvas);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<List<HypothesisDto>> GetHypothesesAsync(Guid canvasId, CanvasComponentType? componentType = null)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var hypotheses = canvas.Components
                    .Where(c => c.VersionId == canvas.CurrentVersionId)
                    .WhereIf(componentType.HasValue, c => c.ComponentType == componentType.Value)
                    .SelectMany(c => c.Hypotheses)
                    .ToList();

                return hypotheses
                    .Select(h => ObjectMapper.Map<Entities.AssociatedHypothesis, HypothesisDto>(h))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<List<HypothesisDto>> GetPendingHypothesesAsync(Guid canvasId)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var pendingHypotheses = await _hypothesisTrackingService.GetPendingHypothesesAsync(canvas);

                return pendingHypotheses
                    .Select(h => ObjectMapper.Map<Entities.AssociatedHypothesis, HypothesisDto>(h))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<double> GetValidationProgressAsync(Guid canvasId)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                return await _hypothesisTrackingService.CalculateValidationProgressAsync(canvas);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<List<string>> GetContentSuggestionsAsync(Guid canvasId, CanvasComponentType componentType)
        {
            try
            {
                var canvas = await _canvasRepository.GetAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                return await _canvasEditorService.GenerateContentSuggestionsAsync(componentType, canvas);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.BusinessModelCanvas.View)]
        public virtual async Task<object> AnalyzeCompletenessAsync(Guid canvasId)
        {
            try
            {
                var canvas = await _canvasRepository.GetWithComponentsAsync(canvasId);
                await CheckCanvasAccessAsync(canvas);

                var result = await _canvasEditorService.AnalyzeCanvasCompletenessAsync(canvas);
                
                return new
                {
                    IsValid = result.IsValid,
                    Errors = result.Errors,
                    Warnings = result.Warnings,
                    ValidationProgress = canvas.GetOverallValidationProgress(),
                    ComponentsAnalyzed = canvas.Components.Count,
                    ValidatedComponents = canvas.Components.Count(c => c.ValidationState.Status == ValidationStatus.Validated)
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper methods
        private async Task CheckCanvasAccessAsync(Aggregates.BusinessModelCanvas canvas)
        {
            // Check ownership or tenant access
            if (canvas.OwnerId != _currentUser.GetId() && canvas.TenantId != CurrentTenant.Id)
            {
                throw new UserFriendlyException(L["AccessDenied"]);
            }
        }

        private async Task<BusinessModelCanvasDto> MapCanvasToDtoAsync(Aggregates.BusinessModelCanvas canvas)
        {
            var dto = ObjectMapper.Map<Aggregates.BusinessModelCanvas, BusinessModelCanvasDto>(canvas);
            dto.ValidationProgress = canvas.GetOverallValidationProgress();
            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in {ServiceName}: {ErrorMessage}", 
                GetType().FullName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
