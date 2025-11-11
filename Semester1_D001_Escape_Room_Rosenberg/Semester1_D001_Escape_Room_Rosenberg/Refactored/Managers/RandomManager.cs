/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : RandomManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Provides centralized randomization utilities for gameplay systems.
* Handles randomized selection, list shuffling, and position generation
* with integrated diagnostic logging for debugging and reproducibility.
*
* Responsibilities:
* - Supplies global random generator for all systems
* - Selects random positions for object spawning
* - Generates randomized subsets of collections (Fisher–Yates shuffle)
* - Logs all randomization-related actions via DiagnosticsManager
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Provides global randomization utilities for the entire game.
    /// Supports randomized position generation, shuffled lists,
    /// and diagnostic logging for controlled randomness.
    /// </summary>
    internal class RandomManager
    {
        // === Dependencies ===
        private readonly DiagnosticsManager _deps;

        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomManager"/> class.
        /// </summary>
        /// <param name="Diagnostics">
        /// Reference to the <see cref="DiagnosticsManager"/> responsible for reporting
        /// randomization-related checks, warnings, and initialization status.
        /// </param>
        public RandomManager(DiagnosticsManager Diagnostics)
        {
            _random = new Random();
            _deps = Diagnostics;
            _deps.AddCheck($"{nameof(RandomManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the internal <see cref="Random"/> generator instance
        /// used for all randomization operations across the system.
        /// </summary>
        public Random Random => _random;

        /// <summary>
        /// Selects a random position from a list of coordinate tuples.
        /// If the provided list is null or empty, a default fallback position is returned.
        /// </summary>
        /// <param name="list">
        /// List of available positions (as <c>(y, x)</c> tuples) to select from.
        /// </param>
        /// <returns>
        /// A randomly chosen position tuple.
        /// Returns <c>(0, 5)</c> if the list is null or empty.
        /// </returns>
        /// <remarks>
        /// This method is primarily used by the <see cref="SpawnManager"/>
        /// to determine random spawn locations for dynamic entities.
        /// </remarks>
        public (int y, int x) RandomPositionFromList(List<(int y, int x)> list)
        {
            (int y, int x) defaultPosition = (0, 5);
            if (list == null || list.Count == 0)
            {
                _deps.AddCheck($"{nameof(RandomManager)}.{nameof(RandomPositionFromList)}:The list was null or empty — using default position {defaultPosition}.");
                return defaultPosition;
            }

            int randomIndex = Random.Next(0, list.Count);
            return list[randomIndex];
        }

        /// <summary>
        /// Returns a randomized subset of elements from the given list.
        /// </summary>
        /// <typeparam name="T">
        /// The element type contained within the list.
        /// </typeparam>
        /// <param name="list">
        /// The source list from which random elements are selected.
        /// </param>
        /// <param name="amount">
        /// The number of random elements to return.
        /// If the amount exceeds the list size, the entire shuffled list is returned.
        /// </param>
        /// <returns>
        /// A new <see cref="List{T}"/> containing the randomly selected elements.
        /// Returns an empty list if the input list is null or empty.
        /// </returns>
        /// <remarks>
        /// This method performs a Fisher–Yates shuffle to ensure unbiased randomness.
        /// Used primarily by the <see cref="SpawnManager"/> for randomized NPC selection.
        /// </remarks>
        public List<T> GetRandomElements<T>(List<T> list, int amount)
        {
            if (list == null || list.Count == 0)
            {
                _deps.AddWarning($"{nameof(RandomManager)}.{nameof(GetRandomElements)}: Source list is empty.");
                return new List<T>();
            }

            List<T> shuffled = new List<T>(list);

            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            return shuffled.GetRange(0, amount);
        }
    }
}