/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : KeyFragmentInstance.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Represents a collectible key fragment placed on the game board.
* Handles its symbol, position, and quantity value while maintaining
* diagnostic traceability through injected dependencies.
*
* Responsibilities:
* - Store and manage key fragment data (symbol, position, quantity)
* - Log initialization and assignment events for debugging
* - Integrate with symbol and diagnostic systems through dependencies
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key
{
    /// <summary>
    /// Represents a collectible key fragment placed on the game board.
    /// </summary>
    /// <remarks>
    /// Each <see cref="KeyFragmentInstance"/> holds its visual symbol, position, and quantity value.
    /// It is initialized through dependency injection using <see cref="KeyFragmentInstanceDependencies"/>
    /// and logs all operations via the <see cref="DiagnosticsManager"/>.
    /// Key fragments serve as collectible objects that can unlock specific doors when collected.
    /// </remarks>
    internal class KeyFragmentInstance
    {
        // === Dependencies ===

        private readonly KeyFragmentInstanceDependencies _deps;
        // === Fields ===

        private char _symbol;
        private (int y, int x) _position;
        private int _amount;
        private TileType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyFragmentInstance"/> class.
        /// </summary>
        /// <param name="keyFragmentInstanceDependencies">
        /// Provides access to the symbol configuration (<see cref="SymbolsManager"/>) and
        /// diagnostic system (<see cref="DiagnosticsManager"/>).
        /// </param>
        /// <remarks>
        /// When created, the instance automatically assigns its default symbol from the
        /// <see cref="SymbolsManager"/>, sets its type to <see cref="TileType.Key"/>,
        /// and initializes its quantity to 1.
        /// </remarks>
        public KeyFragmentInstance(KeyFragmentInstanceDependencies keyFragmentInstanceDependencies)
        {
            _deps = keyFragmentInstanceDependencies;
            _symbol = _deps.Symbol.KeyFragmentSymbol;
            _amount = 1;
            _type = TileType.Key;
            _deps.Diagnostic.AddCheck($"{nameof(KeyFragmentInstance)}: Default symbol {_symbol} and amount {_amount} - Keyfragment assigned at start.");
        }

        /// <summary>
        /// Gets the quantity of key fragments represented by this instance.
        /// </summary>
        /// <remarks>
        /// Typically set to <c>1</c>, but can be expanded for advanced gameplay mechanics
        /// where multiple fragments are combined into a single collectible entity.
        /// </remarks>
        public int Amount => _amount;

        /// <summary>
        /// Gets the tile type assigned to this key fragment (typically <see cref="TileType.Key"/>).
        /// </summary>
        public TileType Type => _type;

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
        /// Assigns a new symbol to this key fragment.
        /// </summary>
        /// <param name="symbol">The new character symbol to visually represent the key fragment.</param>
        /// <remarks>
        /// Updates the visual representation and logs the change for traceability and validation.
        /// </remarks>
        public void AssignSymbol(char symbol)
        {
            _symbol = symbol;
            _deps.Diagnostic.AddCheck($"{nameof(KeyFragmentInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} - KeyFragment symbol successfully assigned");
        }

        /// <summary>
        /// Assigns a new position to this key fragment on the game board.
        /// </summary>
        /// <param name="position">The new grid position of the key fragment as (Y, X) coordinates.</param>
        /// <remarks>
        /// The new position is logged for verification and used during rendering and interaction checks.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _deps.Diagnostic.AddCheck($"{nameof(KeyFragmentInstance)}.{nameof(AssignPosition)}: Position {_position} - KeyFragment successfully assigned");
        }

        /// <summary>
        /// Initializes the key fragment by assigning its initial position.
        /// </summary>
        /// <param name="position">The position of the key fragment on the game board as (Y, X) coordinates.</param>
        /// <remarks>
        /// Typically called by the <see cref="SpawnManager"/> during runtime initialization
        /// when placing collectibles on the board.
        /// Logs successful initialization through diagnostics.
        /// </remarks>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
            _deps.Diagnostic.AddCheck($"{nameof(KeyFragmentInstance)}.{nameof(Initialize)}: Initialized successfully.");
        }
    }
}