using NUnit.Framework;
using UnityEngine;
using LanternTrail.Save;

namespace LanternTrail.Tests.EditMode
{
    [TestFixture]
    public class SaveSystemTests
    {
        [Test]
        public void AudioSettings_SaveAndLoad_MaintainsValues()
        {
            float expectedMusic = 0.42f;
            float expectedSfx = 0.88f;
            bool expectedVib = false;

            SaveSystem.SaveAudioSettings(expectedMusic, expectedSfx, expectedVib);

            Assert.AreEqual(expectedMusic, SaveSystem.GetMusicVolume(), 0.001f);
            Assert.AreEqual(expectedSfx, SaveSystem.GetSfxVolume(), 0.001f);
            Assert.AreEqual(expectedVib, SaveSystem.GetVibrationEnabled());
        }

        [Test]
        public void ControlSettings_SaveAndLoad_MaintainsLayout()
        {
            SaveSystem.SaveControlSettings(true, 1.25f);

            Assert.IsTrue(SaveSystem.GetLeftHanded());
            Assert.AreEqual(1.25f, SaveSystem.GetJoystickSize(), 0.001f);
        }

        [Test]
        public void GraphicsTier_SaveAndLoad_MaintainsTier()
        {
            SaveSystem.SaveGraphicsTier(0);
            Assert.AreEqual(0, SaveSystem.GetGraphicsTier());

            SaveSystem.SaveGraphicsTier(1);
            Assert.AreEqual(1, SaveSystem.GetGraphicsTier());
        }
    }
}
