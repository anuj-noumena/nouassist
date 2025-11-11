using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CoreProductivity.ProjectManagement.Data;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.CoreProductivity.ProjectManagement.Permissions;
using NouAssist.CoreProductivity.ProjectManagement.SuccessCriteria.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.ProjectManagement.SuccessCriteria
{
    [Authorize(ProjectsPermissions.SuccessCriteria.Default)]
    public class ProjectSuccessCriterionAppService : ApplicationService, IProjectSuccessCriterionAppService
    {
        private readonly IProjectSuccessCriterionRepository _criterionRepository;

        public ProjectSuccessCriterionAppService(
            IProjectSuccessCriterionRepository criterionRepository)
        {
            _criterionRepository = criterionRepository;
        }

        public async Task<SuccessCriterionDto> GetAsync(Guid id)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            return ObjectMapper.Map<ProjectSuccessCriterion, SuccessCriterionDto>(criterion);
        }

        public async Task<List<SuccessCriterionDto>> GetByProjectAsync(Guid projectId)
        {
            var criteria = await _criterionRepository.GetByProjectIdAsync(projectId);
            return ObjectMapper.Map<List<ProjectSuccessCriterion>, List<SuccessCriterionDto>>(criteria);
        }

        public async Task<List<SuccessCriterionDto>> GetUnmetCriteriaAsync(Guid projectId)
        {
            var criteria = await _criterionRepository.GetUnmetCriteriaAsync(projectId);
            return ObjectMapper.Map<List<ProjectSuccessCriterion>, List<SuccessCriterionDto>>(criteria);
        }

        [Authorize(ProjectsPermissions.SuccessCriteria.Create)]
        public async Task<SuccessCriterionDto> CreateAsync(CreateSuccessCriterionDto input)
        {
            var criterion = new ProjectSuccessCriterion(
                GuidGenerator.Create(),
                input.ProjectId,
                input.Title,
                input.CriterionType,
                input.Description,
                input.MeasurementMethod,
                input.TargetValue,
                input.Weight);

            if (!string.IsNullOrWhiteSpace(input.Unit))
            {
                criterion.Unit = input.Unit;
            }

            if (input.TargetNumeric.HasValue)
            {
                criterion.TargetNumeric = input.TargetNumeric;
            }

            if (input.TargetDate.HasValue)
            {
                criterion.TargetDate = input.TargetDate;
            }

            await _criterionRepository.InsertAsync(criterion);

            return ObjectMapper.Map<ProjectSuccessCriterion, SuccessCriterionDto>(criterion);
        }

        [Authorize(ProjectsPermissions.SuccessCriteria.Update)]
        public async Task<SuccessCriterionDto> UpdateAsync(Guid id, UpdateSuccessCriterionDto input)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            
            criterion.Title = input.Title;
            criterion.Description = input.Description;
            criterion.MeasurementMethod = input.MeasurementMethod;
            criterion.TargetValue = input.TargetValue;
            criterion.TargetNumeric = input.TargetNumeric;
            criterion.Unit = input.Unit;
            criterion.SetWeight(input.Weight);
            criterion.TargetDate = input.TargetDate;
            criterion.Notes = input.Notes;
            
            await _criterionRepository.UpdateAsync(criterion);

            return ObjectMapper.Map<ProjectSuccessCriterion, SuccessCriterionDto>(criterion);
        }

        [Authorize(ProjectsPermissions.SuccessCriteria.Update)]
        public async Task<SuccessCriterionDto> UpdateActualValueAsync(Guid id, UpdateActualValueDto input)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            
            criterion.UpdateActualValue(input.ActualValue, input.ActualNumeric);
            
            await _criterionRepository.UpdateAsync(criterion);

            return ObjectMapper.Map<ProjectSuccessCriterion, SuccessCriterionDto>(criterion);
        }

        [Authorize(ProjectsPermissions.SuccessCriteria.MarkAsMet)]
        public async Task<SuccessCriterionDto> MarkAsMetAsync(Guid id, MarkCriterionAsMetDto input)
        {
            var criterion = await _criterionRepository.GetAsync(id);
            
            var metBy = CurrentUser.UserName ?? CurrentUser.Id?.ToString() ?? "System";
            criterion.MarkAsMet(metBy, input.Notes, input.EvidenceUrl);
            
            await _criterionRepository.UpdateAsync(criterion);

            return ObjectMapper.Map<ProjectSuccessCriterion, SuccessCriterionDto>(criterion);
        }

        [Authorize(ProjectsPermissions.SuccessCriteria.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _criterionRepository.DeleteAsync(id);
        }
    }
}
