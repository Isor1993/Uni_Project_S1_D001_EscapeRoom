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
        private readonly SymbolsManager _symbols;
        private readonly GameBoardBuilder _boardBuilder;
        private readonly PrinterManager _printer;


        public RulesManager(SymbolsManager symbols, GameBoardBuilder boardBuilder, PrinterManager printer)
        {
            _symbols = symbols;
            _boardBuilder = boardBuilder;
            _printer = printer;
        }

        /// <summary>
        /// Determines whether there are no obstacles around the given position.
        /// </summary>
        /// <param name="position">The target position to check</param>
        /// <returns>True if all surrounding cells are empty; otherwise, false.</returns>
        public bool IsPositionFree((int y, int x) position)
        {
            // Get the game board array.
            char[,] board = _boardBuilder.GameBoardArray;
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
                    if (board[y, x] != _symbols.EmptySymbol)
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
            char arrayCell = _boardBuilder.GameBoardArray[newPlayerPosition.y, newPlayerPosition.x];
            // Check if the cell contains a blocking symbol. If so, movement is not allowed.
            if (arrayCell == _symbols.WallTopSymbol ||
                arrayCell == _symbols.WallSideSymbol ||
                arrayCell == _symbols.WallRightTopCornerSymbol ||
                arrayCell == _symbols.WallRightBottomCornerSymbol ||
                arrayCell == _symbols.WallLeftTopCornerSymbol ||
                arrayCell == _symbols.WallLeftBottomCornerSymbol ||
                arrayCell == _symbols.QuestSymbol ||
                arrayCell == _symbols.PlayerSymbol ||
                arrayCell == _symbols.ClosedDoorTopWallSymbol ||
                arrayCell == _symbols.ClosedDoorSideWallSymbol ||
                arrayCell == _symbols.DeathSymbol)
            { return false; }
            // Check if the cell contains a valid movement symbol. If so, movement is allowed.
            else if (arrayCell == _symbols.OpenDoorTopWallSymbol ||
                arrayCell == _symbols.OpenDoorSideWallSymbol ||
                arrayCell == _symbols.KeyFragmentSymbol ||
                arrayCell == _symbols.EmptySymbol)
            { return true; }
            // Handle unexpected symbols: log an error and prevent movement.
            else
            {
                _printer.GetErrorMessage("Critical Error: IsMoveAllowed() => Unknown symbol encountered!");
                return false;
            }
        }

















    }
}
