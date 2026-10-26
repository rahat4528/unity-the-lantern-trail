using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LanternTrail.Core;

namespace LanternTrail.UI
{
    public class EndingScreen : MonoBehaviour
    {
        [Header("Stats Displays")]
        [SerializeField] private Text starsText;
        [SerializeField] private Text lanternsCountText;
        [SerializeField] private Text goldenCountText;
        [SerializeField] private Text notesCountText;
        [SerializeField] private Text timeText;

        [Header("Buttons")]
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            if (playAgainButton != null) playAgainButton.onClick.AddListener(PlayAgain);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(ReturnToMenu);

            if (GameManager.Instance != null)
            {
                DisplayResults(
                    StarRating.Calculate(GameManager.Instance.GoldenLanternsFound),
                    GameManager.Instance.LanternsCollected,
                    GameManager.Instance.GoldenLanternsFound,
                    GameManager.Instance.NotesReadCount,
                    GameManager.Instance.TimeElapsed
                );
            }
        }

        public void DisplayResults(int stars, int lanterns, int golden, int notes, float time)
        {
            if (starsText != null)
            {
                starsText.text = new string('★', stars) + new string('☆', 3 - stars);
            }
            if (lanternsCountText != null) lanternsCountText.text = lanterns.ToString();
            if (goldenCountText != null) goldenCountText.text = golden.ToString();
            if (notesCountText != null) notesCountText.text = notes.ToString();

            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timeText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        public void PlayAgain()
        {
            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewRun();
            }

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadSceneAsync("Location_01");
            }
            else
            {
                SceneManager.LoadScene("Location_01");
            }
        }

        public void ReturnToMenu()
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
