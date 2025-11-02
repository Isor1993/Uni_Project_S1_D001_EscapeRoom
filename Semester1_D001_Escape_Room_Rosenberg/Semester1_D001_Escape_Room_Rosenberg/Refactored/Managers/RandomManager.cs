using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Provides centralized randomization utilities for gameplay systems.
    /// Handles randomized selection, list shuffling, and position generation
    /// with integrated diagnostic logging.
    /// </summary>
    internal class RandomManager
    {
        // === Dependencies ===        
        private readonly DiagnosticsManager _deps;

        /// <summary>
        /// The internal random number generator instance used by all operations.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomManager"/> class,
        /// creating a new random generator and binding the provided diagnostics manager.
        /// </summary>
        /// <param name="Diagnostics">
        /// The diagnostics system used to report randomization-related checks or warnings.
        /// </param>      
        public RandomManager(DiagnosticsManager Diagnostics)
        {
            _random = new Random();
            _deps = Diagnostics;
            _deps.AddCheck($"{nameof(RandomManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the global random generator instance for other systems.
        /// </summary>
        public Random Random => _random;


        /// <summary>
        /// Selects a random position from the provided list of grid coordinates.
        /// If the list is null or empty, a default position is returned instead.
        /// </summary>
        /// <param name="list">
        /// The list of available positions to choose from.
        /// </param>
        /// <returns>
        /// A randomly selected position as a tuple (<c>y</c>, <c>x</c>).
        /// Returns (0, 5) if the list is null or empty.
        /// </returns>
        public (int y, int x) RandomPositionFromList(List<(int y, int x)> list)
        {
            (int y, int x) defaultPosition = (0, 5);
            if (list == null|| list.Count==0)
            {
               
                _deps.AddCheck($"{nameof(RandomManager)}.{nameof(RandomPositionFromList)}:The list was null or empty — using default position {defaultPosition}.");
                return defaultPosition;
            }

            int randomIndex = Random.Next(0, list.Count);
            return list[randomIndex];
        }

        /// <summary>
        /// Returns a randomized subset of elements from the given list.
        /// The method performs a Fisher–Yates shuffle and returns the
        /// specified number of randomly selected elements.
        /// </summary>
        /// <typeparam name="T">
        /// The element type contained within the list.
        /// </typeparam>
        /// <param name="list">
        /// The list from which random elements are selected.
        /// </param>
        /// <param name="amount">
        /// The number of random elements to return.
        /// If <paramref name="amount"/> is greater than or equal to the list size,
        /// the method returns a full shuffled copy of the list.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> containing the randomized elements.
        /// Returns an empty list if the source list is null or empty.
        /// </returns>
        public List<T> GetRandomElements<T> (List<T> list,int amount)
        {
            if(list == null|| list.Count==0)
            {
                _deps.AddWarning($"{nameof(RandomManager)}.{nameof(GetRandomElements)}: Source list is empty.");
            }

            if (amount >= list!.Count)
            {
                return new List<T>(list);
            }

            var shuffled=new List<T> (list);

            for(int i=shuffled.Count-1;i>0;i--)
            {
               int j = _random.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            return shuffled.GetRange(0, amount);
        }
    }
}
