#if !NETCORE && !MSTESTS
using System;
using NUnit.Framework;

namespace Acr.Settings.Tests {

    [TestFixture]
    public class LocalSettingTests : AbstractSettingTests {

        protected override ISettings Create() {
            return new SettingsImpl(null);
        }


        [Test]
        public void GlobalInstanceTest() {
            Assert.IsNotNull(Acr.Settings.CrossSettings.Local);
            Assert.IsFalse(Acr.Settings.CrossSettings.Local.IsRoamingProfile, "Romaing profile detected");
        }
    }
}
#endif