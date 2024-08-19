using NUnit.Framework;
using SnakeCore.VersionManagement;

namespace SnakeCore.Tests.PlayMode.VersionManagement
{
    public class VersionManagementTest : RuntimeTestBoot
    {
        protected override string AdditionalConfig { get; } = "";

        private string[] versionStrings =
        {
            "0.1.2",
            "1.1.0",
            "12.45.2-rc.43",
            "12.45.2-rc.42",
            "12.45.2-alpha.42",
            "12.45.2",
            "12.45.2-alpha.1",
            "12.45.2"
        };
        
        [Test]
        public void VersionComparisionTest()
        {
            Version[] versions = new Version[versionStrings.Length];
            for (int i = 0; i < versions.Length; i++)
            {
                versions[i] = Version.Parse(versionStrings[i]);
            }
            
            Assert.That(
                versions[0].Major == 0 && 
                versions[0].Minor == 1 && 
                versions[0].Patch == 2 && 
                !versions[0].HasPreReleaseTag);

            Assert.That(versions[2].HasPreReleaseTag && 
                        versions[2].PreReleaseTag.Version == 43 && 
                        versions[2].PreReleaseTag.Tag == "rc");
            
            Assert.That(versions[0] < versions[1]);
            Assert.That(versions[2] > versions[3]);
            Assert.That(versions[2] > versions[3]);
            Assert.That(versions[3] > versions[4]);
            Assert.That(versions[5] > versions[3]);
            Assert.That(versions[4] > versions[6]);
            Assert.That(versions[5] == versions[7]);
            Assert.That(versions[5] >= versions[7]);
            Assert.That(versions[5] <= versions[7]);
            
        }

    }
}
