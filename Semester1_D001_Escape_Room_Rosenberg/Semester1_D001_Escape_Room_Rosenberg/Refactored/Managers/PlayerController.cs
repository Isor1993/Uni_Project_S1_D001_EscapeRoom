/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PlayerController.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all player-related input, movement, and interaction logic within the game board.
* Interprets keyboard input, validates movement rules, and triggers interactions
* with objects such as keys, NPCs, and doors.
* Also manages player rendering updates and logs every player action.
*
* Responsibilities:
* - Interpret console input and map it to player actions
* - Validate and perform player movement on the game board
* - Handle manual interactions (e.g. talk, collect, open)
* - Update console rendering through PrintManager
* - Log all movement, errors, and interaction events via DiagnosticsManager
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Central controller for all player-related actions in the game world.
    /// Processes user input, handles movement validation, and coordinates interactions
    /// with other systems (GameBoard, Interaction, Print, and Diagnostics).
    /// </summary>
    internal class PlayerController
    {
        // === Dependencies ===
        private readonly PlayerControllerDependencies _deps;

        // === Player Instance ===
        private PlayerInstance? _player;

        // === Movement Tracking ===
        private PlayerActions _lastMoveDirection;

        /// <summary>
        /// Defines the possible player actions interpreted from keyboard input.
        /// </summary>
        private enum PlayerActions
        {
            None,
            Up,
            Down,
            Left,
            Right,
            Interact,
            Quit,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="playerControllerDependencies">
        /// Dependency container providing references to managers for diagnostics,
        /// interaction, printing, symbol access, and rule validation.
        /// </param>
        public PlayerController(PlayerControllerDependencies playerControllerDependencies)
        {
            _deps = playerControllerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Initialized successfully.");
            _player = _deps.GameObject.Player;
            if (_player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: PlayerInstance is null");
                return;
            }
        }

        /// <summary>
        /// Gets the current player position on the board.
        /// </summary>
        public (int y, int x) PlayerPosition => _player!.Position;       

        /// <summary>
        /// Handles player movement, interaction, and command logic based on key input.
        /// </summary>
        /// <param name="key">
        /// The <see cref="ConsoleKey"/> input pressed by the player.
        /// Supported keys: WASD / Arrow keys (movement), E/Enter (interact), Escape (quit).
        /// </param>
        public void MovePlayer(ConsoleKey key)
        {
            if (_player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}.{nameof(MovePlayer)}: Player reference missing!");
                return;
            }
            else if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}.{nameof(MovePlayer)}: GameBoardArray reference missing!");
                return;
            }

            PlayerActions moveDirection = GetMoveDirection(key);
            (int y, int x) newPosition = _player.Position;

            switch (moveDirection)
            {
                case PlayerActions.Up:
                    newPosition.y--;
                    _lastMoveDirection = PlayerActions.Up;
                    break;

                case PlayerActions.Down:
                    newPosition.y++;
                    _lastMoveDirection = PlayerActions.Down;
                    break;

                case PlayerActions.Left:
                    newPosition.x--;
                    _lastMoveDirection = PlayerActions.Left;
                    break;

                case PlayerActions.Right:
                    newPosition.x++;
                    _lastMoveDirection = PlayerActions.Right;
                    break;

                case PlayerActions.Interact:
                    HandleManualInteraction();
                    return;

                case PlayerActions.Quit:
                    _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(MovePlayer)}: Quit command issued.");
                    Environment.Exit(0);
                    return;

                default:
                    _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}.{nameof(MovePlayer)}: No valid input detected.");
                    return;
            }

            bool canMove = _deps.Rule.IsMoveAllowed(newPosition);
            if (!canMove)
            {
                _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}.{nameof(MovePlayer)}: Move blocked at {newPosition}.");
                return;
            }

            KeyFragmentInstance? targetObject = _deps.GameObject.GetObject<KeyFragmentInstance>(newPosition);

            if (targetObject != null)
            {
                _deps.Interaction.InteractionHandler(newPosition);
                _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Key found at {newPosition}.");
            }

            _deps.GameObject.MovePlayer(PlayerPosition, newPosition);

            _deps.Print.PrintTile(PlayerPosition, _deps.Symbol.EmptySymbol);

            _player.AssignPosition(newPosition);

            _deps.Print.PrintTile(newPosition, _player.Symbol);

            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(MovePlayer)}: Player moved to ({newPosition.y},{newPosition.x}).");
        }

        /// <summary>
        /// Resets the player state and connects the start position from <see cref="PlayerInstance"/>.
        /// </summary>
        private void SetStartPosition()
        {
            _lastMoveDirection = PlayerActions.None;

            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(SetStartPosition)}: Connected player position from PlayerInstance.");
        }

        /// <summary>
        /// Maps a pressed keyboard key to a corresponding <see cref="PlayerActions"/> value.
        /// </summary>
        /// <param name="key">The input key from the console.</param>
        /// <returns>
        /// A matching <see cref="PlayerActions"/> enum value.
        /// Returns <see cref="PlayerActions.None"/> if no valid mapping exists.
        /// </returns>
        private PlayerActions GetMoveDirection(ConsoleKey key)
        {
            // Map input keys to movement directions.
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return PlayerActions.Up;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return PlayerActions.Down;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return PlayerActions.Left;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return PlayerActions.Right;

                case ConsoleKey.E:
                case ConsoleKey.Enter:
                    return PlayerActions.Interact;

                case ConsoleKey.Escape:
                    return PlayerActions.Quit;

                default:
                    return PlayerActions.None;
            }
        }

        /// <summary>
        /// Triggers an interaction in the direction of the player's last movement.
        /// </summary>
        /// <remarks>
        /// If no movement occurred before interaction, a warning is logged instead.
        /// </remarks>
        private void HandleManualInteraction()
        {
            if (_lastMoveDirection == PlayerActions.None)
            {
                _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}.{nameof(HandleManualInteraction)}: Cannot interact – no last movement direction set.");
                return;
            }
            if (_player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}.{nameof(HandleManualInteraction)}: Cannot interact – Player is null.");
                return;
            }
            (int y, int x) interactPos = _player.Position;
            switch (_lastMoveDirection)
            {
                case PlayerActions.Up: interactPos.y--; break;
                case PlayerActions.Down: interactPos.y++; break;
                case PlayerActions.Left: interactPos.x--; break;
                case PlayerActions.Right: interactPos.x++; break;
            }

            _deps.Interaction.InteractionHandler(interactPos);
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(HandleManualInteraction)}: Manual interaction triggered at ({interactPos.y},{interactPos.x}).");
        }
    }
}