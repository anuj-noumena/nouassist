using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.Contacts.EntityFrameworkCore
{
    [ConnectionStringName(ContactsDbProperties.ConnectionStringName)]
    public interface IContactsDbContext : IEfCoreDbContext
    {
        DbSet<Contact> Contacts { get; }
        DbSet<Client> Clients { get; }
    }
}
