using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.GTD;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class GtdHorizonAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IGtdHorizonAppService _gtdHorizonAppService;

        public GtdHorizonAppServiceTests()
        {
            _gtdHorizonAppService = GetRequiredService<IGtdHorizonAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateGtdHorizon()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateGtdHorizonDto
                {
                    HorizonLevel = GtdHorizonLevel.Horizon5_Purpose,
                    Title = "Life Purpose"
                };

                var result = await _gtdHorizonAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Title.ShouldBe("Life Purpose");
            });
        }

        [Fact]
        public async Task PublishAsync_Should_PublishHorizon()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateGtdHorizonDto
                {
                    HorizonLevel = GtdHorizonLevel.Horizon5_Purpose,
                    Title = "Life Purpose"
                };

                var created = await _gtdHorizonAppService.CreateAsync(createInput);
                var result = await _gtdHorizonAppService.PublishAsync(created.Id);

                result.IsDraft.ShouldBeFalse();
            });
        }
    }
}
