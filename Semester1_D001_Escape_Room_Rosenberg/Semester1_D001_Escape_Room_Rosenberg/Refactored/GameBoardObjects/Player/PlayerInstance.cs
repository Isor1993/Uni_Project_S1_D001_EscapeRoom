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
    /// </summary>
    /// <remarks>
    /// The <see cref="PlayerInstance"/> holds data such as position, symbol, lives, and alive status.  
    /// It also provides methods to modify the player’s state during gameplay (e.g., taking damage or dying).  
    /// Initialization and symbol assignment are handled via dependency injection through
    /// <see cref="PlayerInstanceDependencies"/>.
    /// </remarks>
    internal class PlayerInstance
    {
        // === Dependencies ===
        private readonly PlayerInstanceDependencies _deps;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;
        private int _life;
        private bool _isAlive;
        private TileTyp _type;

        /// <summary>
        /// nitializes a new instance of the <see cref="PlayerInstance"/> class.
        /// </summary>
        /// <param name="playerInstanceDependencies">
        /// Provides references to the required systems for player initialization, 
        /// such as <see cref="DiagnosticsManager"/> and <see cref="SymbolsManager"/>.
        /// </param>
        /// <remarks>
        /// When created, the player receives a default symbol, three lives, and a "alive" status.  
        /// A log entry is added to diagnostics confirming successful initialization.
        /// </remarks>
        public PlayerInstance(PlayerInstanceDependencies playerInstanceDependencies)
        {
            _deps = playerInstanceDependencies;
            _type = TileTyp.Player;
            _symbol = _deps.Symbol.PlayerSymbol;
            _life = 3;
            _isAlive = true;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}: Default symbbol {_symbol}, typ {_type}, isAlive {_isAlive} and lifes {_life}  - Player instance successfully created with default symbol");
        }

        /// <summary>
        /// Gets the character symbol representing the player on the game board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the number of remaining lives the player currently has.
        /// </summary>
        /// <remarks>
        /// Initially set to <c>3</c>. Each time the player takes damage, this value decreases.
        /// </remarks>
        public int Heart => _life;

        /// <summary>
        /// Indicates whether the player is still alive.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if the player has at least one remaining life;  
        /// set to <c>false</c> when all lives are lost.
        /// </remarks>
        public bool IsAlive => _isAlive;
        /// <summary>
        /// Gets the current position of the player on the game board,
        /// represented as (y, x) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Gets the tile type assigned to this object (always <see cref="TileTyp.Player"/>).
        /// </summary>
        public TileTyp Typ => _type;

        /// <summary>
        /// Assigns a new visual symbol to the player and logs the update.
        /// </summary>
        /// <param name="symbol">The new character symbol representing the player.</param>
        public void AssignSymbol(char symbol)
        {
            _symbol = symbol;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(AssignSymbol)}: Symbol {_symbol} - Player symbol successfully assigned");
        }

        /// <summary>
        /// Assigns a new position to the player on the game board and logs the update.
        /// </summary>
        /// <param name="position">The position of the player, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(AssignPosition)}: Position {_position} - Player successfully assigned");
        }

        /// <summary>
        /// Initializes the player by assigning a starting position on the game board.
        /// </summary>
        /// <param name="position">The player’s initial grid coordinates as (Y, X).</param>
        /// <remarks>
        /// Typically called when the player is spawned or when the level starts.
        /// </remarks>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
        }

        /// <summary>
        /// Sets the player's status to "dead".
        /// </summary>
        /// <remarks>
        /// This method is typically triggered when the player's life count reaches zero.  
        /// The state change is logged for debugging and gameplay validation.
        /// </remarks>
        public void SetDead()
        {
            _isAlive = false;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(SetDead)}: Player has died.");
        }

        /// <summary>
        /// Decreases the player's life count by one.
        /// </summary>
        /// <remarks>
        /// When the player loses all lives, <see cref="SetDead()"/> should be called afterward  
        /// to update the alive status. Each decrement is logged for validation.
        /// </remarks>
        public void loseLife()
        {
            _life -= 1;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(loseLife)}: Player lost one life. Remaining lives: {_life}.");
        }
    }
}