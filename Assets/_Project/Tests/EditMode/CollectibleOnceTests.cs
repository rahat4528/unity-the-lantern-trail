using NUnit.Framework;
using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Interaction;

namespace LanternTrail.Tests.EditMode
{
    [TestFixture]
    public class CollectibleOnceTests
    {
        private GameObject gmGo;
        private GameManager gameManager;

        [SetUp]
        public void SetUp()
        {
            gmGo = new GameObject("GameManager_Test");
            gameManager = gmGo.AddComponent<GameManager>();
            gameManager.StartNewRun();
        }

        [TearDown]
        public void TearDown()
        {
            if (gmGo != null) Object.DestroyImmediate(gmGo);
        }

        [Test]
        public void Collectible_FirstCollection_SucceedsAndIncrementsCounter()
        {
            string id = "L1_Lantern_01";
            bool firstResult = gameManager.TryCollect(id, false);

            Assert.IsTrue(firstResult, "First collection attempt must succeed.");
            Assert.AreEqual(1, gameManager.LanternsCollected);
            Assert.IsTrue(gameManager.IsCollected(id));
        }

        [Test]
        public void Collectible_SecondCollection_FailsAndDoesNotDuplicateReward()
        {
            string id = "L1_Lantern_01";
            gameManager.TryCollect(id, false);

            bool secondResult = gameManager.TryCollect(id, false);

            Assert.IsFalse(secondResult, "Second collection attempt on the same ID must return false.");
            Assert.AreEqual(1, gameManager.LanternsCollected, "Lantern counter must not increase on duplicate collection.");
        }
    }
}
