namespace NouAssist.AuditTrail.Enums
{
    [Flags]
    public enum ComplianceFlag
    {
        None = 0,
        SOX = 1 << 0,              // Sarbanes-Oxley compliance
        GDPR = 1 << 1,             // General Data Protection Regulation
        PCI_DSS = 1 << 2,          // Payment Card Industry Data Security Standard
        HIPAA = 1 << 3,            // Health Insurance Portability and Accountability Act
        AML = 1 << 4,              // Anti-Money Laundering
        FinancialTransaction = 1 << 5,
        HighRisk = 1 << 6,
        Suspicious = 1 << 7
    }
}
