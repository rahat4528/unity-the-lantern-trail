using UnityEngine;
using LanternTrail.Core;

namespace LanternTrail.Interaction
{
    public class WispCompanion : MonoBehaviour
    {
        public static WispCompanion Instance { get; private set; }

        [Header("Follow Settings")]
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Vector3 followOffset = new Vector3(0.9f, 1.4f, -0.4f);
        [SerializeField] private float followSpeed = 3.2f;
        [SerializeField] private float bobSpeed = 3.0f;
        [SerializeField] private float bobHeight = 0.25f;

        [Header("Hint Configuration")]
        [SerializeField] private float autoHintInterval = 45f;
        [SerializeField] private float guideTime = 4.0f;

        private Vector3 currentVelocity = Vector3.zero;
        private Transform currentHintTarget = null;
        private float lastProgressTime = 0f;
        private bool isGuiding = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            lastProgressTime = Time.time;

            if (playerTarget == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTarget = player.transform;
            }

            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.OnProgressChanged += (c, t) => ResetHintTimer();
                ObjectiveManager.Instance.OnObjectiveCompleted += ResetHintTimer;
            }
        }

        private void Update()
        {
            if (playerTarget == null) return;

            // Auto-hint trigger after 45 seconds of no progress
            if (!isGuiding && Time.time - lastProgressTime > autoHintInterval)
            {
                if (ObjectiveManager.Instance != null && !ObjectiveManager.Instance.IsCompleted)
                {
                    TriggerHint();
                }
            }

            Vector3 destination;
            if (isGuiding && currentHintTarget != null)
            {
                destination = currentHintTarget.position + Vector3.up * 1.5f;
            }
            else
            {
                float bobY = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
                destination = playerTarget.position + followOffset + new Vector3(0f, bobY, 0f);
            }

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref currentVelocity, 1f / followSpeed);
        }

        public void TriggerHint()
        {
            ResetHintTimer();
            FindObjectiveTarget();

            if (currentHintTarget != null)
            {
                isGuiding = true;
                CancelInvoke(nameof(ReturnToPlayer));
                Invoke(nameof(ReturnToPlayer), guideTime);
            }
        }

        private void ReturnToPlayer()
        {
            isGuiding = false;
        }

        private void ResetHintTimer()
        {
            lastProgressTime = Time.time;
        }

        private void FindObjectiveTarget()
        {
            currentHintTarget = null;

            // Priority 1: Uncollected regular lanterns
            var collectibles = FindObjectsOfType<Collectible>();
            float closestDist = float.MaxValue;

            foreach (var c in collectibles)
            {
                if (!c.IsGolden && !c.IsCollected && c.gameObject.activeInHierarchy)
                {
                    float dist = Vector3.Distance(transform.position, c.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        currentHintTarget = c.transform;
                    }
                }
            }

            // Priority 2: Unactivated pressure plate
            if (currentHintTarget == null)
            {
                var plates = FindObjectsOfType<PressurePlate>();
                foreach (var p in plates)
                {
                    if (!p.IsActivated)
                    {
                        currentHintTarget = p.transform;
                        break;
                    }
                }
            }

            // Priority 3: Beacon or Gate if objective complete
            if (currentHintTarget == null)
            {
                var beacon = FindObjectOfType<Beacon>();
                if (beacon != null && !beacon.IsIgnited)
                {
                    currentHintTarget = beacon.transform;
                }
                else
                {
                    var gate = FindObjectOfType<Gate>();
                    if (gate != null) currentHintTarget = gate.transform;
                }
            }
        }
    }
}
