using System;
using System.Collections.Generic;
using SnakeCore.DI;
using SnakeCore.Logging;

namespace SnakeCore.VersionManagement
{
    /// <summary>
    /// Represents a semantic versioning pre-release tag.
    /// </summary>
    public struct PreReleaseField : IComparable<PreReleaseField>
    {
        public string Tag { get; private set; }
        public int Version { get; private set; }

        private readonly IComparer<string> m_tagComparer;

        /// <summary>
        /// Pre-release tags consists of 1 value or 2 values
        /// separated by dot. The second value must be an integer. The following are valid pre-release tags:
        /// - alpha
        /// - alpha.1
        /// - alpha.12
        /// - beta.1
        /// - rc
        /// - rc.20
        /// - __any-custom-tag__.12
        /// </summary>
        /// <param name="tag">Input tag</param>
        /// <param name="version">The number that comes after the tag</param>
        /// <param name="tagComparer">Tag comparer must be provided to determine how 2 tags compare to each other.
        /// By default, [ __custom-tags__ &lt; alpha &lt; beta &lt; rc ] <see cref="CommonTagComparer"/></param>
        public PreReleaseField(string tag, int version = 0, IComparer<string> tagComparer = null)
        {
            Tag = tag;
            Version = version;
            m_tagComparer = tagComparer ?? new CommonTagComparer();
        }

        /// <summary>
        /// Parses a pre-release string to a PreReleaseField object. Pre-release tags consists of 1 value or 2 values
        /// separated by dot. The second value must be an integer. The following are valid pre-release tags:
        /// - alpha
        /// - alpha.1
        /// - alpha.12
        /// - beta.1
        /// - rc
        /// - rc.20
        /// - __any-custom-tag__.12
        /// </summary>
        /// <param name="rawTag">Tag to parse</param>
        /// <param name="tagComparer">Tag comparer must be provided to determine how 2 tags compare to each other.
        /// By default, [ __custom-tags__ &lt; alpha &lt; beta &lt; rc ] <see cref="CommonTagComparer"/></param>
        public static PreReleaseField Parse(string rawTag, IComparer<string> tagComparer = null)
        {
            string[] splitted = rawTag.Split(".");
            if (splitted.Length > 2 || splitted.Length < 1)
            {
                DoWarning(rawTag);
                return default;
            }
            
            int returnVersion = 0;
            string returnTag = splitted[0];
            
            if(splitted.Length == 2 && int.TryParse(splitted[1], out int ver))
            {
                returnVersion = ver;
            }
            
            if (tagComparer != null)
            {
                return new PreReleaseField(returnTag, returnVersion, tagComparer);
            }
            
            return new PreReleaseField(returnTag, returnVersion);
        }
        
        private static void DoWarning(string tag)
        {
            SnakeCoreApplicationRuntime.LogWarning($"Tag {tag} could not be parsed into a valid pre-release tag." +
                                                   $"Checkout semantic versioning documentation for details. ");
        }
        
        public int CompareTo(PreReleaseField other)
        {
            if (m_tagComparer != null)
            {
                var tagComparison = m_tagComparer.Compare(Tag, other.Tag);
                if (tagComparison != 0) return tagComparison;
            }
            
            return Version.CompareTo(other.Version);
        }

        #region Operator Overloads

        public static bool operator <(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) < 0;

        public static bool operator >(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) > 0;

        public static bool operator ==(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) == 0;

        public static bool operator !=(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) != 0;
        
        public static bool operator <=(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) <= 0;
        
        public static bool operator >=(PreReleaseField p1, PreReleaseField p2) => p1.CompareTo(p2) >= 0;

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (other is not PreReleaseField p2) return false;
            return this == p2;
        }

