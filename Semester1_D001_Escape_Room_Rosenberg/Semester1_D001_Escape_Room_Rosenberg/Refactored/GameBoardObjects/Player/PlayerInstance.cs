using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player
{
    /// <summary>
    /// Represents the player instance on the game board.
    /// Stores the player’s position, symbol, and tile type,
    /// and provides functionality to assign and initialize these values.
    /// </summary>
    internal class PlayerInstance
    {
        // === Dependencies ===
        private readonly PlayerInstanceDependencies _playerInstanceDeps;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInstance"/> class.
        /// Sets up all required dependencies for player initialization,
        /// assigns the default player symbol, and logs the creation event.
        /// </summary>
        /// <param name="playerInstanceDependencies">
        /// Reference to the <see cref="PlayerInstanceDependencies"/> object that provides
        /// the necessary managers and configuration data for player setup.
        /// </param>
        public PlayerInstance(PlayerInstanceDependencies playerInstanceDependencies)
        {
            _playerInstanceDeps = playerInstanceDependencies;
            _typ = TileTyp.Player;
            _symbol= _playerInstanceDeps.SymbolsManager.PlayerSymbol;
            _playerInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(PlayerInstance)}: Default symbol {_symbol} - Player instance successfully created with default symbol");           
        }

        /// <summary>
        /// Gets the character symbol representing the player on the game board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the current position of the player on the game board,
        /// represented as (y, x) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Gets the tile type assigned to this object (always <see cref="TileTyp.Player"/>).
        /// </summary>
        public TileTyp Typ => _typ;

        /// <summary>
        /// Assigns a new visual symbol to the player and logs the update.
        /// </summary>
        /// <param name="symbol">The new character symbol representing the player.</param>
        public void AssignSymbol(char symbol)
        {
            _symbol = symbol;
            _playerInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(PlayerInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} - Player symbol successfully assigned");
        }

        /// <summary>
        /// Assigns a new position to the player on the game board and logs the update.
        /// </summary>
        /// <param name="position">The position of the player, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _playerInstanceDeps.DiagnosticsManager.AddCheck($"{nameof(PlayerInstance)}.{nameof(AssignPosition)}: Position {_position} - Player successfully assigned");
        }

        /// <summary>
        /// Initializes the player instance by assigning a specific position on the game board.
        /// </summary>
        /// <param name="position">The player’s starting position, represented as (y, x) coordinates.</param>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
        }
    }
}