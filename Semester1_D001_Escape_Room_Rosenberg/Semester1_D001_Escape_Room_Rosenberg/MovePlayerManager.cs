using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class MovePlayerManager
    {
        private readonly SpawnerManager _spawner;
        private readonly GameBoardBuilder _boardBuilder;
        private readonly RulesManager _rules;
        private readonly SymbolsManager _symbols;
        public MovePlayerManager(SpawnerManager spawner, GameBoardBuilder boardBuilder, RulesManager rules,SymbolsManager symbols)
        {
            _spawner = spawner;
            _boardBuilder = boardBuilder;
            _rules = rules;
            _symbols = symbols;
        }
        // Defines the possible movement directions for the player and interaction.
        enum PlayerMove
        {
            None,
            Up,
            Down,
            Left,
            Right,
            Interact,
        }

        /// <summary>
        /// Stores the current position of the player on the board.
        /// </summary>
        private (int y, int x) _playerPosition;
        /// <summary>
        /// Provides read-only access to the player’s current position.
        /// </summary>
        public (int y, int x) PlayerPosition => _playerPosition;

        /// <summary>
        /// Sets the player's initial spawn position.
        /// </summary>
        /// <param name="spawnPosition">The starting coordinates of the player.</param>
        public void SetStartPosition((int y, int x) spawnPosition)
        {
            _playerPosition =spawnPosition;
        }
        /// <summary>
        /// Converts a pressed keyboard key into a movement direction.
        /// </summary>
        /// <param name="key">The key pressed by the player.</param>
        /// <returns>The corresponding PlayerMove direction.</returns>
        private PlayerMove GetMoveDirection(ConsoleKey key)
        {
            // Map input keys to movement directions.
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return PlayerMove.Up;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return PlayerMove.Down;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return PlayerMove.Left;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return PlayerMove.Right;
                case ConsoleKey.E:
                case ConsoleKey.Enter:
                    return PlayerMove.Interact;
                default:
                    return PlayerMove.None;
            }
        }
        /// <summary>
        /// Moves the player based on the given key input, if the target position is allowed.
        /// </summary>
        /// <param name="key">The key pressed by the player to trigger movement.</param>
        public void MovePlayer(ConsoleKey key)

        {
            // Determine the desired move direction.
            PlayerMove moveDirection = GetMoveDirection(key);
            // Copy the current player position as a base for movement calculation.
            (int y, int x) newPosition = _playerPosition;
            // Adjust the position based on the chosen direction.
            switch (moveDirection)
            {
                case PlayerMove.Up:
                    newPosition.y--;
                    break;

                case PlayerMove.Down:
                    newPosition.y++;
                    break;

                case PlayerMove.Left:
                    newPosition.x--;
                    break;

                case PlayerMove.Right:
                    newPosition.x++;
                    break;
            }
            // Check if the new position is valid according to game rules.
            if (_rules.IsMoveAllowed(newPosition))
            {
                // Clear the player's old position on the console and draw new empty symbol.
                Console.SetCursorPosition(_playerPosition.x,_playerPosition.y);
                Console.Write(_symbols.EmptySymbol);
                // Draw the player at the new position.
                Console.SetCursorPosition(newPosition.x,newPosition.y);
                Console.Write(_symbols.PlayerSymbol);
                // Update the game board data array.
                _boardBuilder.GameBoardArray[_playerPosition.y, _playerPosition.x] = _symbols.EmptySymbol;
                _boardBuilder.GameBoardArray[newPosition.y, newPosition.x] = _symbols.PlayerSymbol;
                // Save the new position as the player's current position.
                _playerPosition = newPosition;
            }









        }





    }
}