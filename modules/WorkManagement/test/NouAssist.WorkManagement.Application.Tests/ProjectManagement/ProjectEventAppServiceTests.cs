using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Events;
using NouAssist.WorkManagement.ProjectManagement.Events.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectEventAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectEventAppService _projectEventAppService;

        public ProjectEventAppServiceTests()
        {
            _projectEventAppService = GetRequiredService<IProjectEventAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateProjectEvent()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectEventDto
                {
                    ProjectId = Guid.NewGuid(),
                    Title = "Project Kickoff",
                    EventDate = DateTime.Now
                };

                var result = await _projectEventAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Title.ShouldBe("Project Kickoff");
            });
        }
    }
}
