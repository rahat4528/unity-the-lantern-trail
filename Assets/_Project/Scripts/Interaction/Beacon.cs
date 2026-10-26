using System.Collections;
using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.Audio;

namespace LanternTrail.Interaction
{
    public class Beacon : MonoBehaviour, IInteractable
    {
        [Header("Flame & Light")]
        [SerializeField] private ParticleSystem beaconFire;
        [SerializeField] private Light beaconLight;
        [SerializeField] private float endingDelay = 2.5f;

        private bool isIgnited = false;

        public bool IsIgnited => isIgnited;

        private void Start()
        {
            if (beaconFire != null) beaconFire.Stop();
            if (beaconLight != null) beaconLight.enabled = false;
        }

        public string GetPromptText() => isIgnited ? "Beacon is Burning" : "Ignite Beacon";

        public bool CanInteract() => !isIgnited;

        public Vector3 GetWorldPosition() => transform.position;

        public void Interact(InteractionDetector detector)
        {
            if (isIgnited) return;
            isIgnited = true;

            if (beaconFire != null) beaconFire.Play();
            if (beaconLight != null) beaconLight.enabled = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayBeaconIgnite();
            }

            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.CompleteObjective();
            }

            StartCoroutine(FinishGameRoutine());
        }

        private IEnumerator FinishGameRoutine()
        {
            yield return new WaitForSeconds(endingDelay);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteGame();
            }

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync("Ending");
            }
        }
    }
}
