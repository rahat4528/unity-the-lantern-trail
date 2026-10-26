using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LanternTrail.Core;

namespace LanternTrail.Tests.PlayMode
{
    public class SceneLoaderGuardTests
    {
        private GameObject loaderGo;
        private SceneLoader sceneLoader;

        [SetUp]
        public void SetUp()
        {
            loaderGo = new GameObject("SceneLoader_Test");
            sceneLoader = loaderGo.AddComponent<SceneLoader>();
        }

        [TearDown]
        public void TearDown()
        {
            if (loaderGo != null) Object.Destroy(loaderGo);
        }

        [UnityTest]
        public IEnumerator SceneLoader_ConcurrentRequests_IgnoresSecondCall()
        {
            yield return null;

            bool firstCallAccepted = sceneLoader.LoadSceneAsync(0);
            bool secondCallAccepted = sceneLoader.LoadSceneAsync(1);

            Assert.IsTrue(firstCallAccepted, "Initial scene load request should be accepted.");
            Assert.IsFalse(secondCallAccepted, "Concurrent second load request must be rejected while loading.");
        }
    }
}
