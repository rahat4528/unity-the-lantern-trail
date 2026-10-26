using System;
using System.Collections.Generic;
using UnityEngine;
using LanternTrail.Save;

namespace LanternTrail.Core
{
    public enum GameLocation
    {
        VillagePaths = 1,
        MistyRiver = 2,
        LighthouseHill = 3
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        ReadingNote,
        Ending
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("State")]
        [SerializeField] private GameState currentState = GameState.Playing;
        [SerializeField] private GameLocation currentLocation = GameLocation.VillagePaths;

        [Header("Run Stats")]
        [SerializeField] private int lanternsCollected = 0;
        [SerializeField] private int goldenLanternsFound = 0;
        [SerializeField] private int notesReadCount = 0;
        [SerializeField] private float timeElapsed = 0f;

        private readonly HashSet<string> collectedIds = new HashSet<string>();
        private readonly List<JournalEntry> journalEntries = new List<JournalEntry>();

        public event Action<GameState> OnStateChanged;
        public event Action<GameLocation> OnLocationChanged;
        public event Action<int, int, int, int, float> OnGameFinished;

        public GameState CurrentState => currentState;
        public GameLocation CurrentLocation => currentLocation;
        public int LanternsCollected => lanternsCollected;
        public int GoldenLanternsFound => goldenLanternsFound;
        public int NotesReadCount => notesReadCount;
        public float TimeElapsed => timeElapsed;
        public IReadOnlyList<JournalEntry> Journal => journalEntries;

        [Serializable]
        public struct JournalEntry
        {
            public string id;
            public string title;
            public string body;
            public GameLocation location;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (currentState == GameState.Playing)
            {
                timeElapsed += Time.deltaTime;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // Section 3.4: Auto-pause when losing focus or backgrounded
            if (!hasFocus && currentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && currentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        public void StartNewRun()
        {
            lanternsCollected = 0;
            goldenLanternsFound = 0;
            notesReadCount = 0;
            timeElapsed = 0f;
            collectedIds.Clear();
            journalEntries.Clear();
            currentLocation = GameLocation.VillagePaths;
            SetState(GameState.Playing);
            OnLocationChanged?.Invoke(currentLocation);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            Time.timeScale = (newState == GameState.Paused || newState == GameState.ReadingNote) ? 0f : 1f;
            OnStateChanged?.Invoke(newState);
        }

        public bool TryCollect(string id, bool isGolden)
        {
            if (collectedIds.Contains(id)) return false;
            collectedIds.Add(id);

            if (isGolden) goldenLanternsFound++;
            else lanternsCollected++;

            return true;
        }

        public bool IsCollected(string id) => collectedIds.Contains(id);

        public void AddJournalNote(string id, string title, string body)
        {
            if (!collectedIds.Contains(id))
            {
                collectedIds.Add(id);
                notesReadCount++;
                journalEntries.Add(new JournalEntry
                {
                    id = id,
                    title = title,
                    body = body,
                    location = currentLocation
                });
            }
        }

        public void SetLocation(GameLocation newLocation)
        {
            currentLocation = newLocation;
            OnLocationChanged?.Invoke(currentLocation);
        }

        public void CompleteGame()
        {
            SetState(GameState.Ending);
            int stars = StarRating.Calculate(goldenLanternsFound);
            SaveSystem.SaveBestResult(stars, lanternsCollected, goldenLanternsFound, notesReadCount, timeElapsed);
            OnGameFinished?.Invoke(stars, lanternsCollected, goldenLanternsFound, notesReadCount, timeElapsed);
        }
    }
}
