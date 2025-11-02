using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// 
    /// </summary>
    internal class RulesManager
    {
        // === Dependencies ===
        private readonly RulesManagerDependencies _deps;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="gameBoardBuilder"></param>
        /// <param name="diagnosticsManager"></param>
        public RulesManager(RulesManagerDependencies rulesManagerDependencies)
        {
            _deps = rulesManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(RulesManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Checks whether a given position on the game board is free and suitable for spawning.
        /// The method verifies a 3x3 area around the specified coordinates to ensure that
        /// no interactive or blocking tiles (such as doors, players, NPCs, or key fragments)
        /// are present in the surrounding cells.
        /// </summary>
        /// <param name="position">The target position to evaluate, represented as (y, x) coordinates.</param>
        /// <returns>
        /// <c>true</c> if the position and its 3x3 surroundings are free of interactive tiles;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool IsPositionFreeForSpawn((int y, int x) position)
        {
            if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddCheck($"{nameof(RulesManager)}.{nameof(IsPositionFreeForSpawn)}: GameboardArray is null");
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
        /// 
        /// </summary>
        /// <param name="targetposition"></param>
        /// <returns></returns>
        public bool IsMoveAllowed((int y, int x) targetposition)
        {
            if (_deps.GameBoard.GameBoardArray == null)
            {

                _deps.Diagnostic.AddError($"{nameof(RulesManager)}.{nameof(IsMoveAllowed)}: GameboardArray is null");
                return false;

            }
            TileType typ = _deps.GameBoard.GameBoardArray[targetposition.y, targetposition.x];

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

        //TODO mögliche Methoden die man braucht für interaction
        public void IsDoorOpenable()
        {

        }

        //TODO mögliche Methoden die man braucht für interaction
        public void IsNpcInteractable()
        {

        }

        //TODO mögliche Methoden die man braucht für interaction
        public void IsInsideBounds((int y, int x) position)
        {

        }
    }
}

