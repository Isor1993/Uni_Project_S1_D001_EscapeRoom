using Semester1_D001_Escape_Room_Rosenberg.Refactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class RulesManager
    {
        private readonly SymbolsManager _symbolsManager;
        private readonly GameBoardBuilder _gameBoardBuilder;
        private readonly PrintManager _printManager;


        public RulesManager(SymbolsManager symbols, GameBoardBuilder gameBoardBuilder, PrintManager printManager)
        {
            this._symbolsManager = symbols;
            this._gameBoardBuilder = gameBoardBuilder;
            this._printManager = printManager;
        }

        /// <summary>
        /// Determines whether there are no obstacles around the given position.
        /// </summary>
        /// <param name="position">The target position to check</param>
        /// <returns>True if all surrounding cells are empty; otherwise, false.</returns>
        public bool IsPositionFree((int y, int x) position)
        {
            // Get the game board array.
            char[,] board = _gameBoardBuilder.GameBoardArray;
            // Get the maximum board boundaries.
            int height = board.GetLength(0);
            int width = board.GetLength(1);
            // Iterate through all cells in a 3x3 area around the given position.
            for (int y = position.y - 1; y <= position.y + 1; y++)
            {
                for (int x = position.x - 1; x <= position.x + 1; x++)
                {
                    // Skip positions outside the board boundaries.
                    if (y < 0 || x < 0 || y >= height || x >= width)
                    {
                        continue;
                    }
                    // If any surrounding cell is not empty, the position is not free.
                    if (board[y, x] != _symbolsManager.EmptySymbol)
                    {
                        return false;
                    }
                }
            }
            // All surrounding cells are empty — position is free.
            return true;
        }
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
