using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LanternTrail.Core;

namespace LanternTrail.Tests.PlayMode
{
    public class FullRunSmokeTest
    {
        private GameObject gmGo;
        private GameObject omGo;
        private GameManager gameManager;
        private ObjectiveManager objectiveManager;

        [SetUp]
        public void SetUp()
        {
            gmGo = new GameObject("GameManager_SmokeTest");
            gameManager = gmGo.AddComponent<GameManager>();

            omGo = new GameObject("ObjectiveManager_SmokeTest");
            objectiveManager = omGo.AddComponent<ObjectiveManager>();

            gameManager.StartNewRun();
        }

        [TearDown]
        public void TearDown()
        {
            if (gmGo != null) Object.Destroy(gmGo);
            if (omGo != null) Object.Destroy(omGo);
        }

        [UnityTest]
        public IEnumerator FullRun_ProgressionThroughThreeLocationsAndEnding_ResetsCleanly()
        {
            yield return null;

            // Location 1: Village Paths (3 lanterns + 1 golden)
            Assert.AreEqual(GameLocation.VillagePaths, gameManager.CurrentLocation);
            objectiveManager.InitializeObjective("Collect Lanterns", "Find 3", 3);
            gameManager.TryCollect("L1_Lantern_01", false);
            gameManager.TryCollect("L1_Lantern_02", false);
            gameManager.TryCollect("L1_Lantern_03", false);
            gameManager.TryCollect("L1_Golden_01", true);
            objectiveManager.AddProgress(3);

            Assert.IsTrue(objectiveManager.IsCompleted);
            Assert.AreEqual(3, gameManager.LanternsCollected);
            Assert.AreEqual(1, gameManager.GoldenLanternsFound);

            // Location 2: Misty River (Step plates sequence + 1 golden)
            gameManager.SetLocation(GameLocation.MistyRiver);
            Assert.AreEqual(GameLocation.MistyRiver, gameManager.CurrentLocation);
            objectiveManager.InitializeObjective("Step Plates", "N -> E -> W", 3);
            gameManager.TryCollect("L2_Golden_01", true);
            objectiveManager.AddProgress(3);

            Assert.IsTrue(objectiveManager.IsCompleted);
            Assert.AreEqual(2, gameManager.GoldenLanternsFound);

            // Location 3: Lighthouse Hill (1 golden + light beacon)
            gameManager.SetLocation(GameLocation.LighthouseHill);
            Assert.AreEqual(GameLocation.LighthouseHill, gameManager.CurrentLocation);
            objectiveManager.InitializeObjective("Light Beacon", "Ignite flame", 1);
            gameManager.TryCollect("L3_Golden_01", true);
            objectiveManager.AddProgress(1);

            Assert.IsTrue(objectiveManager.IsCompleted);
            Assert.AreEqual(3, gameManager.GoldenLanternsFound);

            // Complete Game -> Ending
            gameManager.CompleteGame();
            Assert.AreEqual(GameState.Ending, gameManager.CurrentState);
            Assert.AreEqual(3, StarRating.Calculate(gameManager.GoldenLanternsFound), "Finding all 3 golden lanterns must award 3 stars.");

            // Play Again -> Fresh Run reset verification (Section 3.5)
            gameManager.StartNewRun();
            Assert.AreEqual(0, gameManager.LanternsCollected, "Lanterns must reset to 0 on new run.");
            Assert.AreEqual(0, gameManager.GoldenLanternsFound, "Golden lanterns must reset to 0 on new run.");
            Assert.AreEqual(GameLocation.VillagePaths, gameManager.CurrentLocation, "Must reset to Location 1.");
            Assert.AreEqual(GameState.Playing, gameManager.CurrentState);
        }
    }
}
