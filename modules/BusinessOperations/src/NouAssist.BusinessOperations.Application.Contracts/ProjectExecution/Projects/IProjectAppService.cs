using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects
{
    /// <summary>
    /// Interface for project application service
    /// </summary>
    public interface IProjectAppService : 
        ICrudAppService<
            ProjectDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateProjectDto,
            UpdateProjectDto>
    {
        /// <summary>
        /// Gets a project with all details
        /// </summary>
        Task<ProjectDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a list of projects by organization ID
        /// </summary>
        Task<List<ProjectListDto>> GetListByOrganizationIdAsync(Guid organizationId);
        
        /// <summary>
        /// Gets a list of projects by contact ID
        /// </summary>
        Task<List<ProjectListDto>> GetListByContactIdAsync(Guid contactId);
        
        /// <summary>
        /// Gets a list of projects by status
        /// </summary>
        Task<List<ProjectListDto>> GetListByStatusAsync(ProjectStatus status);
        
        /// <summary>
        /// Updates the status of a project
        /// </summary>
        Task<ProjectDto> UpdateStatusAsync(Guid id, UpdateProjectStatusDto input);
        
        /// <summary>
        /// Adds a phase to a project
        /// </summary>
        Task<ProjectPhaseDto> AddPhaseAsync(Guid id, CreateProjectPhaseDto input);
        
        /// <summary>
        /// Adds a milestone to a project
        /// </summary>
        Task<ProjectMilestoneDto> AddMilestoneAsync(Guid id, CreateProjectMilestoneDto input);
        
        /// <summary>
        /// Updates a milestone status (complete/incomplete)
        /// </summary>
        Task UpdateMilestoneStatusAsync(Guid id, UpdateMilestoneStatusDto input);
        
        /// <summary>
        /// Adds a member to a project
        /// </summary>
        Task<ProjectMemberDto> AddMemberAsync(Guid id, CreateProjectMemberDto input);
        
        /// <summary>
        /// Removes a member from a project
        /// </summary>
        Task RemoveMemberAsync(Guid id, Guid userId);
        
        /// <summary>
        /// Creates a project from a template
        /// </summary>
        Task<ProjectDto> CreateFromTemplateAsync(CreateProjectFromTemplateDto input);
        
        /// <summary>
        /// Gets a list of available templates
        /// </summary>
        Task<List<string>> GetAvailableTemplatesAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects
{
    /// <summary>
    /// Interface for project application service
    /// </summary>
    public interface IProjectAppService : IApplicationService
    {
        /// <summary>
        /// Gets a project by ID
        /// </summary>
        Task<ProjectDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a project with all its details by ID
        /// </summary>
        Task<ProjectWithDetailsDto> GetWithDetailsAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of projects
        /// </summary>
        Task<PagedResultDto<ProjectDto>> GetListAsync(GetProjectListDto input);
        
        /// <summary>
        /// Creates a new project
        /// </summary>
        Task<ProjectDto> CreateAsync(CreateProjectDto input);
        
        /// <summary>
        /// Updates an existing project
        /// </summary>
        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input);
        
        /// <summary>
        /// Updates a project's status
        /// </summary>
        Task<ProjectDto> UpdateStatusAsync(Guid id, UpdateProjectStatusDto input);
        
        /// <summary>
        /// Deletes a project
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a phase to a project
        /// </summary>
        Task<ProjectPhaseDto> AddPhaseAsync(Guid projectId, CreateProjectPhaseDto input);
        
        /// <summary>
        /// Adds a milestone to a project
        /// </summary>
        Task<ProjectMilestoneDto> AddMilestoneAsync(Guid projectId, CreateProjectMilestoneDto input);
        
        /// <summary>
        /// Updates a milestone's status
        /// </summary>
        Task UpdateMilestoneStatusAsync(Guid projectId, string milestoneName, UpdateProjectMilestoneStatusDto input);
        
        /// <summary>
        /// Adds a member to a project
        /// </summary>
        Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto input);
        
        /// <summary>
        /// Removes a member from a project
        /// </summary>
        Task RemoveMemberAsync(Guid projectId, Guid userId);
        
        /// <summary>
        /// Gets project completion percentage
        /// </summary>
        Task<double> GetCompletionPercentageAsync(Guid projectId);
        
        /// <summary>
        /// Gets the total logged hours for a project
        /// </summary>
        Task<double> GetTotalLoggedHoursAsync(Guid projectId);
        
        /// <summary>
        /// Creates a project from a template
        /// </summary>
        Task<ProjectDto> CreateFromTemplateAsync(string templateName, CreateProjectDto input);
        
        /// <summary>
        /// Gets a list of available project templates
        /// </summary>
        Task<List<string>> GetAvailableTemplatesAsync();
    }
}
