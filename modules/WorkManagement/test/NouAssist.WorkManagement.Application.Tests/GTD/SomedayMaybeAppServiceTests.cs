using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.GTD;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class SomedayMaybeAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly ISomedayMaybeAppService _somedayMaybeAppService;

        public SomedayMaybeAppServiceTests()
        {
            _somedayMaybeAppService = GetRequiredService<ISomedayMaybeAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateSomedayMaybeItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateSomedayMaybeDto
                {
                    Title = "Learn Piano"
                };

                var result = await _somedayMaybeAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Title.ShouldBe("Learn Piano");
            });
        }

        [Fact]
        public async Task PromoteAsync_Should_PromoteItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateSomedayMaybeDto
                {
                    Title = "Learn Piano"
                };

                var created = await _somedayMaybeAppService.CreateAsync(createInput);
                var promoteInput = new PromoteSomedayMaybeDto
                {
                    PromotedToType = "Project",
                    PromotedToId = Guid.NewGuid()
                };

                var result = await _somedayMaybeAppService.PromoteAsync(created.Id, promoteInput);

                result.Status.ShouldBe(Enums.SomedayMaybeStatus.Promoted);
            });
        }
    }
}
