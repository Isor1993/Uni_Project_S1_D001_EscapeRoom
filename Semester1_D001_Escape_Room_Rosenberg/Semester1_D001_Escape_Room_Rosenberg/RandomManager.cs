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
    }

}
