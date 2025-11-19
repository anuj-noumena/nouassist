using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.GTD;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class FollowupAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IFollowupAppService _followupAppService;

        public FollowupAppServiceTests()
        {
            _followupAppService = GetRequiredService<IFollowupAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateFollowup()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateFollowupDto
                {
                    FollowupAt = DateTime.Now.AddDays(7)
                };

                var result = await _followupAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
            });
        }

        [Fact]
        public async Task MarkAsDoneAsync_Should_MarkFollowupAsDone()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateFollowupDto
                {
                    FollowupAt = DateTime.Now.AddDays(7)
                };

                var created = await _followupAppService.CreateAsync(createInput);
                var result = await _followupAppService.MarkAsDoneAsync(created.Id);

                result.Status.ShouldBe(Enums.FollowupStatus.Done);
            });
        }
    }
}
