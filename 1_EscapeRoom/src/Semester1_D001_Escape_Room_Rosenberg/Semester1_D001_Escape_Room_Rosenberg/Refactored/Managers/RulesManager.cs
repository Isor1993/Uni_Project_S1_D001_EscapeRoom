/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : RulesManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines and validates core logical rules for the Escape Room.
* Verifies player movement, spawn conditions, and object interaction rules.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Handles logical rules and interaction validation for the Escape Room game.
    /// Responsible for verifying movement, spawn positions, and potential interactions
    /// between entities and environment tiles.
    /// </summary>
    internal class RulesManager
    {
        // === Dependencies ===
        private readonly RulesManagerDependencies _deps;

        /// <summary>
        /// Initializes a new instance of the <see cref="RulesManager"/> with required dependencies.
        /// Logs a system check once initialization completes successfully.
        /// </summary>
        /// <param name="rulesManagerDependencies">
        /// Structured record providing references to <see cref="DiagnosticsManager"/> and <see cref="GameBoardManager"/>.
        /// </param>
        public RulesManager(RulesManagerDependencies rulesManagerDependencies)
        {
            _deps = rulesManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(RulesManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Checks if a target position and its 3×3 surrounding area are free
        /// from interactive or blocking tiles such as doors, players, NPCs, or keys.
        /// </summary>
        /// <param name="position">Target (y, x) coordinates to evaluate.</param>
        /// <returns>
        /// <remarks>
        /// This method relies on <see cref="TileType"/> classification 
        /// (Door, Player, Npc, Key, Empty) to decide spawn validity.
        /// </remarks>
        /// <c>true</c> if the area is clear for spawning; otherwise <c>false</c>.
        /// </returns>
        public bool IsPositionFreeForSpawn((int y, int x) position)
        {
            if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(RulesManager)}.{nameof(IsPositionFreeForSpawn)}: GameboardArray is null");
                return false;
            }
            // Retrieve the game board.
            TileType[,] board = _deps.GameBoard.GameBoardArray;
            int height = board.GetLength(0);
            int width = board.GetLength(1);

            // Iterate through all cells in the 3x3 area around the given position.
            for (int y = position.y - 1; y <= position.y + 1; y++)
            {
                for (int x = position.x - 1; x <= position.x + 1; x++)
                {
                    // Skip positions outside the board boundaries.
                    if (y < 0 || x < 0 || y >= height || x >= width)
                        continue;

                    //  Prevent spawning near any interactive object
                    if (board[y, x] == TileType.Door ||
                        board[y, x] == TileType.Player ||
                        board[y, x] == TileType.Npc ||
                        board[y, x] == TileType.Key)
                    {
                        _deps.Diagnostic.AddWarning($"{nameof(RulesManager)}.{nameof(IsPositionFreeForSpawn)}:  Position {position} rejected — too close to an active object {board[y, x]}.");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether movement to the specified target position is permitted
        /// based on the current <see cref="TileType"/> at that location.
        /// </summary>
        /// <param name="targetPosition">The board coordinates to move to (y, x).</param>
        /// <returns>
        /// <c>true</c> if the tile is walkable (empty or key); otherwise <c>false</c>.
        /// </returns>
        public bool IsMoveAllowed((int y, int x) targetPosition)
        {
            if (_deps.GameBoard.GameBoardArray == null)
            {

                _deps.Diagnostic.AddError($"{nameof(RulesManager)}.{nameof(IsMoveAllowed)}: GameboardArray is null");
                return false;

            }
            TileType typ = _deps.GameBoard.GameBoardArray[targetPosition.y, targetPosition.x];

            switch (typ)
            {
                case TileType.Key:
                    return true;

                case TileType.Empty:
                    return true;

                default:
                    _deps.Diagnostic.AddWarning($"{nameof(RulesManager)}.{nameof(IsMoveAllowed)}: All others posibillities are false");
                    return false;
            }
        }
    }
}