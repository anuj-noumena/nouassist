using Volo.Abp;
using Volo.Abp.Domain.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities
{
    /// <summary>
    /// Represents a class of shares with specific rights and characteristics
    /// </summary>
    public class ShareClass : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public SecurityType SecurityType { get; private set; }
        public decimal ParValue { get; private set; }
        public int VotingRights { get; private set; }
        public bool HasDividendRights { get; private set; }
        public bool HasLiquidationPreference { get; private set; }
        public decimal LiquidationMultiple { get; private set; }
        public bool HasAntiDilutionRights { get; private set; }
        public string AntiDilutionType { get; private set; }

        protected ShareClass() { }

        internal ShareClass(
            Guid id,
            string name,
            string description,
            SecurityType securityType,
            decimal parValue = 0.001m,
            int votingRights = 1,
            bool hasDividendRights = true,
            bool hasLiquidationPreference = false,
            decimal liquidationMultiple = 1.0m,
            bool hasAntiDilutionRights = false,
            string antiDilutionType = null) : base(id)
        {
            SetName(name);
            SetDescription(description);
            SecurityType = securityType;
            ParValue = Math.Max(parValue, 0);
            VotingRights = Math.Max(votingRights, 0);
            HasDividendRights = hasDividendRights;
            HasLiquidationPreference = hasLiquidationPreference;
            LiquidationMultiple = Math.Max(liquidationMultiple, 0);
            HasAntiDilutionRights = hasAntiDilutionRights;
            AntiDilutionType = antiDilutionType;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CapTableConsts.MaxShareClassNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: CapTableConsts.MaxDescriptionLength
            );
        }

        internal void UpdateDetails(
            string name,
            string description,
            int votingRights,
            bool hasDividendRights,
            bool hasLiquidationPreference,
            decimal liquidationMultiple,
            bool hasAntiDilutionRights,
            string antiDilutionType)
        {
            SetName(name);
            SetDescription(description);
            VotingRights = Math.Max(votingRights, 0);
            HasDividendRights = hasDividendRights;
            HasLiquidationPreference = hasLiquidationPreference;
            LiquidationMultiple = Math.Max(liquidationMultiple, 0);
            HasAntiDilutionRights = hasAntiDilutionRights;
            AntiDilutionType = antiDilutionType;
        }
    }
}
