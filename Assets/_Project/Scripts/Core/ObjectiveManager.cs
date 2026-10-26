using System;
using UnityEngine;

namespace LanternTrail.Core
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { get; private set; }

        [Header("Location Objectives")]
        [SerializeField] private int targetCount = 3;
        [SerializeField] private int currentCount = 0;
        [SerializeField] private bool isCompleted = false;

        public event Action<string, string> OnObjectiveChanged;
        public event Action<int, int> OnProgressChanged;
        public event Action OnObjectiveCompleted;

        public int CurrentCount => currentCount;
        public int TargetCount => targetCount;
        public bool IsCompleted => isCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void InitializeObjective(string title, string description, int target)
        {
            targetCount = target;
            currentCount = 0;
            isCompleted = false;
            OnObjectiveChanged?.Invoke(title, description);
            OnProgressChanged?.Invoke(currentCount, targetCount);
        }

        public void AddProgress(int amount = 1)
        {
            if (isCompleted) return;

            currentCount = Mathf.Min(targetCount, currentCount + amount);
            OnProgressChanged?.Invoke(currentCount, targetCount);

            if (currentCount >= targetCount)
            {
                CompleteObjective();
            }
        }

        public void ResetProgress()
        {
            currentCount = 0;
            isCompleted = false;
            OnProgressChanged?.Invoke(currentCount, targetCount);
        }

        public void CompleteObjective()
        {
            if (isCompleted) return;
            isCompleted = true;
            OnObjectiveCompleted?.Invoke();
        }
    }
}
