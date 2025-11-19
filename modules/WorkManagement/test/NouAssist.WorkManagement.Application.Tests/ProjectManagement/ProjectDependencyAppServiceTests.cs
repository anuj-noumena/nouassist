using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Dependencies;
using NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectDependencyAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectDependencyAppService _projectDependencyAppService;

        public ProjectDependencyAppServiceTests()
        {
            _projectDependencyAppService = GetRequiredService<IProjectDependencyAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateProjectDependency()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectDependencyDto
                {
                    ProjectId = Guid.NewGuid(),
                    DependsOnProjectId = Guid.NewGuid()
                };

                var result = await _projectDependencyAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.ProjectId.ShouldBe(input.ProjectId);
            });
        }
    }
}
