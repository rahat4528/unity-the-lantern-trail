using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LanternTrail.Core;

namespace LanternTrail.Tests.PlayMode
{
    public class PauseTests
    {
        private GameObject gmGo;
        private GameManager gameManager;

        [SetUp]
        public void SetUp()
        {
            gmGo = new GameObject("GameManager_PauseTest");
            gameManager = gmGo.AddComponent<GameManager>();
            gameManager.StartNewRun();
        }

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            if (gmGo != null) Object.Destroy(gmGo);
        }

        [UnityTest]
        public IEnumerator Pause_SetsTimeScaleZero_AndResumeRestores()
        {
            yield return null;

            gameManager.SetState(GameState.Paused);
            Assert.AreEqual(0f, Time.timeScale, "Time.timeScale must be 0 when paused.");
            Assert.AreEqual(GameState.Paused, gameManager.CurrentState);

            yield return null;

            gameManager.SetState(GameState.Playing);
            Assert.AreEqual(1f, Time.timeScale, "Time.timeScale must be restored to 1 on resume.");
            Assert.AreEqual(GameState.Playing, gameManager.CurrentState);
        }
    }
}
