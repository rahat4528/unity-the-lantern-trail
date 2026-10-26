using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.Audio;

namespace LanternTrail.Interaction
{
    public class Collectible : MonoBehaviour, IInteractable
    {
        [Header("Identity")]
        [SerializeField] private string collectibleId = "L1_Lantern_01";
        [SerializeField] private bool isGolden = false;

        [Header("Visual & Effects")]
        [SerializeField] private GameObject visualModel;
        [SerializeField] private ParticleSystem collectParticles;
        [SerializeField] private Light lanternLight;
        [SerializeField] private float bobAmplitude = 0.15f;
        [SerializeField] private float bobSpeed = 2.5f;

        private Collider itemCollider;
        private bool isCollected = false;
        private Vector3 startPos;

        public string CollectibleId => collectibleId;
        public bool IsGolden => isGolden;
        public bool IsCollected => isCollected;

        private void Awake()
        {
            itemCollider = GetComponent<Collider>();
            startPos = transform.position;
        }

        private void Start()
        {
            // Restore state if re-entering scene
            if (GameManager.Instance != null && GameManager.Instance.IsCollected(collectibleId))
            {
                ApplyCollectedState();
            }
        }

        private void Update()
        {
            if (isCollected) return;

            // Idle floating motion
            float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            transform.position = startPos + new Vector3(0f, yOffset, 0f);
        }

        public string GetPromptText() => isGolden ? "Collect Golden Lantern" : "Collect Lantern";

        public bool CanInteract() => !isCollected;

        public Vector3 GetWorldPosition() => transform.position;

        public void Interact(InteractionDetector detector)
        {
            if (isCollected) return;

            if (GameManager.Instance != null)
            {
                bool success = GameManager.Instance.TryCollect(collectibleId, isGolden);
                if (!success) return; // Prevent double reward
            }

            if (!isGolden && ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.AddProgress(1);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCollect(isGolden);
            }

            ApplyCollectedState();
        }

        private void ApplyCollectedState()
        {
            isCollected = true;

            if (itemCollider != null) itemCollider.enabled = false;
            if (lanternLight != null) lanternLight.enabled = false;

            if (collectParticles != null)
            {
                collectParticles.transform.SetParent(null);
                collectParticles.Play();
                Destroy(collectParticles.gameObject, 2f);
            }

            if (visualModel != null)
            {
                visualModel.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCollected)
            {
                var detector = other.GetComponent<InteractionDetector>();
                Interact(detector);
            }
        }
    }
}
