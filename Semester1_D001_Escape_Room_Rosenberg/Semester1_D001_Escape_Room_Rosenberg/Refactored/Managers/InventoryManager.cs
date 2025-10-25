using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Manages all player-related inventory values such as key fragments and score points.  
    /// </summary>
    /// <remarks>
    /// The <see cref="InventoryManager"/> handles resource tracking within the Escape Room system.  
    /// It maintains the player's collected <b>key fragments</b> and <b>score points</b>,  
    /// offering methods for controlled addition, removal, and resetting of these values.  
    /// Diagnostic messages are automatically logged to support debugging and validation.
    /// </remarks>
    internal class InventoryManager
    {
        // === Dependencies ===
        readonly InventoryDependencies _deps;

        // === Fields ===
        private int _keyFragment;
        private int _score;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryManager"/> class.
        /// </summary>
        /// <param name="inventoryDependencies">
        /// Provides access to required systems, primarily the <see cref="DiagnosticsManager"/> 
        /// for logging inventory-related operations.
        /// </param>
        public InventoryManager(InventoryDependencies inventoryDependencies)
        {
            _deps = inventoryDependencies;           
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}: Initialized inventory manager successfully.");
        }

        /// <summary>
        /// Gets the total number of collected key fragments.
        /// </summary>
        public int KeyFragment => _keyFragment;

        /// <summary>
        /// Gets the current player score.
        /// </summary>
        public int Score => _score;

        /// <summary>
        /// Adds a specified amount of key fragments to the player inventory.
        /// </summary>
        /// <param name="amount">The number of fragments to add.</param>
        public void AddKeyFragment(int amount)
        {
            _keyFragment+=amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(AddKeyFragment)}: Added {amount} key fragments. Total now: {_keyFragment}.");

        }

        /// <summary>
        /// Removes a specified amount of key fragments from the player inventory.
        /// </summary>
        /// <param name="amount">The number of fragments to remove.</param>
        public void RemoveKeyFragment(int amount)
        {
            _keyFragment -=amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(RemoveKeyFragment)}: Removed {amount} key fragments. Total now: {_keyFragment}.");
        }

        /// <summary>
        /// Adds a specified number of score points to the player's total score.
        /// </summary>
        /// <param name="amount">The number of points to add.</param>
        public void AddScorePoints(int amount)
        {
            _score+=amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(AddScorePoints)}: Added {amount} score points. Total now: {_score}.");
        }
        /// <summary>
        /// Removes a specified number of score points from the player's total score.
        /// </summary>
        /// <param name="amount">The number of points to remove.</param>
        public void RemoveScorePoints(int amount)
        {
            _score -= amount;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(RemoveScorePoints)}: Removed {amount} score points. Total now: {_score}.");
        }

        /// <summary>
        /// Resets all inventory values, clearing key fragments and score points.
        /// </summary>
        public void ResetInventory()
        {
            _keyFragment = 0;
            _score = 0;
            _deps.Diagnostic.AddCheck($"{nameof(InventoryManager)}.{nameof(ResetInventory)}: Inventory succsessfully reset");
        }
    }
}
