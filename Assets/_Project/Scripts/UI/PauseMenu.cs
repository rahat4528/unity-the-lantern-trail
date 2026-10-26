using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LanternTrail.Core;

namespace LanternTrail.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button journalButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Sub-screens")]
        [SerializeField] private JournalScreen journalScreen;
        [SerializeField] private SettingsMenu settingsMenu;

        private void Awake()
        {
            if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
            if (journalButton != null) journalButton.onClick.AddListener(OpenJournal);
            if (restartButton != null) restartButton.onClick.AddListener(RestartLocation);
            if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            gameObject.SetActive(false);
        }

        private void Update()
        {
            // Section 3.4: Android Back button (Escape) toggles pause or backs out of sub-menus
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleBackButton();
            }
        }

        private void HandleBackButton()
        {
            if (journalScreen != null && journalScreen.gameObject.activeSelf)
            {
                journalScreen.CloseJournal();
                return;
            }

            if (settingsMenu != null && settingsMenu.gameObject.activeSelf)
            {
                settingsMenu.Close();
                return;
            }

            if (gameObject.activeSelf)
            {
                Resume();
            }
            else
            {
                OpenPauseMenu();
            }
        }

        public void OpenPauseMenu()
        {
            gameObject.SetActive(true);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(GameState.Paused);
            }
        }

        public void Resume()
        {
            gameObject.SetActive(false);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetState(GameState.Playing);
            }
        }

        public void OpenJournal()
        {
            if (journalScreen != null)
            {
                journalScreen.OpenJournal();
            }
        }

        public void OpenSettings()
        {
            if (settingsMenu != null)
            {
                settingsMenu.Open();
            }
        }

        public void RestartLocation()
        {
            Time.timeScale = 1f;
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync("MainMenu");
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
