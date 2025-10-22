using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

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
        private readonly WallInstanceDependencies _wallInstanceDeps;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="WallInstance"/> class.
        /// Sets up all required dependencies for wall initialization and logging,
        /// and assigns a default tile type of <see cref="TileTyp.None"/>.
        /// </summary>
        /// <param name="wallInstanceDependencies">
        /// Reference to the <see cref="WallInstanceDependencies"/> object that provides
        /// the required managers and configuration data for wall initialization.
        /// </param>
        public WallInstance(WallInstanceDependencies wallInstanceDependencies)
        {
            _wallInstanceDeps = wallInstanceDependencies;
            _typ = TileTyp.None;
            _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}: Wall instance successfully created.");
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
                _symbol = _wallInstanceDeps.SymbolsManager.WallHorizontalSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (horizontal wall).");
            }
            else if (typ == TileTyp.WallVertical)
            {
                _symbol = _wallInstanceDeps.SymbolsManager.WallVerticalSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (vertical wall).");
            }
            else if (typ == TileTyp.WallCornerBottomLeft)
            {
                _symbol = _wallInstanceDeps.SymbolsManager.WallCornerBottomLeftSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-left corner).");
            }
            else if (typ == TileTyp.WallCornerBottomRight)
            {
                _symbol = _wallInstanceDeps.SymbolsManager.WallCornerBottomRightSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-right corner).");
            }
            else if (typ == TileTyp.WallCornerTopLeft)
            {
                _symbol = _wallInstanceDeps.SymbolsManager.WallCornerTopLeftSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-left corner).");
            }
            else if (typ == TileTyp.WallCornerTopRight)
            {
                _symbol = _wallInstanceDeps.SymbolsManager.WallCornerTopRightSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-right corner).");
            }
            else
            {
                _symbol = _wallInstanceDeps.SymbolsManager.DeathSymbol;
                _wallInstanceDeps.DiagnosticsManager.AddWarning($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Invalid wall type provided — using fallback symbol.");
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
            _wallInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(WallInstance)}.{nameof(AssignPosition)}: Position {_position} successfully assigned.");
        }
    }
}
