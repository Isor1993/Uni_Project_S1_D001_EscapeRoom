using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// 
    /// </summary>
    internal class RulesManager
    {
        // === Dependencies ===
        private readonly SymbolsManager _symbolsManager;
        private readonly GameBoardManager _gameBoardBuilder;
        private readonly DiagnosticsManager _diagnosticsManager;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="gameBoardBuilder"></param>
        /// <param name="diagnosticsManager"></param>
        public RulesManager(SymbolsManager symbols, GameBoardManager gameBoardBuilder, DiagnosticsManager diagnosticsManager)
        {
            this._symbolsManager = symbols;
            this._gameBoardBuilder = gameBoardBuilder;
            this._diagnosticsManager = diagnosticsManager;
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
        public bool IsPositionFree((int y, int x) position)
        {
            if(_gameBoardBuilder.GameBoardArray==null)
            {
                _diagnosticsManager.AddCheck($"{nameof(RulesManager)}.{nameof(IsPositionFree)}: GameboardArray is null");
                return false;
            }
            // Retrieve the game board.
            TileTyp[,] board = _gameBoardBuilder.GameBoardArray;
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
                    if (board[y, x] == TileTyp.Door ||
                        board[y, x] == TileTyp.Player ||
                        board[y, x] == TileTyp.Npc ||
                        board[y, x] == TileTyp.Key)
                    {
                        _diagnosticsManager.AddWarning($"{nameof(RulesManager)}.{nameof(IsPositionFree)}:  Position {position} rejected — too close to an active object {board[y, x]}.");
                        return false;
                    }
                }
            }           
            return true;
        }



       //TODO  noch refactorn später
       // |
       // V
        /// <summary>
        /// Determines whether the player is allowed to move to the specified position.
        /// </summary>
        /// <param name="newPlayerPosition">The target position to check.</param>
        /// <returns>True if movement is allowed; otherwise, false.</returns>
        public bool IsMoveAllowed((int y, int x) newPlayerPosition)
        {
            // Retrieve the symbol at the target cell.
            char arrayCell = _gameBoardBuilder.GameBoardArray[newPlayerPosition.y, newPlayerPosition.x];
            // Check if the cell contains a blocking symbol. If so, movement is not allowed.
            if (arrayCell == _symbolsManager.WallHorizontalSymbol ||
                arrayCell == _symbolsManager.WallVerticalSymbol ||
                arrayCell == _symbolsManager.WallCornerTopRightSymbol ||
                arrayCell == _symbolsManager.WallCornerBottomRightSymbol ||
                arrayCell == _symbolsManager.WallCornerTopLeftSymbol ||
                arrayCell == _symbolsManager.WallCornerBottomLeftSymbol ||
                arrayCell == _symbolsManager.QuestSymbol ||
                arrayCell == _symbolsManager.PlayerSymbol ||
                arrayCell == _symbolsManager.ClosedDoorHorizontalSymbol ||
                arrayCell == _symbolsManager.ClosedDoorVerticalSymbol ||
                arrayCell == _symbolsManager.DeathSymbol)
            { return false; }
            // Check if the cell contains a valid movement symbol. If so, movement is allowed.
            else if (arrayCell == _symbolsManager.OpenDoororizontalSymbol ||
                arrayCell == _symbolsManager.OpenDoorVerticalSymbol ||
                arrayCell == _symbolsManager.KeyFragmentSymbol ||
                arrayCell == _symbolsManager.EmptySymbol)
            { return true; }
            // Handle unexpected symbols: log an error and prevent movement.
            else
            {
                _printManager.GetErrorMessage("Critical Error: IsMoveAllowed() => Unknown symbol encountered!");
                return false;
            }
        }

















    }
}
