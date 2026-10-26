using System;
using System.Collections.Generic;
using UnityEngine;
using LanternTrail.Interaction;

namespace LanternTrail.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        public static InteractionDetector Instance { get; private set; }

        [Header("Detection Settings")]
        [SerializeField] private float detectionRadius = 1.8f;
        [SerializeField] private LayerMask interactableLayer = ~0;

        private IInteractable nearestInteractable;
        private readonly List<IInteractable> inRangeInteractables = new List<IInteractable>();

        public event Action<IInteractable> OnTargetChanged;
        public IInteractable CurrentTarget => nearestInteractable;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            UpdateNearestTarget();

            // Keyboard shortcut for Editor testing
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                TriggerInteract();
            }
        }

        private void UpdateNearestTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);
            inRangeInteractables.Clear();

            foreach (var col in colliders)
            {
                var interactable = col.GetComponent<IInteractable>() ?? col.GetComponentInParent<IInteractable>();
                if (interactable != null && interactable.CanInteract())
                {
                    inRangeInteractables.Add(interactable);
                }
            }

            IInteractable bestTarget = null;
            float closestDist = float.MaxValue;

            foreach (var item in inRangeInteractables)
            {
                float dist = Vector3.Distance(transform.position, item.GetWorldPosition());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    bestTarget = item;
                }
            }

            if (bestTarget != nearestInteractable)
            {
                nearestInteractable = bestTarget;
                OnTargetChanged?.Invoke(nearestInteractable);
            }
        }

        public void TriggerInteract()
        {
            if (nearestInteractable != null && nearestInteractable.CanInteract())
            {
                nearestInteractable.Interact(this);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
