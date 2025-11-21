using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.AuditTrail
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AuditEntryToAuditEntryDtoMapper : MapperBase<AuditEntry, AuditEntryDto>
    {
        [MapperIgnoreSource(nameof(AuditEntry.TenantId))]
        [MapperIgnoreSource(nameof(AuditEntry.EntityReference))]
        [MapperIgnoreSource(nameof(AuditEntry.UserContext))]
        [MapperIgnoreSource(nameof(AuditEntry.Metadata))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityType))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityId))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityName))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.UserId))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.UserName))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.IPAddress))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.Metadata))]
        public override partial AuditEntryDto Map(AuditEntry source);

        [MapperIgnoreSource(nameof(AuditEntry.TenantId))]
        [MapperIgnoreSource(nameof(AuditEntry.EntityReference))]
        [MapperIgnoreSource(nameof(AuditEntry.UserContext))]
        [MapperIgnoreSource(nameof(AuditEntry.Metadata))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityType))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityId))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.EntityName))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.UserId))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.UserName))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.IPAddress))]
        [MapperIgnoreTarget(nameof(AuditEntryDto.Metadata))]
        public override partial void Map(AuditEntry source, AuditEntryDto destination);

        public override void AfterMap(AuditEntry source, AuditEntryDto destination)
        {
            destination.EntityType = source.EntityReference.EntityType;
            destination.EntityId = source.EntityReference.EntityId;
            destination.EntityName = source.EntityReference.EntityName;
            destination.UserId = source.UserContext.UserId;
            destination.UserName = source.UserContext.UserName;
            destination.IPAddress = source.UserContext.IPAddress;
            destination.Metadata = source.Metadata.Properties;
        }
    }
}
