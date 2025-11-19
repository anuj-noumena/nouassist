using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Projects;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectAppService _projectAppService;

        public ProjectAppServiceTests()
        {
            _projectAppService = GetRequiredService<IProjectAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateProject()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectDto
                {
                    Title = "Test Project",
                    Description = "Test Description",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                };

                // Act
                var result = await _projectAppService.CreateAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
                result.Title.ShouldBe(input.Title);
                result.Description.ShouldBe(input.Description);
            });
        }

        [Fact]
        public async Task CreateAsync_WithEmptyTitle_Should_ThrowValidationException()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectDto
                {
                    Title = "", // Empty title
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                };

                // Act & Assert
                await Should.ThrowAsync<AbpValidationException>(async () =>
                    await _projectAppService.CreateAsync(input));
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateProjectDto
                {
                    Title = "Test Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                };

                var createdProject = await _projectAppService.CreateAsync(createInput);
                var retrievedProject = await _projectAppService.GetAsync(createdProject.Id);

                // Assert
                retrievedProject.ShouldNotBeNull();
                retrievedProject.Id.ShouldBe(createdProject.Id);
                retrievedProject.Title.ShouldBe(createdProject.Title);
            });
        }

        [Fact]
        public async Task UpdateAsync_WithValidInput_Should_UpdateProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateProjectDto
                {
                    Title = "Original Title",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                };

                var createdProject = await _projectAppService.CreateAsync(createInput);

                var updateInput = new UpdateProjectDto
                {
                    Title = "Updated Title",
                    Description = "Updated Description"
                };

                var updatedProject = await _projectAppService.UpdateAsync(createdProject.Id, updateInput);

                // Assert
                updatedProject.ShouldNotBeNull();
                updatedProject.Title.ShouldBe("Updated Title");
                updatedProject.Description.ShouldBe("Updated Description");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteProject()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateProjectDto
                {
                    Title = "Project to Delete",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                };

                var createdProject = await _projectAppService.CreateAsync(createInput);
                await _projectAppService.DeleteAsync(createdProject.Id);

                // Assert
                await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
                    await _projectAppService.GetAsync(createdProject.Id));
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test projects
                await _projectAppService.CreateAsync(new CreateProjectDto
                {
                    Title = "Project 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                });
                await _projectAppService.CreateAsync(new CreateProjectDto
                {
                    Title = "Project 2",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                });

                var input = new GetProjectListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _projectAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}
