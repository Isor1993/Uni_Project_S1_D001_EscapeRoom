/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : LevelManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Controls level progression, transitions, and resets within the game loop.
* The LevelManager acts as the central system that initializes new levels,
* increases difficulty, and clears previous data before generating the next map.
*
* Responsibilities:
* - Manage current level state and progression
* - Control required key fragments per level
* - Reset and reinitialize game systems for new levels
* - Coordinate with SpawnManager and GameObjectManager for cleanup
* - Trigger console updates and diagnostics logs
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Coordinates level progression, transitions, and resets.
    /// The <see cref="LevelManager"/> manages level state, increases difficulty,
    /// and clears previous entities to prepare for new gameplay sessions.
    /// </summary>
    internal sealed class LevelManager
    {
        // === Dependencies ===
        private readonly LevelManagerDependencies _deps;

        // === Fields ===
        private int _currentLevel = 1;

        private int _requiredKeys = 5;
        private bool _isNextLvl = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelManager"/> class.
        /// </summary>
        /// <param name="levelManagerDependencies">
        /// Provides access to dependent managers such as <see cref="InventoryManager"/>,
        /// <see cref="GameObjectManager"/>, <see cref="SpawnManager"/>, and <see cref="DiagnosticsManager"/>.
        /// </param>
        public LevelManager(LevelManagerDependencies levelManagerDependencies)
        {
            _deps = levelManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(LevelManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the number of key fragments required to unlock the next level.
        /// </summary>
        public int RequiredKeys => _requiredKeys;

        /// <summary>
        /// Gets the currently active level index.
        /// </summary>
        public int CurrentLvl => _currentLevel;

        /// <summary>
        /// Gets or sets a flag indicating whether the next level should start.
        /// </summary>
        public bool IsNextLvl { get => _isNextLvl; set => _isNextLvl = value; }

        /// <summary>
        /// Increments the current level counter.
        /// </summary>
        public void AddLvl()
        {
            _currentLevel++;
        }

        /// <summary>
        /// Prepares and initializes the next level when progression requirements are met.
        /// </summary>
        /// <param name="inventoryScore">
        /// The player's current key fragment score used to determine level transition readiness.
        /// </param>
        /// <remarks>
        /// This method:
        /// <list type="number">
        /// <item><description>Removes collected key fragments from the inventory.</description></item>
        /// <item><description>Increases required fragments for future levels.</description></item>
        /// <item><description>Adjusts map size and entity counts dynamically.</description></item>
        /// <item><description>Clears all spawned and registered objects.</description></item>
        /// <item><description>Logs transition status for diagnostics.</description></item>
        /// </list>
        /// </remarks>
        public void NewLevel(int inventoryScore)
        {
            _deps.Inventory.RemoveKeyFragment(_requiredKeys);

            _requiredKeys += 3;

            AddLvl();

            Program.NewArraySizeX += 3;
            Program.NpcAmount += 3;
            Program.KeyAmount += 3;

            _isNextLvl = true;

            _deps.Spawn.ClearAll();
            _deps.GameObject.ClearAll();

            Console.Clear();
            _deps.Diagnostic.AddCheck($"{nameof(LevelManager)}.{nameof(NewLevel)}: Level {_currentLevel} initialized. Required keys now {_requiredKeys}.");
        }
    }
}