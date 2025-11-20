using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.DocumentManagement.ValueObjects
{
    /// <summary>
    /// Represents a version number for a document
    /// </summary>
    public class FileVersionNumber : ValueObject, IComparable<FileVersionNumber>
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }

        private FileVersionNumber() { }

        public FileVersionNumber(int major, int minor)
        {
            Check.Range(major, nameof(major), 0, int.MaxValue);
            Check.Range(minor, nameof(minor), 0, int.MaxValue);

            Major = major;
            Minor = minor;
        }

        public static FileVersionNumber Create(int versionNumber)
        {
            return new FileVersionNumber(versionNumber, 0);
        }

        public static FileVersionNumber Initial => new FileVersionNumber(1, 0);

        public FileVersionNumber Next()
        {
            return new FileVersionNumber(Major + 1, 0);
        }

        public FileVersionNumber NextMinor()
        {
            return new FileVersionNumber(Major, Minor + 1);
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}";
        }

        public int ToVersionNumber()
        {
            return Major;
        }

        public int CompareTo(FileVersionNumber other)
        {
            if (other == null) return 1;

            if (Major != other.Major)
                return Major.CompareTo(other.Major);

            return Minor.CompareTo(other.Minor);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Major;
            yield return Minor;
        }
    }
}
