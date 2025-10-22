using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
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
    /// This class manages the door’s open/close state, its position, and
    /// the symbol that represents it visually on the board.
    /// </summary>
    internal class DoorInstance
    {
        // === Dependencies ===
        private readonly DoorInstanceDependencies _doorInstanceDeps;

        // === Fields ===
        private (int y, int x) _position;
        private char _symbol;
        private bool _isOpen;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoorInstance"/> class.
        /// Sets up all required dependencies, initializes the door in a closed state,
        /// assigns its tile type, and logs the creation event.
        /// </summary>
        /// <param name="doorInstanceDependencies">
        /// Reference to the <see cref="DoorInstanceDependencies"/> object that provides
        /// the necessary managers and configuration data for door initialization.
        /// </param>
        public DoorInstance(DoorInstanceDependencies doorInstanceDependencies)
        {
            _doorInstanceDeps = doorInstanceDependencies;
            _isOpen = false;
            _typ = TileTyp.Door;
            _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}: Door instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this door object (typically <see cref="TileTyp.Door"/>).
        /// </summary>
        public TileTyp Typ => _typ;

        /// <summary>
        /// Gets the current position of the door on the game board.
        /// Represented as a tuple of (y, x) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Gets the character symbol representing the door on the board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets a value indicating whether the door is currently open.
        /// </summary>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// Updates the current position of the object on the game board.
        /// </summary>
        /// <param name="position">The new position of the object, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignPosition)}: Position {position} - Door successfully assigned");
        }

        /// <summary>
        /// Closes the door by setting its state to closed.
        /// </summary>
        public void CloseDoor()
        {
            _isOpen = false;
            _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(CloseDoor)}:  Door closed successfully.");

        }
        /// <summary>
        /// Opens the door by setting its state to open.
        /// </summary>
        public void OpenDoor()
        {
            _isOpen = true;
            _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(OpenDoor)}:  Door opened successfully.");
        }

        /// <summary>
        /// TODO switch case upgrade?
        /// Assigns the appropriate door symbol based on its open/closed state and position on the board.
        /// If the door is placed on a valid wall (top/bottom or side), the correct symbol is assigned.
        /// Otherwise, a warning is logged, and a fallback symbol is applied.
        /// </summary>
        public void AssignDoorSymbol()
        {

            if (_isOpen)
            {
                if (_position.x == 0 || _position.x == _doorInstanceDeps.GameboardBuilder.ArraySizeX - 1)
                {
                    _symbol = _doorInstanceDeps.SymbolsManager.OpenDoorVerticalSymbol;
                    _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }
                else if (_position.y == 0 || _position.y == _doorInstanceDeps.GameboardBuilder.ArraySizeY - 1)
                {
                    _symbol = _doorInstanceDeps.SymbolsManager.OpenDoororizontalSymbol;
                    _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }

                else
                {
                    _doorInstanceDeps.DiagnosticsManager.AddWarning($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Position{_position} - Wall is not in the right position!");
                    _symbol = _doorInstanceDeps.SymbolsManager.DeathSymbol;
                }
            }

            else
            {
                if (_position.x == 0 || _position.x == _doorInstanceDeps.GameboardBuilder.ArraySizeX - 1)
                {
                    _symbol = _doorInstanceDeps.SymbolsManager.ClosedDoorVerticalSymbol;
                    _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }
                else if (_position.y == 0 || _position.y == _doorInstanceDeps.GameboardBuilder.ArraySizeY - 1)
                {
                    _symbol = _doorInstanceDeps.SymbolsManager.ClosedDoorHorizontalSymbol;
                    _doorInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }

                else
                {
                    _doorInstanceDeps.DiagnosticsManager.AddWarning($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Position {_position} - Wall is not in the right position!");
                    _symbol = _doorInstanceDeps.SymbolsManager.DeathSymbol;
                }
            }
        }

        /// <summary>
        /// Initializes the door object by setting its position and assigning the appropriate door symbol.
        /// </summary>
        /// <param name="position">The position of the door on the game board, represented as (y, x) coordinates.</param>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
            AssignDoorSymbol();
        }
    }
}
