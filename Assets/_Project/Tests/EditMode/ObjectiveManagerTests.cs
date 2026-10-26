using NUnit.Framework;
using UnityEngine;
using LanternTrail.Core;

namespace LanternTrail.Tests.EditMode
{
    [TestFixture]
    public class ObjectiveManagerTests
    {
        private GameObject go;
        private ObjectiveManager objectiveManager;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("ObjectiveManager_Test");
            objectiveManager = go.AddComponent<ObjectiveManager>();
        }

        [TearDown]
        public void TearDown()
        {
            if (go != null) Object.DestroyImmediate(go);
        }

        [Test]
        public void InitializeObjective_SetsTargetAndResetsProgress()
        {
            objectiveManager.InitializeObjective("Test Objective", "Collect 3 items", 3);

            Assert.AreEqual(3, objectiveManager.TargetCount);
            Assert.AreEqual(0, objectiveManager.CurrentCount);
            Assert.IsFalse(objectiveManager.IsCompleted);
        }

        [Test]
        public void AddProgress_IncrementsCounter_AndInvokesEvent()
        {
            int reportedCurrent = -1;
            int reportedTarget = -1;
            objectiveManager.OnProgressChanged += (cur, tar) =>
            {
                reportedCurrent = cur;
                reportedTarget = tar;
            };

            objectiveManager.InitializeObjective("Test", "Desc", 3);
            objectiveManager.AddProgress(1);

            Assert.AreEqual(1, objectiveManager.CurrentCount);
            Assert.AreEqual(1, reportedCurrent);
            Assert.AreEqual(3, reportedTarget);
        }

        [Test]
        public void AddProgress_ReachingTarget_CompletesObjective()
        {
            bool completedFired = false;
            objectiveManager.OnObjectiveCompleted += () => completedFired = true;

            objectiveManager.InitializeObjective("Test", "Desc", 3);
            objectiveManager.AddProgress(1);
            objectiveManager.AddProgress(2);

            Assert.IsTrue(objectiveManager.IsCompleted);
            Assert.IsTrue(completedFired, "OnObjectiveCompleted event must fire when target is reached.");
        }
    }
}