        public bool Equals(PreReleaseField other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_tagComparer, Tag, Version);
        }

        #endregion
        
        private class CommonTagComparer : IComparer<string>
        {
            private string[] priorityOrder =
            {
                "alpha",
                "beta",
                "rc"
            };
            
            public int Compare(string x, string y)
            {
                int xPriority = Array.IndexOf(priorityOrder, x);
                int yPriority = Array.IndexOf(priorityOrder, y);

                return xPriority - yPriority;
            }
        }
    }
    
    /// <summary>
    /// Keeps data about the version in compliance with Semantic Versioning Format.
    /// </summary>
    public struct Version : IComparable<Version>
    {
        /// <summary>
        /// The Major Tag
        /// </summary>
        public int Major { get; private set; }
        
        /// <summary>
        /// The Minor Tag
        /// </summary>
        public int Minor { get; private set; }
        
        /// <summary>
        /// The Patch Tag
        /// </summary>
        public int Patch { get; private set; }
        
        /// <summary>
        /// The pre-release tag (if not exists, default)
        /// </summary>
        public PreReleaseField PreReleaseTag{ get; private set; }

        /// <summary>
        /// If has the pre-release tag.
        /// </summary>
        public bool HasPreReleaseTag { get; private set; }
        
        
        public Version(int major, int minor, int patch, PreReleaseField preReleaseTag = default)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreReleaseTag = preReleaseTag;
            HasPreReleaseTag = !preReleaseTag.Equals(default);
        }
        
        /// <summary>
        /// Parses given version tag string to a Version object. If the versionTag not valid, returns default value.
        /// </summary>
        /// <param name="versionTag">Version tag to parse</param>
        /// <param name="preReleaseTagComparer">Tag comparer must be provided to determine how 2 pre-release tags compare to each other.
        /// By default, [ __custom-tags__ &lt; alpha &lt; beta &lt; rc ] </param>
        public static Version Parse(string versionTag, IComparer<string> preReleaseTagComparer = null)
        {
            string[] splitted = versionTag.Split("-");

            if (splitted.Length == 0)
            {
                DoWarning(versionTag);
                return default;
            }

            PreReleaseField preReleaseTag = default;
            if (splitted.Length == 2)
            {
                preReleaseTag = PreReleaseField.Parse(splitted[1], preReleaseTagComparer);
            }

            string[] versionSplitted = splitted[0].Split(".");

            if (versionSplitted.Length != 3
                || !int.TryParse(versionSplitted[0], out int major)
                || !int.TryParse(versionSplitted[1], out int minor)
                || !int.TryParse(versionSplitted[2], out int patch))
            {
                DoWarning(versionTag);
                return default;
            }

            return new Version(major, minor, patch, preReleaseTag);
        }

        private static void DoWarning(string versionTag)
        {
            SnakeCoreApplicationRuntime.LogWarning($"Version tag {versionTag} could not be parsed into " +
                                                   $"a valid version. Use semantic versioning.");
        }

        public int CompareTo(Version other)
        {
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            var patchComparison = Patch.CompareTo(other.Patch);
            if (patchComparison != 0) return patchComparison;
            
            // The one without the prerelease tag is bigger.
            if (HasPreReleaseTag && !other.HasPreReleaseTag) return -1;
            if (!HasPreReleaseTag && other.HasPreReleaseTag) return 1;
            if (!HasPreReleaseTag && !other.HasPreReleaseTag) return 0;
            
            var preReleaseTagComparison = PreReleaseTag.CompareTo(other.PreReleaseTag);
            return preReleaseTagComparison;

        }

        #region Operator Overloads

        public static bool operator <(Version p1, Version p2) => p1.CompareTo(p2) < 0;

        public static bool operator >(Version p1, Version p2) => p1.CompareTo(p2) > 0;

        public static bool operator ==(Version p1, Version p2) => p1.CompareTo(p2) == 0;

        public static bool operator !=(Version p1, Version p2) => p1.CompareTo(p2) != 0;
        
        public static bool operator <=(Version p1, Version p2) => p1.CompareTo(p2) <= 0;
        
        public static bool operator >=(Version p1, Version p2) => p1.CompareTo(p2) >= 0;

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (other is not Version p2) return false;
            return this == p2;
        }

        public bool Equals(Version other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor, Patch, PreReleaseTag);
        }
        
        #endregion
    }
}