/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PlayerInstance.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Represents the player instance on the game board.
* Stores position, symbol, lives, and alive state while maintaining diagnostic
* traceability through dependency injection.
*
* Responsibilities:
* - Manage player data (position, symbol, lives, state)
* - Provide methods for state changes and initialization
* - Log all updates for debugging and runtime validation
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player
{
    /// <summary>
    /// Represents the player instance on the game board.
    /// </summary>
    /// <remarks>
    /// The <see cref="PlayerInstance"/> manages player-specific attributes such as name,
    /// position, lives, and active state.
    /// Initialization and symbol assignment are performed via
    /// <see cref="PlayerInstanceDependencies"/> and all actions are logged through the
    /// <see cref="DiagnosticsManager"/>.
    /// </remarks>
    internal class PlayerInstance
    {
        // === Dependencies ===
        private readonly PlayerInstanceDependencies _deps;

        // === Fields ===
        private char _symbol;

        private (int y, int x) _position;
        private int _lives;
        private bool _isAlive;
        private TileType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInstance"/> class.
        /// </summary>
        /// <param name="playerInstanceDependencies">
        /// Provides access to required systems such as
        /// <see cref="DiagnosticsManager"/> and <see cref="SymbolsManager"/>.
        /// </param>
        /// <param name="name">The display name of the player.</param>
        /// <remarks>
        /// When created, the player receives the default symbol, three lives, and
        /// is marked as alive.
        /// A log entry confirms successful creation.
        /// </remarks>
        public PlayerInstance(PlayerInstanceDependencies playerInstanceDependencies, string name)
        {
            _deps = playerInstanceDependencies;
            _type = TileType.Player;
            _symbol = _deps.Symbol.PlayerSymbol;
            _lives = 3;
            Name = name;
            _isAlive = true;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}: Default symbol {_symbol}, typ {_type}, isAlive {_isAlive} and lives {_lives}  - Player instance successfully created with default symbol");
        }

        /// <summary>
        /// Gets the player's display name used for HUD and diagnostics.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the character symbol representing the player on the game board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the number of remaining lives the player currently has.
        /// </summary>
        /// <remarks>
        /// Initially set to <c>3</c>.
        /// Each time the player takes damage, this value decreases.
        /// </remarks>
        public int Lives => _lives;

        /// <summary>
        /// Indicates whether the player is currently alive.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if the player has at least one remaining life;
        /// set to <c>false</c> when all lives are lost.
        /// </remarks>
        public bool IsAlive => _isAlive;

        /// <summary>
        /// Gets the player's current position on the game board as (Y, X) coordinates.
        /// </summary>
        public (int y, int x) Position => _position;

        /// <summary>
        /// Gets the tile type assigned to this instance (always <see cref="TileType.Player"/>).
        /// </summary>
        public TileType Type => _type;

        /// <summary>
        /// Assigns a new visual symbol to the player and logs the change.
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
        /// <param name="position">The player's new (Y, X) coordinates.</param>
        public void AssignPosition((int y, int x) position)
        {
            _position = position;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(AssignPosition)}: Position {_position} - Player successfully assigned");
        }

        /// <summary>
        /// Initializes the player by assigning a starting position on the game board.
        /// </summary>
        /// <param name="position">The player's initial grid coordinates (Y, X).</param>
        /// <remarks>
        /// Typically called when spawning the player or starting a level.
        /// Logs successful initialization through diagnostics.
        /// </remarks>
        public void Initialize((int y, int x) position)
        {
            AssignPosition(position);
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(Initialize)}:Initialize successfully.");
        }

        /// <summary>
        /// Sets the player's status to dead.
        /// </summary>
        /// <remarks>
        /// Called when the player's life count reaches zero.
        /// The change is logged for debugging and validation.
        /// </remarks>
        public void SetDead()
        {
            _isAlive = false;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(SetDead)}: Player has died.");
        }

        /// <summary>
        /// Decreases the player's life count by one and logs the result.
        /// </summary>
        /// <remarks>
        /// When all lives are lost, <see cref="SetDead"/> should be called afterward
        /// to update the alive status.
        /// </remarks>
        public void LoseLife()
        {
            _lives -= 1;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(LoseLife)}: Player lost one life. Remaining lives: {_lives}.");
        }

        /// <summary>
        /// Updates the player's display name and logs the change.
        /// </summary>
        /// <param name="newName">The new name to assign.</param>
        public void SetName(string newName)
        {
            Name = newName;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerInstance)}.{nameof(SetName)}: Player name changed to '{newName}'.");
        }
    }
}