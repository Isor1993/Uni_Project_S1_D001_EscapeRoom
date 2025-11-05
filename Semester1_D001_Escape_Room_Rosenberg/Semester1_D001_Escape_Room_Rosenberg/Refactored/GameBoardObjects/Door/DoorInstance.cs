using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door
{
    /// <summary>
    /// Represents a door object within the game board.
    /// </summary>
    /// <remarks>
    /// The <see cref="DoorInstance"/> manages its open or closed state, symbol assignment, and board position.  
    /// It determines its visual representation dynamically based on its position on the board (horizontal or vertical walls) 
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
        public DoorInstance(DoorInstanceDependencies doorInstanceDependencies)
        {
            _deps = doorInstanceDependencies;
            _isOpen = false;
            _type = TileType.Door;
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}: Instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this door instance.
        /// </summary>
        public TileType Typ => _type;

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
        /// Assigns the current grid position to the door.
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
        /// <param name="arraySizeY">The total height of the game board grid.</param>
        /// <param name="arraySizeX">The total width of the game board grid.</param>
        /// <remarks>
        /// After closing, the door symbol is reassigned based on its orientation (horizontal or vertical).  
        /// Logs both the visual update and state transition in diagnostics.
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
        /// <param name="arraySizeY">The total height of the game board grid.</param>
        /// <param name="arraySizeX">The total width of the game board grid.</param>
        /// <remarks>
        /// The door symbol is automatically chosen from <see cref="SymbolsManager"/> 
        /// based on whether the door is located on a vertical or horizontal wall edge.
        /// </remarks>
        public void OpenDoor()
        {
            _isOpen = true;
            AssignDoorSymbol();
            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(OpenDoor)}:  Door opened successfully.");
        }

        /// <summary>
        /// Assigns the correct symbol to the door based on its state and position on the board.
        /// </summary>
        /// <param name="arraySizeY">The height of the game board grid.</param>
        /// <param name="arraySizeX">The width of the game board grid.</param>
        /// <remarks>
        /// - Vertical edge → vertical door symbol  
        /// - Horizontal edge → horizontal door symbol  
        /// If the door is positioned incorrectly (not on a valid wall edge),  
        /// a warning is logged and a fallback symbol is used.
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

            _deps.Diagnostic.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol {_symbol} assigned for door at {_position}");
        }



        /// <summary>
        /// Initializes the door instance with its board position and automatically assigns the correct symbol.
        /// </summary>
        /// <param name="position">The door’s coordinates on the grid (Y, X).</param>
        /// <param name="arraySizeX">The total board width used for orientation check.</param>
        /// <param name="arraySizeY">The total board height used for orientation check.</param>
        /// <remarks>
        /// This method ensures the door is visually aligned and properly initialized before rendering on the board.  
        /// It also registers the initial diagnostics entry for traceability.
        /// </remarks>
        public void Initialize((int y, int x)position)
        {
            AssignPosition(position);
            AssignDoorSymbol();
        }
    }
}
