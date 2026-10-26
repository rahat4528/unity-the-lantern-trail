using System.Collections;
using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.Audio;

namespace LanternTrail.Interaction
{
    public class Gate : MonoBehaviour, IInteractable
    {
        [Header("Gate Settings")]
        [SerializeField] private bool isLocked = true;
        [SerializeField] private Collider gateCollider;
        [SerializeField] private Transform doorLeft;
        [SerializeField] private Transform doorRight;
        [SerializeField] private float openAngle = 85f;
        [SerializeField] private float openDuration = 1.0f;

        public bool IsLocked => isLocked;

        private void Start()
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.OnObjectiveCompleted += UnlockGate;
            }

            if (gateCollider == null) gateCollider = GetComponent<Collider>();
            if (gateCollider != null) gateCollider.isTrigger = !isLocked;
        }

        private void OnDestroy()
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.OnObjectiveCompleted -= UnlockGate;
            }
        }

        public string GetPromptText() => isLocked ? "Gate is Locked" : "Exit to Next Area";

        public bool CanInteract() => !isLocked;

        public Vector3 GetWorldPosition() => transform.position;

        public void UnlockGate()
        {
            if (!isLocked) return;
            isLocked = false;

            if (gateCollider != null) gateCollider.isTrigger = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGateUnlock();
            }

            StartCoroutine(OpenGateRoutine());
        }

        public void Interact(InteractionDetector detector)
        {
            if (isLocked) return;

            // Trigger scene transition to next location
            var exitZone = GetComponent<ExitZone>();
            if (exitZone != null)
            {
                exitZone.TriggerTransition();
            }
        }

        private IEnumerator OpenGateRoutine()
        {
            float t = 0f;
            Quaternion lStart = doorLeft != null ? doorLeft.localRotation : Quaternion.identity;
            Quaternion rStart = doorRight != null ? doorRight.localRotation : Quaternion.identity;
            Quaternion lTarget = lStart * Quaternion.Euler(0f, -openAngle, 0f);
            Quaternion rTarget = rStart * Quaternion.Euler(0f, openAngle, 0f);

            while (t < openDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.SmoothStep(0f, 1f, t / openDuration);
                if (doorLeft != null) doorLeft.localRotation = Quaternion.Slerp(lStart, lTarget, progress);
                if (doorRight != null) doorRight.localRotation = Quaternion.Slerp(rStart, rTarget, progress);
                yield return null;
            }
        }
    }
}
