using UnityEngine;
using UnityEngine.UI;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.Interaction;

namespace LanternTrail.UI
{
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }

        [Header("Top HUD")]
        [SerializeField] private Text objectiveTitleText;
        [SerializeField] private Text objectiveCounterText;
        [SerializeField] private Text locationBadgeText;
        [SerializeField] private Button pauseButton;

        [Header("Controls")]
        [SerializeField] private Button interactButton;
        [SerializeField] private Text interactButtonText;
        [SerializeField] private Button hintButton;
        [SerializeField] private VirtualJoystick joystick;

        [Header("Note Modal")]
        [SerializeField] private GameObject noteModalRoot;
        [SerializeField] private Text noteModalTitle;
        [SerializeField] private Text noteModalBody;
        [SerializeField] private Button noteModalCloseButton;

        [Header("Pause & Journal")]
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private JournalScreen journalScreen;

        private void Awake()
        {
            Instance = this;

            if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseClicked);
            if (hintButton != null) hintButton.onClick.AddListener(OnHintClicked);
            if (interactButton != null) interactButton.onClick.AddListener(OnInteractClicked);
            if (noteModalCloseButton != null) noteModalCloseButton.onClick.AddListener(CloseNoteModal);

            if (noteModalRoot != null) noteModalRoot.SetActive(false);
            if (interactButton != null) interactButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.OnObjectiveChanged += UpdateObjectiveText;
                ObjectiveManager.Instance.OnProgressChanged += UpdateProgressText;
                ObjectiveManager.Instance.OnObjectiveCompleted += HandleObjectiveCompleted;
            }

            if (InteractionDetector.Instance != null)
            {
                InteractionDetector.Instance.OnTargetChanged += HandleTargetChanged;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLocationChanged += UpdateLocationBadge;
            }
        }

        private void OnDestroy()
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.OnObjectiveChanged -= UpdateObjectiveText;
                ObjectiveManager.Instance.OnProgressChanged -= UpdateProgressText;
                ObjectiveManager.Instance.OnObjectiveCompleted -= HandleObjectiveCompleted;
            }

            if (InteractionDetector.Instance != null)
            {
                InteractionDetector.Instance.OnTargetChanged -= HandleTargetChanged;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLocationChanged -= UpdateLocationBadge;
            }
        }

        private void UpdateObjectiveText(string title, string description)
        {
            if (objectiveTitleText != null) objectiveTitleText.text = title;
        }

        private void UpdateProgressText(int current, int target)
        {
            if (objectiveCounterText != null)
            {
                objectiveCounterText.text = target > 1 ? $"{current} / {target}" : "";
            }
        }

        private void HandleObjectiveCompleted()
        {
            if (objectiveTitleText != null)
            {
                objectiveTitleText.text = "✔ Objective Complete! Proceed onward.";
            }
            if (objectiveCounterText != null)
            {
                objectiveCounterText.text = "";
            }
        }

        private void UpdateLocationBadge(GameLocation loc)
        {
            if (locationBadgeText != null)
            {
                locationBadgeText.text = loc switch
                {
                    GameLocation.VillagePaths => "VILLAGE PATHS",
                    GameLocation.MistyRiver => "MISTY RIVER",
                    GameLocation.LighthouseHill => "LIGHTHOUSE HILL",
                    _ => "UNKNOWN"
                };
            }
        }

        private void HandleTargetChanged(IInteractable target)
        {
            if (interactButton == null) return;

            if (target != null && target.CanInteract())
            {
                interactButton.gameObject.SetActive(true);
                if (interactButtonText != null) interactButtonText.text = target.GetPromptText();
            }
            else
            {
                interactButton.gameObject.SetActive(false);
            }
        }

        private void OnInteractClicked()
        {
            if (InteractionDetector.Instance != null)
            {
                InteractionDetector.Instance.TriggerInteract();
            }
        }

        private void OnHintClicked()
        {
            if (WispCompanion.Instance != null)
            {
                WispCompanion.Instance.TriggerHint();
            }
        }

        private void OnPauseClicked()
        {
            if (pauseMenu != null)
            {
                pauseMenu.OpenPauseMenu();
            }
        }

        public void DisplayNoteModal(string title, string body)
        {
            if (noteModalTitle != null) noteModalTitle.text = title;
            if (noteModalBody != null) noteModalBody.text = body;
            if (noteModalRoot != null) noteModalRoot.SetActive(true);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(GameState.ReadingNote);
            }
        }

        public void CloseNoteModal()
        {
            if (noteModalRoot != null) noteModalRoot.SetActive(false);

            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.ReadingNote)
            {
                GameManager.Instance.SetState(GameState.Playing);
            }
        }
    }
}
