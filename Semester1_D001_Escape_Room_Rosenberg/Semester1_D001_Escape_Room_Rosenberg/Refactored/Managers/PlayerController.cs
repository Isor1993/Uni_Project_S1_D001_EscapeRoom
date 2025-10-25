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
    /// Controls player movement and interactions within the Escape Room game environment.
    /// </summary>
    /// <remarks>
    /// The <see cref="PlayerController"/> is responsible for translating user keyboard input 
    /// into player actions, updating positional data, and maintaining synchronization with 
    /// the player entity stored in the dependency system.  
    /// It supports movement in four directions, interaction with objects, and game quitting.
    /// </remarks>
    internal class PlayerController
    {
        // === Dependencies ===
        private readonly PlayerControllerDependencies _deps;

        // === Fields ===
       
        private PlayerActions _lastMoveDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class 
        /// with injected dependencies.
        /// </summary>
        /// <param name="playerControllerDependencies">
        /// Provides references to external systems required for player management,
        /// including the player data instance and related managers.
        /// </param>
        public PlayerController(PlayerControllerDependencies playerControllerDependencies)
        {
            _deps = playerControllerDependencies;
        }

        /// <summary>
        /// Defines all possible actions the player can perform through keyboard input.
        /// </summary>
        enum PlayerActions
        {
            None,
            Up,
            Down,
            Left,
            Right,
            Interact,
            Quit,
        }

        
        public (int y, int x) PlayerPosition => _deps.Player.Position;

        
        public void SetStartPosition()
        {            
            _lastMoveDirection = PlayerActions.None;

            _deps.Diagnostics.AddCheck($"{nameof(PlayerController)}.{nameof(SetStartPosition)}: Connected player position from PlayerInstance.");

        }

        
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

       
        public void MovePlayer(ConsoleKey key)
        {
            if (_deps.Player == null)
            {
                _deps.Diagnostics.AddError($"{nameof(PlayerController)}: Player reference missing!");
                return;
            }
            else if(_deps.GameBoard.GameBoardArray==null)
            {
                _deps.Diagnostics.AddError($"{nameof(PlayerController)}: GameBoardArray reference missing!");
                return;

            }
            else if(_deps.GameBoard==null)
            {
                _deps.Diagnostics.AddError($"{nameof(PlayerController)}: GameBoard reference missing!");
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
                        _deps.Diagnostics.AddWarning($"{nameof(PlayerController)}: Cannot interact – no last movement direction set.");
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
                    _deps.Diagnostics.AddCheck($"{nameof(PlayerController)}: Manual interaction triggered at ({interactPos.y},{interactPos.x}).");
                    return;
                    
                case PlayerActions.Quit:
                    _deps.Diagnostics.AddCheck($"{nameof(PlayerController)}: Quit command issued.");
                    return;

                default:
                    _deps.Diagnostics.AddWarning($"{nameof(PlayerController)}: No valid input detected.");
                    return;
            }

            bool canMove = _deps.Rule.IsMoveAllowed(newPosition);
            if (!canMove)
            {
                _deps.Diagnostics.AddWarning($"{nameof(PlayerController)}: Move blocked at {newPosition}.");
                return;
            }


            TileTyp tile =_deps.GameBoard.GetTileTyp(newPosition);
            if (tile != TileTyp.Empty)
            {
                _deps.Interaction.InteractionHandler(newPosition);
            }

            _deps.Player.AssignPosition(newPosition);
            _deps.Diagnostics.AddCheck($"{nameof(PlayerController)}: Player moved to ({newPosition.y},{newPosition.x}).");
        }

    }
}