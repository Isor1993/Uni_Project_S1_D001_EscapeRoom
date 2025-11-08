/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : WallInstance.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Represents a wall element on the game board.  
* Stores its type, position, and visual symbol, while maintaining diagnostic
* traceability through dependency injection.
*
* Responsibilities:
* - Store wall type, position, and visual representation
* - Assign correct symbol based on wall type
* - Log all initialization and assignment events
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall
{
    /// <summary>
    /// Represents a wall element on the game board.
    /// </summary>
    /// <remarks>
    /// A <see cref="WallInstance"/> stores the type, position, and symbol of a single wall tile.  
    /// It retrieves its visual symbol from the <see cref="WallInstanceDependencies.Symbol"/> configuration 
    /// and logs all initialization steps via <see cref="DiagnosticsManager"/> for debugging and validation.
    /// </remarks>
    internal class WallInstance
    {
        // === Dependencies ===
        private readonly WallInstanceDependencies _deps;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;
        private TileType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="WallInstance"/> class.
        /// </summary>
        /// <param name="wallInstanceDependencies">
        /// Reference to the <see cref="WallInstanceDependencies"/> object providing 
        /// access to the symbol configuration and diagnostic manager.
        /// </param>
        /// <remarks>
        /// The instance starts with <see cref="TileType.None"/> until it is initialized.
        /// </remarks>
        public WallInstance(WallInstanceDependencies wallInstanceDependencies)
        {
            _deps = wallInstanceDependencies;
            _type = TileType.None;
            _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}: Successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this wall instance 
        /// (typically one of the <see cref="TileType.Wall..."/> values).
        /// </summary>
        public TileType Type => _type;

        /// <summary>
        /// Gets the visual character symbol representing this wall instance.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the wall’s position on the game board.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Assigns a visual symbol to the wall based on the provided <see cref="TileType"/>.
        /// </summary>
        /// <param name="type">The wall type determining the correct symbol to use.</param>
        /// <remarks>
        /// Each wall type is mapped to its corresponding symbol from 
        /// <see cref="WallInstanceDependencies.Symbol"/>.  
        /// If the type is invalid, a fallback death symbol is applied and a warning is logged.
        /// </remarks>
        public void AssignSymbol(TileType type)
        {
            switch (type)
            {
                case TileType.WallHorizontal:
                    _symbol = _deps.Symbol.WallHorizontalSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (horizontal wall).");
                    break;

                case TileType.WallVertical:
                    _symbol = _deps.Symbol.WallVerticalSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (vertical wall).");
                    break;

                case TileType.WallCornerBottomLeft:
                    _symbol = _deps.Symbol.WallCornerBottomLeftSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-left corner).");
                    break;

                case TileType.WallCornerBottomRight:
                    _symbol = _deps.Symbol.WallCornerBottomRightSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (bottom-right corner).");
                    break;

                case TileType.WallCornerTopLeft:
                    _symbol = _deps.Symbol.WallCornerTopLeftSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-left corner).");
                    break;

                case TileType.WallCornerTopRight:
                    _symbol = _deps.Symbol.WallCornerTopRightSymbol;
                    _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} assigned (top-right corner).");
                    break;

                default:
                    _symbol = _deps.Symbol.DeathSymbol;
                    _deps.Diagnostic.AddWarning($"{nameof(WallInstance)}.{nameof(AssignSymbol)}: Invalid wall type provided — using fallback symbol.");
                    break;
            }
        }

        /// <summary>
        ///Initializes the wall instance by assigning its type and position.
        /// </summary>
        /// <param name="type">The type of wall (horizontal, vertical, or one of the corner types).</param>
        /// <param name="position">The position of the wall on the game board as (y, x) coordinates.</param>
        /// <remarks>
        /// Calls <see cref="AssignSymbol(TileType)"/> to apply the correct symbol,  
        /// assigns the grid position, and logs both actions in the diagnostics system.
        /// </remarks>
        public void Initialize(TileType typ, (int y, int x) position)
        {
            _type = typ;
            AssignSymbol(_type);
            AssignPosition(position);
        }

        /// <summary>
        /// Assigns the board position to this wall instance.
        /// </summary>
        /// <param name="position">The Y/X coordinates where this wall should be placed.</param>
        /// <remarks>
        /// This method only updates internal position data and logs the assignment.  
        /// It does not modify the actual game board array — this is handled by the <see cref="GameBoardManager"/>.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _deps.Diagnostic.AddCheck($"{nameof(WallInstance)}.{nameof(AssignPosition)}: Position {_position} successfully assigned.");
        }
    }
}
