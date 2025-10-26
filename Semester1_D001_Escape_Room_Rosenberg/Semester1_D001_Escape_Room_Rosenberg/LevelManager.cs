using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg
{ 

    /// <summary>
    /// Coordinates level progression, transitions, and resets.
    /// The LevelManager acts as the central progression controller
    /// that initializes, restarts, and advances game levels.
    /// </summary>
    internal sealed class LevelManager
    {
        // === Dependencies ===
        private readonly LevelManagerDependencies _deps;

        // === Internal State ===
        private int _currentLevel = 1;
        private int _score;
        private int _requiredKeys = 5;

        // === Constructor ===
        public LevelManager(LevelManagerDependencies levelManagerDependencies)
        {
            _deps = levelManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(LevelManager)}: Initialized successfully.");
        }

        public int RequiredKeys => _requiredKeys;
        public int CurrentLvl=> _currentLevel;
        public int Score=>_score;

        public void AddScore(int ammount)
        {
            _score += ammount;
        }

        public void AddLvl()
        {
            _currentLevel++;
        }

        public void NewLevel(int inventoryScore)
        {
            _requiredKeys += 3;
            AddScore(inventoryScore);
        }
    /*
        // === Initialize a specific level ===
        public void InitializeLevel(int levelIndex)
        {
            _currentLevel = levelIndex;

            _deps.Diagnostics.AddCheck($"{nameof(LevelManager)}: Initializing Level {_currentLevel}...");

            // Clear previous objects and board
            _deps.GameObject.ClearAll();
            _deps.GameBoard.ClearBoard();

            // Build the new board & spawn objects
            _deps.GameBoard.LoadLevelLayout(_currentLevel);
            _deps.Spawn.SpawnObjectsForLevel(_currentLevel);

            _deps.Diagnostics.AddCheck($"{nameof(LevelManager)}: Level {_currentLevel} initialized successfully.");
        }

        // === Progress to the next level ===
        public void GoToNextLevel()
        {
            _currentLevel++;
            _deps.Diagnostics.AddCheck($"{nameof(LevelManager)}: Transitioning to Level {_currentLevel}...");
            InitializeLevel(_currentLevel);
        }

        // === Restart the current level ===
        public void ResetCurrentLevel()
        {
            _deps.Diagnostics.AddWarning($"{nameof(LevelManager)}: Restarting Level {_currentLevel}...");
            InitializeLevel(_currentLevel);
        }

        // === End game ===
        public void EndGame()
        {
            _deps.Diagnostics.AddCheck($"{nameof(LevelManager)}: Final Level reached. Game completed!");
            _deps.UI.DisplayEndMessage("Congratulations, you escaped the tower!");
        }

        // === Accessors ===
        public int CurrentLevel => _currentLevel;
    }
    */
}
