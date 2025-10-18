namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key
{
    /// <summary>
    /// Represents a collectible key fragment on the game board.
    /// Stores the key’s symbol and position, and provides methods to assign or initialize these values.
    /// </summary>
    internal class KeyFragment
    {
        // === Fields ===
        private readonly SymbolsManager _symbolsManager;       
        private char _symbol;
        private (int y, int x) _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyFragment"/> class.
        /// </summary>
        /// <param name="symbolsManager">A reference to the <see cref="SymbolsManager"/> used to assign the default key fragment symbol.</param>
        public KeyFragment(SymbolsManager symbolsManager)
        {
            _symbolsManager = symbolsManager;
            _symbol = _symbolsManager.KeyFragmentSymbol;
            _position = (0, 0);
        }

        /// <summary>
        /// Gets the character symbol that visually represents this key fragment.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the current position of the key fragment on the game board.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Assigns a new key symbol to this fragment.
        /// </summary>
        public void AssignSymbol(char symbol)
        {
            _symbol = symbol;
        }

        /// <summary>
        /// Assigns a new position to this key fragment on the game board.
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
