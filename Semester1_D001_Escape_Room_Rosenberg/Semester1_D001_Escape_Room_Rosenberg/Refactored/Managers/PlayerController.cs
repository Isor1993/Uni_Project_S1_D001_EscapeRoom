using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    ///  Handles all player-related input, movement, and interaction logic within the game board.
    /// </summary>
    /// <remarks>
    /// The <see cref="PlayerController"/> interprets keyboard input, validates movement permissions,  
    /// and triggers interactions via the <see cref="InteractionManager"/>.  
    /// It also updates the player position visually using the <see cref="PrintManager"/>  
    /// and logs all actions via the <see cref="DiagnosticsManager"/>.
    /// </remarks>
    internal class PlayerController
    {
        // === Dependencies ===
        private readonly PlayerControllerDependencies _deps;
        private  PlayerInstance? _player;

        // === Fields ===
        
        private PlayerActions _lastMoveDirection;

        /// <Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="playerControllerDependencies">
        /// Dependency container providing access to managers for diagnostics, interaction,  
        /// printing, game board management, and player data.
        /// </param>
        public PlayerController(PlayerControllerDependencies playerControllerDependencies)
        {
            _deps = playerControllerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Initialized successfully.");
            _player = _deps.GameObject.Player;
            if (_player==null)
            {

                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: PlayerInstance is null");
                return;
            }
        }

        /// <summary>
        /// Defines the available player actions recognized by the input system.
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
        /// Gets the current player position on the game board.
        /// </summary>
        public (int y, int x) PlayerPosition => _player.Position;

        public PlayerInstance? GetPlayer => _player;
        /// <summary>
        /// Connects the player position from the <see cref="PlayerInstance"/>  
        /// and initializes the movement state.
        /// </summary>
        public void SetStartPosition()
        {
            _lastMoveDirection = PlayerActions.None;

            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}.{nameof(SetStartPosition)}: Connected player position from PlayerInstance.");

        }

        /// <summary>
        /// Maps a pressed keyboard key to a corresponding player action.
        /// </summary>
        /// <param name="key">The input key received from the console.</param>
        /// <returns>
        /// Returns the mapped <see cref="PlayerActions"/> value based on the pressed key.  
        /// If no valid key is detected, <see cref="PlayerActions.None"/> is returned.
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
        /// Handles player movement and interaction logic based on key input.
        /// </summary>
        /// <param name="key">
        /// The <see cref="ConsoleKey"/> input to process.  
        /// Valid values include WASD, arrow keys, E/Enter for interaction, and Escape for quitting.
        /// </param>
        public void MovePlayer(ConsoleKey key)
        {
            if (_player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: Player reference missing!");
                return;
            }
            else if (_deps.GameBoard == null||_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: GameBoardArray reference missing!");
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
                    _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Quit command issued.");
                    Environment.Exit(0);
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

            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Player moved to ({newPosition.y},{newPosition.x}).");
        }

        /// <summary>
        /// Handles player-triggered manual interaction in the direction of the last movement.
        /// </summary>
        private void HandleManualInteraction()
        {
            if (_lastMoveDirection == PlayerActions.None)
            {
                _deps.Diagnostic.AddWarning($"{nameof(PlayerController)}: Cannot interact – no last movement direction set.");
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
            _deps.Diagnostic.AddCheck($"{nameof(PlayerController)}: Manual interaction triggered at ({interactPos.y},{interactPos.x}).");
        }
        
    }
}