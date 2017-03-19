#if !NETCORE && !MSTESTS
using System;
using NUnit.Framework;


namespace Acr.Settings.Tests
{

    [TestFixture]
    public class RoamingSettingTests : AbstractSettingTests
    {

        protected override ISettings Create()
        {
            return new SettingsImpl("acr.settings.tests");
        }


        [Test]
        public void GlobalInstanceTest()
        {
            //Acr.Settings.CrossSettings.InitRoaming("acr.settings.tests");
            //Assert.NotNull(Acr.Settings.CrossSettings.Roaming);
            //Assert.IsTrue(Acr.Settings.CrossSettings.Roaming.IsRoamingProfile, "Romaing profile NOT detected");
        }
    }
}
#endif