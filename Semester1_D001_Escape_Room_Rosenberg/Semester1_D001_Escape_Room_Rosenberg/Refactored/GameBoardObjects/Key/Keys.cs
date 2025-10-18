using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key
{
    /// <summary>
    /// Represents a key fragment element on the game board.
    /// Stores the key symbol and its position, and provides functionality to assign or initialize it.
    /// </summary>
    internal class Keys
    {
        // Reference to the SymbolsManager, used to access predefined key symbols.
        readonly SymbolsManager _symbolsManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="Keys"/> class.
        /// </summary>
        /// <param name="symbolsManager">Reference to the SymbolsManager that provides the key symbol.</param>
        public Keys(SymbolsManager symbolsManager)
        {
            _symbolsManager = symbolsManager;
            _symbol = _symbolsManager.KeyFragmentSymbol;
            _position = (0, 0);
        }
        // Private fields for storing key properties.
        private char _symbol;
        private (int y, int x) _position;
        //Public read-only properties for external access.
        public char Symbol => _symbol;
        public (int y, int x) Position => _position;
        /// <summary>
        /// Assigns the key fragment symbol from the SymbolsManager to this key instance.
        /// </summary>
        public void AssingnSymbolKey(char symbol)
        {
            _symbol = symbol;
        }
        /// <summary>
        /// Assigns a position to this key on the game board.
        /// </summary>
        /// <param name="y">The Y-coordinate of the key's position</param>
        /// <param name="x">The X-coordinate of the key's position.</param>
        public void AssignPosition(int y , int x)
        {
             _position = (y, x);
        }
        /// <summary>
        /// Initializes the key with a specific symbol and position on the game board.
        /// </summary>
        /// <param name="symbol">The character symbol representing the key.</param>
        /// <param name="y">The Y-coordinate of the key's position.</param>
        /// <param name="x">The X-coordinate of the key's position.</param>
        public void Initialize(char symbol, int y, int x)
        {
            _symbol = symbol;
            _position = (y, x);
        }
    }
}
