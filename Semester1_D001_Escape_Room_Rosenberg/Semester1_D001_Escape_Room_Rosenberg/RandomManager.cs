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

        public RandomManager()
        {
            _random = new Random();
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
    }

}
