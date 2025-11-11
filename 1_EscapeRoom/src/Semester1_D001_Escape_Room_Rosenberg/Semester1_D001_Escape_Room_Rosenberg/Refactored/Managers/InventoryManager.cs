/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : InventoryManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all player-related inventory data such as key fragments and score points.
* Provides controlled access to add, remove, and reset resources collected during gameplay.
* Ensures consistent diagnostics logging for every modification.
*
* Responsibilities:
* - Manage player inventory values (key fragments, score)
* - Allow controlled addition and removal of resources
* - Support resetting inventory between levels
* - Log all changes through the DiagnosticsManager
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Manages all player-related inventory values such as key fragments and score points.
    /// </summary>
    /// <remarks>
    /// The <see cref="InventoryManager"/> is responsible for tracking player resources collected
    /// during gameplay. It handles operations for increasing or decreasing key fragments and score points,
    /// and allows full inventory resets when starting a new level or session.
    /// Every operation generates diagnostic logs for transparency and debugging.
    /// </remarks>
    internal class InventoryManager
    {
        // === Dependencies ===
        private readonly InventoryDependencies _deps;

        // === Fields ===
        private int _keyFragment;

        private int _score;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryManager"/> class.
        /// </summary>
        /// <param name="inventoryDependencies">
        /// Provides access to the <see cref="DiagnosticsManager"/> used to log inventory changes,
        /// and to any additional systems that depend on inventory state tracking.
        /// </param>
        public InventoryManager(InventoryDependencies inventoryDependencies)
        {
            _deps = inventoryDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the total number of key fragments currently collected by the player.
        /// </summary>
        public int KeyFragment => _keyFragment;

        /// <summary>
        /// Gets the total score points accumulated by the player.
        /// </summary>
        public int Score => _score;

        /// <summary>
        /// Adds a specified number of key fragments to the player’s inventory.
        /// </summary>
        /// <param name="amount">
        /// The amount of key fragments to add.
        /// </param>
        public void AddKeyFragment(int amount)
        {
            _keyFragment += amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(AddKeyFragment)}: Added {amount} key fragments. Total now: {_keyFragment}.");
        }

        /// <summary>
        /// Removes a specified number of key fragments from the player’s inventory.
        /// </summary>
        /// <param name="amount">
        /// The amount of key fragments to remove.
        /// </param>
        public void RemoveKeyFragment(int amount)
        {
            _keyFragment = Math.Max(0, _keyFragment - amount);
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(RemoveKeyFragment)}: Removed {amount} key fragments. Total now: {_keyFragment}.");
        }

        /// <summary>
        /// Adds a specified number of score points to the player’s total score.
        /// </summary>
        /// <param name="amount">
        /// The amount of score points to add.
        /// </param>
        public void AddScorePoints(int amount)
        {
            _score += amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(AddScorePoints)}: Added {amount} score points. Total now: {_score}.");
        }

        /// <summary>
        /// Removes a specified number of score points from the player’s total score.
        /// </summary>
        /// <param name="amount">
        /// The amount of score points to remove.
        /// </param>
        public void RemoveScorePoints(int amount)
        {
            _score -= Math.Max(0, _score - amount);
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(RemoveScorePoints)}: Removed {amount} score points. Total now: {_score}.");
        }

        /// <summary>
        /// Resets all inventory values (key fragments and score) to zero.
        /// </summary>
        public void ResetInventory()
        {
            _keyFragment = 0;
            _score = 0;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(ResetInventory)}: Inventory successfully reset");
        }
    }
}