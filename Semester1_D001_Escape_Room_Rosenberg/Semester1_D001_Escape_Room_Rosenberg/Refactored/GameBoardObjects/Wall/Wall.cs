using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall
{
    /// <summary>
    /// Represents a wall element in the game board.
    /// Provides access to all wall symbols and stores position and symbol data for each wall instance.
    /// </summary>
    internal class Wall
    {
        // Reference to the SymbolsManager, used to access predefined wall symbols.
        readonly SymbolsManager _symbolsManager;
        // Reference to the GameBoardBuilder, used for potential board-related operations.
        readonly GameBoardBuilder _gameBoardBuilder;
        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class with required dependencies.
        /// </summary>
        /// <param name="symbolsManager">Reference to the SymbolsManager that provides wall symbols.</param>
        /// <param name="gameBoardBuilder">Reference to the GameBoardBuilder for board management.</param>
        public Wall(SymbolsManager symbolsManager,GameBoardBuilder gameBoardBuilder) 
        {
            _symbolsManager = symbolsManager;
            _gameBoardBuilder=gameBoardBuilder;            
        }
        // Private fields to store wall properties.
        private char _symbol;
        private (int y, int x) _position;
        // Public read-only properties to access wall data.
        public char Symbol => _symbol;
        public (int y, int x) Position =>_position;

        /// <summary>
        /// Assigns the top wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTop()
        {
             _symbol = _symbolsManager.WallTopSymbol;
        }
        /// <summary>
        /// Assigns the side wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolSide()
        {
             _symbol= _symbolsManager.WallSideSymbol;
        }
        /// <summary>
        /// Assigns the cross wall symbol (used at intersections) to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCross()
        {
             _symbol = _symbolsManager.WallCrossSymbol;
        }
        /// <summary>
        /// Assigns the bottom-left corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerLeftBottom()
        {
             _symbol = _symbolsManager.WallLeftBottomCornerSymbol;
        }
        /// <summary>
        /// Assigns the top-left corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerLeftTop()
        {
             _symbol = _symbolsManager.WallLeftTopCornerSymbol;
        }
        /// <summary>
        /// Assigns the bottom-right corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerRightBottom()
        {
             _symbol = _symbolsManager.WallRightBottomCornerSymbol;
        }
        /// <summary>
        /// Assigns the top-right corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerRightTop()
        {
             _symbol = _symbolsManager.WallRightTopCornerSymbol;
        }
        /// <summary>
        /// Assigns the T-junction wall symbol facing downward.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTDown()
        {
             _symbol=_symbolsManager.WallTDownSymbol;
        }
        /// <summary>
        /// Assigns the T-junction wall symbol facing left.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTLeft()
        {
             _symbol=_symbolsManager.WallTLeftSymbol;
        }
        /// <summary>
        /// Assigns the T-junction wall symbol facing right.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTRight()
        {
             _symbol = _symbolsManager.WallTRightSymbol;
        }
        /// <summary>
        /// Assigns the T-junction wall symbol facing upward.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolUpSymbol()
        {
             _symbol = _symbolsManager.WallTUpSymbol;
        }
        /// <summary>
        /// Assigns a position to this wall element on the game board.
        /// </summary>
        /// <param name="y">The Y-coordinate of the wall's position.</param>
        /// <param name="x">The X-coordinate of the wall's position.</param>
        public void AssignPosition(int y, int x)
        {
            _position = (y, x);
        }
        /// <summary>
        /// Initializes the wall with a specific symbol and position on the game board.
        /// </summary>
        /// <param name="symbol">The wall symbol to assign.</param>
        /// <param name="y">The Y-coordinate of the wall's position.</param>
        /// <param name="x">The X-coordinate of the wall's position.</param>
        public void Initialize(char symbol,int y,int x)
        { 
            _symbol = symbol;
            _position = (y, x);
        }    
    }
}
