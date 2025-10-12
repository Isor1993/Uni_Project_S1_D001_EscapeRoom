using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class RandomManager
    {
        private readonly Random _rand;
        public RandomManager()
        {
            _rand = new Random();
        }
        

        /// <summary>
        /// Gibt die globale Random-Instanz zurück.
        /// </summary>
        public Random Random => _rand;

        /// <summary>
        /// Gibt eine Zufallszahl zwischen min (inklusive) und max (exklusive) zurück.
        /// </summary>
        public int Range(int min, int max)
        {
            return _rand.Next(min, max);
        }

        /// <summary>
        /// Gibt einen Zufallswert zwischen 0.0f und 1.0f zurück.
        /// </summary>
        public double Value()
        {
            return _rand.NextDouble();
        }
    }

}
