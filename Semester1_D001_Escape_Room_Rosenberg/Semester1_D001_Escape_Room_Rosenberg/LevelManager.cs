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
        private int _requiredKeys = 5;
        private bool _isNextLvl=false;

        // === Constructor ===
        public LevelManager(LevelManagerDependencies levelManagerDependencies)
        {
            _deps = levelManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(LevelManager)}: Initialized successfully.");
        }

        public int RequiredKeys => _requiredKeys;
        public int CurrentLvl => _currentLevel;
        public bool IsNextLvl { get => _isNextLvl;set => _isNextLvl = value; }
      

      

        public void AddLvl()
        {
            _currentLevel++;
        }

        public void NewLevel(int inventoryScore)
        {
            _deps.Inventory.RemoveKeyFragment(_requiredKeys);
            _requiredKeys += 3;
            AddLvl();
            Program.NewArraySizeX += 3;
            Program.NpcAmount += 3;
            Program.KeyAmount += 3;
            _isNextLvl= true;
            _deps.Spawn.ClearAll();
            _deps.GameObject.ClearAll();            
            Console.Clear();

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
}