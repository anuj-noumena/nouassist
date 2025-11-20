using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels
{
    [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Default)]
    public class FinancialModelAppService : ApplicationService, IFinancialModelAppService
    {
        private readonly IFinancialModelRepository _financialModelRepository;
        private readonly IFinancialModelingEngine _financialModelingEngine;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public FinancialModelAppService(
            IFinancialModelRepository financialModelRepository,
            IFinancialModelingEngine financialModelingEngine,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _financialModelRepository = financialModelRepository;
            _financialModelingEngine = financialModelingEngine;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<FinancialModelDto> GetAsync(Guid id)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id, includeDetails: false);
                CheckResourceAccess(model);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<FinancialModelDetailDto> GetDetailAsync(Guid id)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id, includeDetails: true);
                CheckResourceAccess(model);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDetailDto>(model);
                await EnrichFinancialModelDetailDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<PagedResultDto<FinancialModelDto>> GetListAsync(GetFinancialModelListDto input)
        {
            try
            {
                var query = await _financialModelRepository.GetQueryableAsync();

                // Apply tenant filter
                query = query.Where(m => m.TenantId == _currentTenant.Id);

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

                query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
                query = query.WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
                query = query.WhereIf(input.CreatedAfter.HasValue, x => x.CreationTime >= input.CreatedAfter.Value);
                query = query.WhereIf(input.CreatedBefore.HasValue, x => x.CreationTime <= input.CreatedBefore.Value);
                query = query.WhereIf(input.CanCalculate.HasValue, 
                    x => input.CanCalculate.Value ? x.Scenarios.Any() && x.Assumptions.Any() : true);

                // Apply sorting
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    query = query.OrderBy(input.Sorting);
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreationTime);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply paging
                var entities = await AsyncExecuter.ToListAsync(
                    query.PageBy(input)
                );

                // Map to DTOs
                var dtos = ObjectMapper.Map<List<FinancialModel>, List<FinancialModelDto>>(entities);

                // Enrich DTOs
                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var model = entities.First(e => e.Id == dto.Id);
                    await EnrichFinancialModelDto(dto, model);
                }));

                return new PagedResultDto<FinancialModelDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Create)]
        public virtual async Task<FinancialModelDto> CreateAsync(CreateFinancialModelDto input)
        {
            try
            {
                // Check for duplicates
                var existingModel = await _financialModelRepository.FindByNameAsync(input.Name, _currentTenant.Id);
                if (existingModel != null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateModelName", input.Name]);
                }

                // Create the model
                var modelId = _guidGenerator.Create();
                var model = FinancialModel.Create(
                    modelId,
                    input.Name,
                    input.Description,
                    input.ProjectionYears,
                    input.BaseDate,
                    input.Currency,
                    _currentTenant.Id
                );

                await _financialModelRepository.InsertAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Update)]
        public virtual async Task<FinancialModelDto> UpdateAsync(Guid id, UpdateFinancialModelDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id);
                CheckResourceAccess(model);

                // Check for duplicate names (excluding current model)
                var existingModel = await _financialModelRepository.FindByNameAsync(input.Name, _currentTenant.Id);
                if (existingModel != null && existingModel.Id != id)
                {
                    throw new UserFriendlyException(L["ErrorMessage:DuplicateModelName", input.Name]);
                }

                model.UpdateBasicInfo(input.Name, input.Description, input.ProjectionYears);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id);
                CheckResourceAccess(model);

                await _financialModelRepository.DeleteAsync(model, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Update)]
        public virtual async Task<FinancialModelDto> ChangeStatusAsync(Guid id, ChangeModelStatusDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id);
                CheckResourceAccess(model);

                model.ChangeStatus(input.Status);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Update)]
        public virtual async Task<FinancialModelDto> ActivateAsync(Guid id)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id);
                CheckResourceAccess(model);

                model.Activate();
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Update)]
        public virtual async Task<FinancialModelDto> DeactivateAsync(Guid id)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(id);
                CheckResourceAccess(model);

                model.Deactivate();
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<FinancialModel, FinancialModelDto>(model);
                await EnrichFinancialModelDto(dto, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task<ModelScenarioDto> CreateScenarioAsync(Guid modelId, CreateModelScenarioDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.AddScenario(input.Name, input.Type, input.Description);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<ModelScenario, ModelScenarioDto>(scenario);
                await EnrichModelScenarioDto(dto, scenario, model);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task<ModelScenarioDto> UpdateScenarioAsync(Guid scenarioId, UpdateModelScenarioDto input)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Scenarios.Any(s => s.Id == scenarioId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                scenario.UpdateInfo(input.Name, input.Description);

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<ModelScenario, ModelScenarioDto>(scenario);
                await EnrichModelScenarioDto(dto, scenario, model);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task DeleteScenarioAsync(Guid scenarioId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Scenarios.Any(s => s.Id == scenarioId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                CheckResourceAccess(model);

                model.RemoveScenario(scenarioId);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task<ModelScenarioDto> ActivateScenarioAsync(Guid scenarioId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Scenarios.Any(s => s.Id == scenarioId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                scenario.Activate();

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<ModelScenario, ModelScenarioDto>(scenario);
                await EnrichModelScenarioDto(dto, scenario, model);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task<ModelScenarioDto> DeactivateScenarioAsync(Guid scenarioId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Scenarios.Any(s => s.Id == scenarioId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                scenario.Deactivate();

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<ModelScenario, ModelScenarioDto>(scenario);
                await EnrichModelScenarioDto(dto, scenario, model);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<List<ModelScenarioDto>> GetScenariosAsync(Guid modelId)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var dtos = ObjectMapper.Map<List<ModelScenario>, List<ModelScenarioDto>>(model.Scenarios.ToList());

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var scenario = model.Scenarios.First(s => s.Id == dto.Id);
                    await EnrichModelScenarioDto(dto, scenario, model);
                }));

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageScenarios)]
        public virtual async Task AssociateAssumptionSetsAsync(Guid scenarioId, AssociateAssumptionSetsDto input)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Scenarios.Any(s => s.Id == scenarioId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);

                // Clear existing associations
                foreach (var existingId in scenario.AssociatedAssumptionSets.ToList())
                {
                    scenario.DisassociateAssumptionSet(existingId);
                }

                // Add new associations
                foreach (var assumptionSetId in input.AssumptionSetIds)
                {
                    // Validate assumption set exists in the model
                    var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                    if (assumptionSet == null)
                    {
                        throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                    }

                    scenario.AssociateAssumptionSet(assumptionSetId);
                }

                await _financialModelRepository.UpdateAsync(model, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task<AssumptionSetDto> CreateAssumptionSetAsync(Guid modelId, CreateAssumptionSetDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var assumptionValues = ObjectMapper.Map<List<AssumptionValueDto>, List<AssumptionValue>>(input.Assumptions);
                var assumptionSet = model.AddAssumptionSet(input.Name, input.Description);

                foreach (var assumption in assumptionValues)
                {
                    assumptionSet.AddAssumption(assumption);
                }

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<AssumptionSet, AssumptionSetDto>(assumptionSet);
                await EnrichAssumptionSetDto(dto, assumptionSet);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task<AssumptionSetDto> UpdateAssumptionSetAsync(Guid assumptionSetId, UpdateAssumptionSetDto input)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Assumptions.Any(a => a.Id == assumptionSetId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                }

                CheckResourceAccess(model);

                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                assumptionSet.UpdateInfo(input.Name, input.Description);

                // Update assumptions
                var assumptionValues = ObjectMapper.Map<List<AssumptionValueDto>, List<AssumptionValue>>(input.Assumptions);
                
                // Clear existing assumptions
                foreach (var existingAssumption in assumptionSet.Assumptions.ToList())
                {
                    assumptionSet.RemoveAssumption(existingAssumption.Name);
                }

                // Add new assumptions
                foreach (var assumption in assumptionValues)
                {
                    assumptionSet.AddAssumption(assumption);
                }

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<AssumptionSet, AssumptionSetDto>(assumptionSet);
                await EnrichAssumptionSetDto(dto, assumptionSet);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task DeleteAssumptionSetAsync(Guid assumptionSetId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Assumptions.Any(a => a.Id == assumptionSetId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                }

                CheckResourceAccess(model);

                model.RemoveAssumptionSet(assumptionSetId);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task<AssumptionSetDto> ActivateAssumptionSetAsync(Guid assumptionSetId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Assumptions.Any(a => a.Id == assumptionSetId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                }

                CheckResourceAccess(model);

                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                assumptionSet.Activate();

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<AssumptionSet, AssumptionSetDto>(assumptionSet);
                await EnrichAssumptionSetDto(dto, assumptionSet);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task<AssumptionSetDto> DeactivateAssumptionSetAsync(Guid assumptionSetId)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Assumptions.Any(a => a.Id == assumptionSetId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                }

                CheckResourceAccess(model);

                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                assumptionSet.Deactivate();

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<AssumptionSet, AssumptionSetDto>(assumptionSet);
                await EnrichAssumptionSetDto(dto, assumptionSet);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<List<AssumptionSetDto>> GetAssumptionSetsAsync(Guid modelId)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var dtos = ObjectMapper.Map<List<AssumptionSet>, List<AssumptionSetDto>>(model.Assumptions.ToList());

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var assumptionSet = model.Assumptions.First(a => a.Id == dto.Id);
                    await EnrichAssumptionSetDto(dto, assumptionSet);
                }));

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.ManageAssumptions)]
        public virtual async Task<AssumptionSetDto> UpdateAssumptionAsync(Guid assumptionSetId, UpdateAssumptionDto input)
        {
            try
            {
                var models = await _financialModelRepository.GetListAsync(
                    await _financialModelRepository.WithDetailsAsync()
                );

                var model = models.FirstOrDefault(m => m.Assumptions.Any(a => a.Id == assumptionSetId));
                if (model == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:AssumptionSetNotFound"]);
                }

                CheckResourceAccess(model);

                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                assumptionSet.UpdateAssumption(input.Name, input.Value);

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<AssumptionSet, AssumptionSetDto>(assumptionSet);
                await EnrichAssumptionSetDto(dto, assumptionSet);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Calculate)]
        public virtual async Task<ProjectionResultDto> CalculateProjectionAsync(Guid modelId, Guid scenarioId, CalculateProjectionDto input)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                // Check if projection already exists and force calculation not requested
                var existingProjection = model.GetProjection(scenarioId, input.Type);
                if (existingProjection != null && !input.ForceRecalculation)
                {
                    var existingDto = ObjectMapper.Map<ProjectionResult, ProjectionResultDto>(existingProjection);
                    await EnrichProjectionResultDto(existingDto, existingProjection, model, scenario);
                    return existingDto;
                }

                // Calculate projection
                ProjectionResult projectionResult = input.Type switch
                {
                    ProjectionType.CashFlow => await _financialModelingEngine.CalculateCashFlowProjectionAsync(model, scenario),
                    ProjectionType.ProfitAndLoss => await _financialModelingEngine.CalculateProfitAndLossProjectionAsync(model, scenario),
                    ProjectionType.Revenue => await _financialModelingEngine.CalculateRevenueProjectionAsync(model, scenario),
                    ProjectionType.Expense => await _financialModelingEngine.CalculateExpenseProjectionAsync(model, scenario),
                    ProjectionType.BreakEven => await _financialModelingEngine.CalculateBreakEvenAnalysisAsync(model, scenario),
                    _ => throw new UserFriendlyException(L["ErrorMessage:UnsupportedProjectionType"])
                };

                // Update notes if provided
                if (!string.IsNullOrEmpty(input.Notes))
                {
                    projectionResult.UpdateNotes(input.Notes);
                }

                // Add to model
                model.AddProjection(scenarioId, input.Type, projectionResult.ProjectedAmounts.ToList(), input.Notes);
                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dto = ObjectMapper.Map<ProjectionResult, ProjectionResultDto>(projectionResult);
                await EnrichProjectionResultDto(dto, projectionResult, model, scenario);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.Calculate)]
        public virtual async Task<List<ProjectionResultDto>> CalculateAllProjectionsAsync(Guid modelId, Guid scenarioId)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var scenario = model.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ScenarioNotFound"]);
                }

                if (!model.CanCalculate())
                {
                    throw new UserFriendlyException(L["ErrorMessage:ModelNotReady"]);
                }

                var projections = await _financialModelingEngine.CalculateAllProjectionsAsync(model, scenario);

                // Add projections to model
                foreach (var projection in projections)
                {
                    model.AddProjection(scenarioId, projection.Type, projection.ProjectedAmounts.ToList());
                }

                await _financialModelRepository.UpdateAsync(model, autoSave: true);

                var dtos = ObjectMapper.Map<List<ProjectionResult>, List<ProjectionResultDto>>(projections);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var projection = projections.First(p => p.Id == dto.Id);
                    await EnrichProjectionResultDto(dto, projection, model, scenario);
                }));

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "ErrorMessage:BusinessError", bex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<List<ProjectionResultDto>> GetProjectionsAsync(Guid modelId, Guid? scenarioId = null)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var projections = scenarioId.HasValue
                    ? model.GetProjectionsForScenario(scenarioId.Value)
                    : model.Projections.ToList();

                var dtos = ObjectMapper.Map<List<ProjectionResult>, List<ProjectionResultDto>>(projections);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var projection = projections.First(p => p.Id == dto.Id);
                    var scenario = model.GetScenario(projection.ScenarioId);
                    await EnrichProjectionResultDto(dto, projection, model, scenario);
                }));

                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<ProjectionResultDto> GetProjectionAsync(Guid modelId, Guid scenarioId, ProjectionType type)
        {
            try
            {
                var model = await _financialModelRepository.GetAsync(modelId, includeDetails: true);
                CheckResourceAccess(model);

                var projection = model.GetProjection(scenarioId, type);
                if (projection == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:ProjectionNotFound"]);
                }

                var scenario = model.GetScenario(scenarioId);
                var dto = ObjectMapper.Map<ProjectionResult, ProjectionResultDto>(projection);
                await EnrichProjectionResultDto(dto, projection, model, scenario);

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:ModelNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<List<FinancialModelDto>> GetActiveModelsAsync()
        {
            try
            {
                var models = await _financialModelRepository.GetActiveModelsAsync(_currentTenant.Id);
                var dtos = ObjectMapper.Map<List<FinancialModel>, List<FinancialModelDto>>(models);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var model = models.First(m => m.Id == dto.Id);
                    await EnrichFinancialModelDto(dto, model);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.FinancialModels.View)]
        public virtual async Task<List<FinancialModelDto>> GetCalculableModelsAsync()
        {
            try
            {
                var models = await _financialModelRepository.GetCalculableModelsAsync(_currentTenant.Id);
                var dtos = ObjectMapper.Map<List<FinancialModel>, List<FinancialModelDto>>(models);

                await Task.WhenAll(dtos.Select(async dto =>
                {
                    var model = models.First(m => m.Id == dto.Id);
                    await EnrichFinancialModelDto(dto, model);
                }));

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(FinancialModel model)
        {
            if (model.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(FinancialModel), model.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in FinancialModelAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }

        protected virtual async Task EnrichFinancialModelDto(FinancialModelDto dto, FinancialModel model)
        {
            dto.ScenarioCount = model.Scenarios.Count;
            dto.AssumptionSetCount = model.Assumptions.Count;
            dto.ProjectionCount = model.Projections.Count;
            dto.CanCalculate = model.CanCalculate();

            if (model.Projections.Any())
            {
                dto.LastCalculatedAt = model.Projections.Max(p => p.CalculatedAt);
            }

            await Task.CompletedTask;
        }

        protected virtual async Task EnrichFinancialModelDetailDto(FinancialModelDetailDto dto, FinancialModel model)
        {
            await EnrichFinancialModelDto(dto, model);

            // Enrich scenarios
            await Task.WhenAll(dto.Scenarios.Select(async scenarioDto =>
            {
                var scenario = model.Scenarios.First(s => s.Id == scenarioDto.Id);
                await EnrichModelScenarioDto(scenarioDto, scenario, model);
            }));

            // Enrich assumption sets
            await Task.WhenAll(dto.AssumptionSets.Select(async assumptionSetDto =>
            {
                var assumptionSet = model.Assumptions.First(a => a.Id == assumptionSetDto.Id);
                await EnrichAssumptionSetDto(assumptionSetDto, assumptionSet);
            }));

            // Enrich projections
            await Task.WhenAll(dto.Projections.Select(async projectionDto =>
            {
                var projection = model.Projections.First(p => p.Id == projectionDto.Id);
                var scenario = model.GetScenario(projection.ScenarioId);
                await EnrichProjectionResultDto(projectionDto, projection, model, scenario);
            }));
        }

        protected virtual async Task EnrichModelScenarioDto(ModelScenarioDto dto, ModelScenario scenario, FinancialModel model)
        {
            // Get assumption set names
            dto.AssumptionSetNames = scenario.AssociatedAssumptionSets
                .Select(id => model.GetAssumptionSet(id)?.Name)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();

            // Get projection count
            dto.ProjectionCount = model.Projections.Count(p => p.ScenarioId == scenario.Id);

            // Get last calculated date
            var lastProjection = model.Projections
                .Where(p => p.ScenarioId == scenario.Id)
                .OrderByDescending(p => p.CalculatedAt)
                .FirstOrDefault();

            dto.LastCalculatedAt = lastProjection?.CalculatedAt;

            await Task.CompletedTask;
        }

        protected virtual async Task EnrichAssumptionSetDto(AssumptionSetDto dto, AssumptionSet assumptionSet)
        {
            dto.AssumptionCount = assumptionSet.Assumptions.Count;
            dto.IsValidated = assumptionSet.ValidateAllAssumptions();

            await Task.CompletedTask;
        }

        protected virtual async Task EnrichProjectionResultDto(ProjectionResultDto dto, ProjectionResult projection, FinancialModel model, ModelScenario scenario)
        {
            dto.ModelName = model.Name;
            dto.ScenarioName = scenario?.Name;
            dto.TotalProjected = projection.GetTotalProjected();
            dto.AverageProjected = projection.GetAverageProjected();
            dto.RunwayMonths = projection.GetRunwayMonths();
            dto.BreakEvenDate = projection.GetBreakEvenDate();

            await Task.CompletedTask;
        }
    }
}
