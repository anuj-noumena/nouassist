namespace NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects
{
    /// <summary>
    /// Value object representing a semantic version identifier
    /// </summary>
    public class VersionIdentifier : ValueObject
    {
        /// <summary>
        /// The full version string (e.g., "1.0.0")
        /// </summary>
        public string Value { get; private set; }
        
        /// <summary>
        /// Major version component
        /// </summary>
        public int Major { get; private set; }
        
        /// <summary>
        /// Minor version component
        /// </summary>
        public int Minor { get; private set; }
        
        /// <summary>
        /// Patch version component
        /// </summary>
        public int Patch { get; private set; }

        private VersionIdentifier() { }

        /// <summary>
        /// Creates a new VersionIdentifier with the specified version string
        /// </summary>
        /// <param name="version">Version string in format "Major.Minor.Patch"</param>
        public VersionIdentifier(string version)
        {
            SetVersion(version);
        }

        /// <summary>
        /// Creates a new VersionIdentifier with the specified major, minor, and patch components
        /// </summary>
        public VersionIdentifier(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Value = $"{major}.{minor}.{patch}";
        }

        private void SetVersion(string version)
        {
            version = Check.NotNullOrWhiteSpace(
                version, 
                nameof(version),
                maxLength: VersionControlConsts.MaxVersionLength
            );
            
            if (!TryParseVersion(version, out var major, out var minor, out var patch))
            {
                throw new BusinessException("VersionControl:ErrorMessage:InvalidVersionFormat")
                    .WithData("version", version);
            }
            
            Major = major;
            Minor = minor;
            Patch = patch;
            Value = version;
        }
        
        private bool TryParseVersion(string version, out int major, out int minor, out int patch)
        {
            major = minor = patch = 0;
            
            var parts = version.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }
            
            return int.TryParse(parts[0], out major) &&
                   int.TryParse(parts[1], out minor) &&
                   int.TryParse(parts[2], out patch);
        }
        
        /// <summary>
        /// Creates a new version with an incremented patch number
        /// </summary>
        public VersionIdentifier IncrementPatch()
        {
            return new VersionIdentifier(Major, Minor, Patch + 1);
        }
        
        /// <summary>
        /// Creates a new version with an incremented minor number and reset patch
        /// </summary>
        public VersionIdentifier IncrementMinor()
        {
            return new VersionIdentifier(Major, Minor + 1, 0);
        }
        
        /// <summary>
        /// Creates a new version with an incremented major number and reset minor and patch
        /// </summary>
        public VersionIdentifier IncrementMajor()
        {
            return new VersionIdentifier(Major + 1, 0, 0);
        }
        
        /// <summary>
        /// Compares this version with another version
        /// </summary>
        /// <returns>
        /// -1 if this version is lower than the other,
        /// 0 if they are equal,
        /// 1 if this version is higher than the other
        /// </returns>
        public int CompareTo(VersionIdentifier other)
        {
            if (Major != other.Major)
            {
                return Major.CompareTo(other.Major);
            }
            
            if (Minor != other.Minor)
            {
                return Minor.CompareTo(other.Minor);
            }
            
            return Patch.CompareTo(other.Patch);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Major;
            yield return Minor;
            yield return Patch;
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}
