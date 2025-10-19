namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall
{    
    /// <summary>
    /// Represents a wall element on the game board.
    /// Provides access to wall symbols and stores both the symbol and position data
    /// for individual wall instances.
    /// </summary>
    internal class WallInstance
    {

        // === Dependencies ===
        // Provides predefined wall symbols.
        private readonly SymbolsManager _symbolsManager;
        // Used for board-related operations.
        private readonly DiagnosticsManager _diagnosticsManager;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="WallInstance"/> class with the required dependencies.
        /// Sets the default tile type and logs the initialization event.
        /// </summary>
        /// <param name="symbolsManager">Reference to the <see cref="SymbolsManager"/> providing wall symbols.</param>
        /// <param name="diagnosticsManager">Reference to the <see cref="DiagnosticsManager"/> used for logging checks and warnings.</param>
        public WallInstance(SymbolsManager symbolsManager, DiagnosticsManager diagnosticsManager)
        {
            this._symbolsManager = symbolsManager;
            this._diagnosticsManager = diagnosticsManager;
            _typ = TileTyp.None;
            _diagnosticsManager.AddCheck($"{nameof(WallInstance)}: Wall instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this Wall (typically a <see cref="TileTyp.Wall..."/> value).
        /// </summary>
        public TileTyp Typ=>_typ;
        /// <summary>
        /// Gets the character symbol that visually represents this wall element.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the wall's position on the game board.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// TODO Switch case upgrade?
        /// Assigns the correct wall symbol based on the given <see cref="TileTyp"/>.
        /// Logs the assignment result through the diagnostics manager.
        /// </summary>
        /// <param name="typ">The wall type that determines which symbol will be assigned.</param>
        public void AssignSymbol(TileTyp typ)
        {
            if (typ == TileTyp.WallHorizontal)
            {
                _symbol = _symbolsManager.WallHorizontalSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (horizontal wall).");
            }
            else if (typ == TileTyp.WallVertical)
            {
                _symbol = _symbolsManager.WallVerticalSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (vertical wall).");
            }
            else if (typ == TileTyp.WallCornerBottomLeft)
            {
                _symbol = _symbolsManager.WallCornerBottomLeftSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-left corner).");
            }
            else if (typ == TileTyp.WallCornerBottomRight)
            {
                _symbol = _symbolsManager.WallCornerBottomRightSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-right corner).");
            }
            else if (typ == TileTyp.WallCornerTopLeft)
            {
                _symbol = _symbolsManager.WallCornerTopLeftSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-left corner).");
            }
            else if (typ == TileTyp.WallCornerTopRight)
            {
                _symbol = _symbolsManager.WallCornerTopRightSymbol;
                _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-right corner).");
            }
            else
            {
                _symbol = _symbolsManager.DeathSymbol;
                _diagnosticsManager.AddWarning($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Invalid wall type provided — using fallback symbol.");
            }
        }

        /// <summary>
        /// Initializes the wall instance by assigning both its type and position.
        /// Automatically applies the appropriate symbol based on the type.
        /// </summary>
        /// <param name="typ">The type of wall (e.g., horizontal, vertical, or corner).</param>
        /// <param name="position">The position of the wall on the game board, represented as (y, x) coordinates.</param>
        public void Initialize(TileTyp typ, (int y, int x) position)
        {
            _typ = typ;
            AssignSymbol(_typ);
            AssignPosition(position);
        }

        /// <summary>
        /// Assigns a position to this wall element on the game board
        /// and logs the position assignment.
        /// </summary>
        /// <param name="position">The new position of the wall, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _diagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignPosition)}: Position {position} successfully assigned.");
        }
    }
}
