using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore;

[ConnectionStringName(ContactManagementDbProperties.ConnectionStringName)]
public class ContactManagementDbContext : AbpDbContext<ContactManagementDbContext>, IContactManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureContactManagement();
    }
}
