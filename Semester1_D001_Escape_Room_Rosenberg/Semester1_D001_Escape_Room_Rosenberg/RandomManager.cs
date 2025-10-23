using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// 
    /// </summary>
    internal class RandomManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Random _random;
        private readonly DiagnosticsManager _diagnosticsManager;

        public RandomManager(DiagnosticsManager diagnosticsManager)
        {
            _random = new Random();
            this._diagnosticsManager = diagnosticsManager;
        }


        /// <summary>
        /// Gibt die globale Random-Instanz zurück.
        /// </summary>
        public Random Random => _random;

        /// <summary>
        /// Gibt eine Zufallszahl zwischen min (inklusive) und max (exklusive) zurück.
        /// </summary>
        public int Range(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// Gibt einen Zufallswert zwischen 0.0f und 1.0f zurück.
        /// </summary>
        public double Value()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public (int y, int x) RadomPositionFromList(List<(int y, int x)> list)
        {
            (int y, int x) defaultPosition = (0, 5);
            if (list == null|| list.Count==0)
            {
               
                _diagnosticsManager.AddCheck($"{nameof(RandomManager)}.{nameof(RadomPositionFromList)}:The list was null or empty — using default position {defaultPosition}.");
                return defaultPosition;
            }

            int randomIndex = Random.Next(0, list.Count);
            return list[randomIndex];
        }

        /// <summary>
        /// Returns a random subset of elements from the given list.
        /// The method creates a shuffled copy of the source list and selects
        /// the specified number of elements from the beginning of the shuffled list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements contained in the source list.
        /// </typeparam>
        /// <param name="list">
        /// The source list from which random elements are selected.
        /// </param>
        /// <param name="amount">
        /// The number of elements to return. If the requested amount is greater than or equal
        /// to the list’s size, a full copy of the list is returned.
        /// </param>
        /// <returns>
        /// A new <see cref="List{T}"/> containing a random subset of elements from the original list.
        /// Returns an empty list if the source list is null or empty.
        /// </returns>
        public List<T> GetRadomElements<T> (List<T> list,int amount)
        {
            if(list == null|| list.Count==0)
            {
                _diagnosticsManager.AddWarning($"{nameof(RandomManager)}.{nameof(GetRadomElements)}: Source list is empty.");
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
