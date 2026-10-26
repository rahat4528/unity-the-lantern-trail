using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;

namespace LanternTrail.Interaction
{
    public class ExitZone : MonoBehaviour, IInteractable
    {
        [Header("Target Destination")]
        [SerializeField] private string targetSceneName = "Location_02";
        [SerializeField] private GameLocation nextLocation = GameLocation.MistyRiver;
        [SerializeField] private bool requireObjectiveComplete = true;

        public string GetPromptText() => "Travel Onward";

        public bool CanInteract()
        {
            if (requireObjectiveComplete && ObjectiveManager.Instance != null)
            {
                return ObjectiveManager.Instance.IsCompleted;
            }
            return true;
        }

        public Vector3 GetWorldPosition() => transform.position;

        public void Interact(InteractionDetector detector)
        {
            TriggerTransition();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && CanInteract())
            {
                TriggerTransition();
            }
        }

        public void TriggerTransition()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetLocation(nextLocation);
            }

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync(targetSceneName);
            }
        }
    }
}
