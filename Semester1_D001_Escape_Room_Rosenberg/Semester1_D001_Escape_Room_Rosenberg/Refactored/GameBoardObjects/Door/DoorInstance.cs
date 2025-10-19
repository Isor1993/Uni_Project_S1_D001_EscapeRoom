using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
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
        private readonly SymbolsManager _symbolsManager;
        private readonly GameBoardBuilder _gameboardBuilder;
        private readonly DiagnosticsManager _diagnosticsManager;

        // === Fields ===
        private (int y, int x) _position;
        private char _symbol;
        private bool _isOpen;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoorInstance"/> class.
        /// Sets up all required dependencies and initializes the door in a closed state.
        /// Also registers the creation event in the diagnostics log.
        /// </summary>
        /// <param name="symbolsManager">Reference to the <see cref="SymbolsManager"/> that provides all door symbols.</param>
        /// <param name="gameBoardBuilder">Reference to the <see cref="GameBoardBuilder"/> that defines the board dimensions.</param>
        /// <param name="diagnosticsManager">Reference to the <see cref="DiagnosticsManager"/> used for logging checks and warnings.</param>
        public DoorInstance(SymbolsManager symbolsManager, GameBoardBuilder gameBoardBuilder, DiagnosticsManager diagnosticsManager)
        {
            this._symbolsManager = symbolsManager;
            this._gameboardBuilder = gameBoardBuilder;
            this._diagnosticsManager = diagnosticsManager;
            _isOpen = false;
            _typ = TileTyp.Door;
            _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}: Door instance successfully created.");
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
        public void MovePosition((int y, int x) position)
        {
            _position = position;
            _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(MovePosition)}: Position {position} - Door successfully assigned");
        }

        /// <summary>
        /// Closes the door by setting its state to closed.
        /// </summary>
        public void CloseDoor()
        {
            _isOpen = false;
            _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(CloseDoor)}:  Door closed successfully.");

        }
        /// <summary>
        /// Opens the door by setting its state to open.
        /// </summary>
        public void OpenDoor()
        {
            _isOpen = true;
            _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(OpenDoor)}:  Door opened successfully.");
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
                if (_position.x == 0 || _position.x == _gameboardBuilder.ArraySizeX - 1)
                {
                    _symbol = _symbolsManager.OpenDoorVerticalSymbol;
                    _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }
                else if (_position.y == 0 || _position.y == _gameboardBuilder.ArraySizeY - 1)
                {
                    _symbol = _symbolsManager.OpenDoororizontalSymbol;
                    _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }

                else
                {
                    _diagnosticsManager.AddWarning($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Position{_position} - Wall is not in the right position!");
                    _symbol = _symbolsManager.DeathSymbol;
                }
            }

            else
            {
                if (_position.x == 0 || _position.x == _gameboardBuilder.ArraySizeX - 1)
                {
                    _symbol = _symbolsManager.ClosedDoorVerticalSymbol;
                    _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }
                else if (_position.y == 0 || _position.y == _gameboardBuilder.ArraySizeY - 1)
                {
                    _symbol = _symbolsManager.ClosedDoorHorizontalSymbol;
                    _diagnosticsManager.AddCheck($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Symbol{_symbol} - Door symbol successfully assigned");
                }

                else
                {
                    _diagnosticsManager.AddWarning($"{nameof(DoorInstance)}.{nameof(AssignDoorSymbol)}: Position {_position} - Wall is not in the right position!");
                    _symbol = _symbolsManager.DeathSymbol;
                }
            }
        }

        /// <summary>
        /// Initializes the door object by setting its position and assigning the appropriate door symbol.
        /// </summary>
        /// <param name="position">The position of the door on the game board, represented as (y, x) coordinates.</param>
        public void Initialize((int y, int x) position)
        {
            MovePosition(position);
            AssignDoorSymbol();
        }
    }
}
