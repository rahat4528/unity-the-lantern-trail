using UnityEngine;
using UnityEngine.UI;
using LanternTrail.Core;

namespace LanternTrail.UI
{
    public class JournalScreen : MonoBehaviour
    {
        [Header("UI Containers")]
        [SerializeField] private Transform notesContainer;
        [SerializeField] private GameObject noteEntryPrefab;
        [SerializeField] private Text emptyStateText;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (closeButton != null) closeButton.onClick.AddListener(CloseJournal);
            gameObject.SetActive(false);
        }

        public void OpenJournal()
        {
            gameObject.SetActive(true);
            PopulateJournal();
        }

        public void CloseJournal()
        {
            gameObject.SetActive(false);
        }

        private void PopulateJournal()
        {
            if (notesContainer == null || GameManager.Instance == null) return;

            // Clear old entry transforms
            foreach (Transform child in notesContainer)
            {
                Destroy(child.gameObject);
            }

            var entries = GameManager.Instance.Journal;
            if (entries.Count == 0)
            {
                if (emptyStateText != null) emptyStateText.gameObject.SetActive(true);
                return;
            }

            if (emptyStateText != null) emptyStateText.gameObject.SetActive(false);

            foreach (var entry in entries)
            {
                if (noteEntryPrefab != null)
                {
                    GameObject item = Instantiate(noteEntryPrefab, notesContainer);
                    Text[] texts = item.GetComponentsInChildren<Text>();
                    if (texts.Length >= 2)
                    {
                        texts[0].text = entry.title;
                        texts[1].text = entry.body;
                    }
                }
            }
        }
    }
}
