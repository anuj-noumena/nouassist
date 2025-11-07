using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore
{
    [ConnectionStringName(ContactsDbProperties.ConnectionStringName)]
    public interface IContactsDbContext : IEfCoreDbContext
    {
        DbSet<Contact> Contacts { get; }
        DbSet<Client> Clients { get; }
    }
}
