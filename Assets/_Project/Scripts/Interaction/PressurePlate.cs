using System;
using System.Collections.Generic;
using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.Audio;

namespace LanternTrail.Interaction
{
    public class PressurePlate : MonoBehaviour, IInteractable
    {
        [Header("Plate Identity")]
        [SerializeField] private string plateName = "North"; // "North", "East", "West"
        [SerializeField] private int requiredOrder = 0;       // 0=N, 1=E, 2=W

        [Header("Visual Feedback")]
        [SerializeField] private Renderer plateRenderer;
        [SerializeField] private Color normalColor = new Color(0.3f, 0.5f, 0.7f, 1f);
        [SerializeField] private Color activeColor = new Color(0.2f, 1.0f, 0.5f, 1f);
        [SerializeField] private Color errorColor = new Color(1.0f, 0.2f, 0.2f, 1f);
        [SerializeField] private Light plateLight;

        private bool isActivated = false;

        // Static puzzle sequence manager
        private static int currentStep = 0;
        private static readonly List<PressurePlate> allPlates = new List<PressurePlate>();

        public bool IsActivated => isActivated;

        private void Awake()
        {
            allPlates.Add(this);
            ResetPlate();
        }

        private void OnDestroy()
        {
            allPlates.Remove(this);
            currentStep = 0;
        }

        public string GetPromptText() => $"Step on {plateName} Plate";

        public bool CanInteract() => !isActivated;

        public Vector3 GetWorldPosition() => transform.position;

        public void Interact(InteractionDetector detector)
        {
            StepOnPlate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isActivated)
            {
                StepOnPlate();
            }
        }

        private void StepOnPlate()
        {
            if (isActivated) return;

            if (requiredOrder == currentStep)
            {
                // Correct step in sequence!
                isActivated = true;
                currentStep++;
                SetColor(activeColor);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPlateStep(true);
                }

                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.AddProgress(1);
                }

                if (currentStep >= 3)
                {
                    // Sequence complete!
                    if (ObjectiveManager.Instance != null)
                    {
                        ObjectiveManager.Instance.CompleteObjective();
                    }
                }
            }
            else
            {
                // Wrong sequence! Flash all plates red and reset
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPlateStep(false);
                }

                currentStep = 0;
                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.ResetProgress();
                }

                foreach (var p in allPlates)
                {
                    p.FlashErrorAndReset();
                }
            }
        }

        public void FlashErrorAndReset()
        {
            isActivated = false;
            SetColor(errorColor);
            CancelInvoke(nameof(ResetPlate));
            Invoke(nameof(ResetPlate), 0.7f);
        }

        public void ResetPlate()
        {
            isActivated = false;
            SetColor(normalColor);
        }

        private void SetColor(Color col)
        {
            if (plateRenderer != null)
            {
                plateRenderer.material.color = col;
            }
            if (plateLight != null)
            {
                plateLight.color = col;
                plateLight.enabled = isActivated;
            }
        }
    }
}
