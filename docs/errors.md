Constructor 'AuditEntry' has 0 parameter(s) but is invoked with 12 argument(s)
 var auditEntry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                entityReference,
                eventType,
                userContext,
                operationDescription,
                SerializeValue(oldValue),
                SerializeValue(newValue),
                metadata,
                severity,
                complianceFlags,
                retentionDays);

in modules/FinancialManagement/src/NouAssist.FinancialManagement.Domain/AuditTrail/Services/AuditLoggingService.cs