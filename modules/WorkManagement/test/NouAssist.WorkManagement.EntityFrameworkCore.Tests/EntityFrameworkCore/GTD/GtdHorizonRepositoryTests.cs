using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    public class GtdHorizonRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IGtdHorizonRepository _gtdHorizonRepository;

        public GtdHorizonRepositoryTests()
        {
            _gtdHorizonRepository = GetRequiredService<IGtdHorizonRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertGtdHorizon()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var horizon = new GtdHorizon(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    GtdHorizonLevel.Horizon5_Purpose,
                    "Life Purpose");

                var inserted = await _gtdHorizonRepository.InsertAsync(horizon);
                inserted.ShouldNotBeNull();
                inserted.Title.ShouldBe("Life Purpose");
            });
        }
    }
}
