using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using LanternTrail.Interaction;

namespace LanternTrail.Tests.PlayMode
{
    public class GateUnlockTests
    {
        private GameObject gateGo;
        private Gate gate;
        private BoxCollider col;

        [SetUp]
        public void SetUp()
        {
            gateGo = new GameObject("Gate_Test");
            col = gateGo.AddComponent<BoxCollider>();
            col.isTrigger = false;
            gate = gateGo.AddComponent<Gate>();
        }

        [TearDown]
        public void TearDown()
        {
            if (gateGo != null) Object.Destroy(gateGo);
        }

        [UnityTest]
        public IEnumerator Gate_InitialState_IsLockedAndBlocks()
        {
            yield return null;
            Assert.IsTrue(gate.IsLocked, "Gate must initially be locked.");
            Assert.IsFalse(gate.CanInteract(), "Locked gate cannot be traversed.");
        }

        [UnityTest]
        public IEnumerator Gate_Unlock_BecomesTriggerAndAllowsPassage()
        {
            yield return null;
            gate.UnlockGate();

            Assert.IsFalse(gate.IsLocked, "Gate must be unlocked after UnlockGate().");
            Assert.IsTrue(gate.CanInteract(), "Unlocked gate must allow interaction.");
            Assert.IsTrue(col.isTrigger, "Gate collider must become a trigger to allow passage.");
        }
    }
}
