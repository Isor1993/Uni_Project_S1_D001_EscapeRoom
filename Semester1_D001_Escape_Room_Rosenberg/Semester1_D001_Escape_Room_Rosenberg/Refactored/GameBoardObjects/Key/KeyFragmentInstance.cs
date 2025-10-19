using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key
{
    /// <summary>
    /// Represents a key fragment instance placed on the game board.
    /// Stores its symbol and position, provides methods for assigning and initializing data,
    /// and logs actions through the diagnostics manager.
    /// </summary>
    internal class KeyFragmentInstance
    {
        // === Fields ===
        private readonly SymbolsManager _symbolsManager;
        private readonly DiagnosticsManager _diagnosticsManager;
        private char _symbol;
        private (int y, int x) _position;
        private TileTyp _typ;

        /// <summary>
        ///Initializes a new instance of the <see cref="KeyFragmentInstance"/> class.
        /// Sets up all required dependencies and assigns the default key fragment symbol and type.
        /// Also logs the creation event through the diagnostics manager.
        /// </summary>
        /// <param name="symbolsManager">Reference to the <see cref="SymbolsManager"/> used to assign the key fragment symbol.</param>
        /// <param name="diagnosticsManager">Reference to the <see cref="DiagnosticsManager"/> used for logging operations.</param>
        public KeyFragmentInstance(SymbolsManager symbolsManager, DiagnosticsManager diagnosticsManager)
        {
            this._symbolsManager = symbolsManager;
            this._diagnosticsManager = diagnosticsManager;
            _symbol = _symbolsManager.KeyFragmentSymbol;
            _position = (0, 0);
            _typ = TileTyp.Key;
            _diagnosticsManager.AddCheck($"{nameof(KeyFragmentInstance)}: Default symbol {_symbol} - Keyfragment assigned at start.");

        }

        /// <summary>
        /// Gets the tile type assigned to this key fragment (typically <see cref="TileTyp.Key"/>).
        /// </summary>
        public TileTyp Typ => _typ;

        /// <summary>
        /// Gets the character symbol representing this object on the game board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the current position of this object on the game board,
        /// represented as (y, x) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Assigns a new symbol to this key fragment and logs the operation.
        /// </summary>
        /// <param name="symbol">The character symbol representing the key fragment.</param>
        public void AssignSymbol(char symbol)
        {
            _symbol = symbol;
            _diagnosticsManager.AddCheck($"{nameof(KeyFragmentInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} - KeyFragment symbol successfully assigned");
        }

        /// <summary>
        /// Assigns a new position to this key fragment on the game board and logs the operation.
        /// </summary>
        /// <param name="position">The new position of the key fragment, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _diagnosticsManager.AddCheck($"{nameof(KeyFragmentInstance)}.{nameof(AssignPosition)}: Position {position} - KeyFragment successfully assigned");
        }

        /// <summary>
        /// Initializes the key fragment by assigning its position on the game board.
        /// </summary>
        /// <param name="position">The position of the key fragment, represented as (y, x) coordinates.</param>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
        }
    }
}
