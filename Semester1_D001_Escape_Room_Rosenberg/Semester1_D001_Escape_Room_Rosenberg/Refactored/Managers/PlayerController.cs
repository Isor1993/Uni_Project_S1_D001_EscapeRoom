using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Handles player movement and interactions based on keyboard input.
    /// </summary>
    /// <remarks>
    /// The <see cref="PlayerController"/> translates keyboard commands into in-game actions.  
    /// It validates player movement using <see cref="RulesManager"/>, updates player position,  
    /// and manages interactions through the <see cref="InteractionManager"/>.  
    /// Additionally, it logs all relevant actions to <see cref="DiagnosticsManager"/> for debugging.
    /// </remarks>
    internal class PlayerController
    {
        // === Dependencies ===
        private readonly PlayerControllerDependencies _deps;

        // === Fields ===

        private PlayerActions _lastMoveDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="playerControllerDependencies">
        /// Provides references to core managers required for player control,  
        /// including the <see cref="GameBoardManager"/>, <see cref="RulesManager"/>,  
        /// <see cref="InteractionManager"/>, and <see cref="DiagnosticsManager"/>.
        /// </param>
        public PlayerController(PlayerControllerDependencies playerControllerDependencies)
        {
            _deps = playerControllerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Initialized successfully.");
        }

        /// <summary>
        /// Defines all possible actions the player can perform via keyboard input.
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
        /// Gets the current position of the player on the game board.
        /// </summary>
        public (int y, int x) PlayerPosition => _deps.Player.Position;

        /// <summary>
        /// Sets the player’s initial position and resets movement direction tracking.
        /// </summary>
        /// <remarks>
        /// Typically called once after spawning or level start.  
        /// Ensures that movement and interaction states are properly initialized.
        /// </remarks>
        public void SetStartPosition()
        {
            _lastMoveDirection = PlayerActions.None;

            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(SetStartPosition)}: Connected player position from PlayerInstance.");

        }

        /// <summary>
        /// Determines the intended player action based on the given keyboard input.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKey"/> pressed by the user.</param>
        /// <returns>
        /// Returns a <see cref="PlayerActions"/> enum representing the intended movement or interaction.
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
        /// Moves the player or triggers interactions depending on keyboard input.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKey"/> representing the player’s action input.</param>
        /// <remarks>
        /// - Validates all required dependencies before processing movement.  
        /// - Prevents moves into invalid tiles by checking <see cref="RulesManager.IsMoveAllowed((int, int))"/>.  
        /// - Automatically triggers interactions via <see cref="InteractionManager"/> when encountering non-empty tiles.  
        /// - Logs every action, including blocked moves and invalid input, for debugging purposes.
        /// </remarks>
        public void MovePlayer(ConsoleKey key)
        {
            if (_deps.Player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: Player reference missing!");
                return;
            }
            else if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: GameBoardArray reference missing!");
                return;

            }
            else if (_deps.GameBoard == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: GameBoard reference missing!");
                return;

            }

            PlayerActions moveDirection = GetMoveDirection(key);
            (int y, int x) newPosition = _deps.Player.Position;

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
                    if (_lastMoveDirection == PlayerActions.None)
                    {
                        _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}: Cannot interact – no last movement direction set.");
                        return;
                    }
                    (int y, int x) interactPos = _deps.Player.Position;
                    switch (_lastMoveDirection)
                    {
                        case PlayerActions.Up: interactPos.y--; break;
                        case PlayerActions.Down: interactPos.y++; break;
                        case PlayerActions.Left: interactPos.x--; break;
                        case PlayerActions.Right: interactPos.x++; break;
                    }

                    _deps.Interaction.InteractionHandler(interactPos);
                    _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Manual interaction triggered at ({interactPos.y},{interactPos.x}).");
                    return;

                case PlayerActions.Quit:
                    _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Quit command issued.");
                    return;

                default:
                    _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}: No valid input detected.");
                    return;
            }

            bool canMove = _deps.Rule.IsMoveAllowed(newPosition);
            if (!canMove)
            {
                _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}: Move blocked at {newPosition}.");
                return;
            }

            TileTyp tile = _deps.GameBoard.GetTileTyp(newPosition);
            if (tile != TileTyp.Empty)
            {
                _deps.Interaction.InteractionHandler(newPosition);
            }

            _deps.Player.AssignPosition(newPosition);
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Player moved to ({newPosition.y},{newPosition.x}).");
        }
    }
}