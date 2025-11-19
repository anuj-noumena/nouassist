using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    public class FollowupRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IFollowupRepository _followupRepository;

        public FollowupRepositoryTests()
        {
            _followupRepository = GetRequiredService<IFollowupRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertFollowup()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var followup = new Followup(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(7));

                var inserted = await _followupRepository.InsertAsync(followup);
                inserted.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnFollowup()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var followup = new Followup(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(7));

                await _followupRepository.InsertAsync(followup);
                var retrieved = await _followupRepository.GetAsync(followup.Id);
                
                retrieved.ShouldNotBeNull();
            });
        }
    }
}
