using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Sprints;
using NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectSprintAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectSprintAppService _sprintAppService;

        public ProjectSprintAppServiceTests()
        {
            _sprintAppService = GetRequiredService<IProjectSprintAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateSprint()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectSprintDto
                {
                    ProjectId = Guid.NewGuid(),
                    Name = "Sprint 1",
                    Goal = "Complete user authentication",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(14)
                };

                var result = await _sprintAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Name.ShouldBe("Sprint 1");
            });
        }
    }
}
