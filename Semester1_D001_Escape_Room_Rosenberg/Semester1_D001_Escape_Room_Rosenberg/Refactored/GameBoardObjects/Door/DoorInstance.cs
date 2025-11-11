/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : DoorInstance.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Represents a door object within the game board.
* Manages its open/closed state, symbol assignment, and position through
* dependency injection, ensuring visual and logical consistency.
*
* Responsibilities:
* - Maintain door state and position
* - Assign correct visual symbols for open/closed states
* - Log all state and visual changes for debugging and validation
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door
{
    /// <summary>
    /// Represents a door object within the game board.
    /// </summary>
    /// <remarks>
    /// The <see cref="DoorInstance"/> manages its open or closed state, symbol assignment, and board position.
    /// It determines its visual representation dynamically based on board edges (horizontal or vertical orientation)
    /// and interacts with <see cref="DoorInstanceDependencies"/> for symbol configuration and diagnostic logging.
    /// </remarks>
    internal class DoorInstance
    {
        // === Dependencies ===
        private readonly DoorInstanceDependencies _deps;

        // === Fields ===
        private char _symbol;

        private (int y, int x) _position;
        private bool _isOpen;
        private TileType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoorInstance"/> class.
        /// </summary>
        /// <param name="doorInstanceDependencies">
        /// Provides access to the symbol configuration (<see cref="SymbolsManager"/>)
        /// and diagnostics system (<see cref="DiagnosticsManager"/>).
        /// </param>
        /// <remarks>
        /// The door starts closed by default and is assigned the <see cref="TileType.Door"/> type.
        /// </remarks>
        public DoorInstance(DoorInstanceDependencies doorInstanceDependencies)
        {
            _deps = doorInstanceDependencies;
            _isOpen = false;
            _type = TileType.Door;
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the tile type assigned to this door instance.
        /// </summary>
        public TileType Type => _type;

        /// <summary>
        /// Gets the current position of the door on the game board as (Y, X) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Gets the current symbol representing the door visually on the board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets a value indicating whether the door is currently open.
        /// </summary>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// Assigns the current grid position to the door and logs the update.
        /// </summary>
        /// <param name="position">The target (Y, X) coordinates where the door is placed.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignPosition)}: Position {position} assigned successfully.");
        }

        /// <summary>
        /// Closes the door and updates its visual representation accordingly.
        /// </summary>
        /// <remarks>
        /// After closing, the door symbol is reassigned based on its orientation
        /// (horizontal or vertical) and logged through diagnostics.
        /// </remarks>
        public void CloseDoor()
        {
            _isOpen = false;
            AssignDoorSymbol();
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(CloseDoor)}:  Door closed successfully.");
        }

        /// <summary>
        /// Opens the door and updates its visual representation accordingly.
        /// </summary>
        /// <remarks>
        /// The symbol is dynamically selected from the <see cref="SymbolsManager"/>
        /// depending on whether the door is placed on a vertical or horizontal wall edge.
        /// </remarks>
        public void OpenDoor()
        {
            _isOpen = true;
            AssignDoorSymbol();
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(OpenDoor)}:  Door opened successfully.");
        }

        /// <summary>
        /// Assigns the correct symbol to the door based on its state and position.
        /// </summary>
        /// <remarks>
        /// - Vertical edge → vertical door symbol
        /// - Horizontal edge → horizontal door symbol
        /// If the door is not placed on a valid edge, a fallback death symbol is assigned
        /// and a warning is logged for debugging.
        /// </remarks>
        public void AssignDoorSymbol()
        {
            bool isTop = _position.y == 0;
            bool isBottom = _position.y == Program.ArraySizeY - 1;
            bool isLeft = _position.x == 0;
            bool isRight = _position.x == Program.ArraySizeX - 1;

            if (_isOpen)
            {
                if (isLeft || isRight)
                    _symbol = _deps.Symbol.OpenDoorVerticalSymbol;
                else if (isTop || isBottom)
                    _symbol = _deps.Symbol.OpenDoorHorizontalSymbol;
                else
                    _symbol = _deps.Symbol.DeathSymbol; // Fallback
            }
            else
            {
                if (isLeft || isRight)
                    _symbol = _deps.Symbol.ClosedDoorVerticalSymbol;
                else if (isTop || isBottom)
                    _symbol = _deps.Symbol.ClosedDoorHorizontalSymbol;
                else
                    _symbol = _deps.Symbol.DeathSymbol; // Fallback
            }
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol {_symbol} assigned for door at {_position}.");
        }

        /// <summary>
        /// Initializes the door instance by assigning its board position and symbol.
        /// </summary>
        /// <param name="position">The door’s grid coordinates (Y, X).</param>
        /// <remarks>
        /// Ensures the door is properly aligned and visually initialized before rendering.
        /// Logs successful setup in the diagnostics system.
        /// </remarks>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
            AssignDoorSymbol();
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(Initialize)}: Initialized successfully.");
        }
    }
}